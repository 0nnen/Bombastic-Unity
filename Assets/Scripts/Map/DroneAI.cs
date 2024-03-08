using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public Transform ball;
    public float speed = 5.0f;
    public float catchDistance = 6.0f;
    public float evasionDistance = 3.0f;
    public Vector3 areaSize = new Vector3(50, 10, 50);
    public float catchDecisionTime = 10.0f;  // Time between catch decisions
    public float holdTime = 5.0f;  // Time to hold the ball

    private bool hasBall = false;
    private Rigidbody ballRigidbody;
    private Transform targetLocation;
    private GameObject[] players;
    private float catchDecisionTimer = 0.0f;
    private float holdTimer = 0.0f;

    void Start()
    {
        ballRigidbody = ball.GetComponent<Rigidbody>();
        targetLocation = new GameObject("TargetLocation").transform;
        players = GameObject.FindGameObjectsWithTag("Player");
        SetRandomTargetLocation();
        catchDecisionTimer = catchDecisionTime;
    }

    void Update()
    {
        if (hasBall)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                ReleaseBall();
                SetRandomTargetLocation();
                holdTimer = 0.0f;
            }
            else
            {
                // Move towards the target location while holding the ball
                MoveTowards(targetLocation.position);
            }
        }
        else
        {
            catchDecisionTimer -= Time.deltaTime;
            if (catchDecisionTimer <= 0)
            {
                catchDecisionTimer = catchDecisionTime;
                DecideToCatchBall();
            }

            float distanceToBall = Vector3.Distance(transform.position, ball.position);

            // Evasion of players
            foreach (var player in players)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < evasionDistance)
                {
                    Evade(player.transform.position);
                    return;
                }
            }

            // Move towards the ball
            MoveTowards(ball.position);
        }
    }

    void DecideToCatchBall()
    {
        if (Random.value > 0.5) // 50% chance to decide to catch the ball
        {
            float distanceToBall = Vector3.Distance(transform.position, ball.position);
            if (distanceToBall <= catchDistance)
            {
                CatchBall();
            }
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void Evade(Vector3 threatPosition)
    {
        Vector3 direction = (transform.position - threatPosition).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void CatchBall()
    {
        ball.SetParent(transform);
        ballRigidbody.isKinematic = true;
        hasBall = true;
    }

    void ReleaseBall()
    {
        ball.SetParent(null);
        ballRigidbody.isKinematic = false;
        hasBall = false;
    }

    void SetRandomTargetLocation()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0,
            Random.Range(-areaSize.z / 2, areaSize.z / 2)
        );

        targetLocation.position = transform.position + randomPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
