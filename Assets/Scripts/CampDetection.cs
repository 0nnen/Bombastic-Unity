using UnityEngine;

public class CampDetection : MonoBehaviour
{
    public int camp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BALL"))
        {
            other.gameObject.GetComponent<BallManager>().SetCamp(camp);
        }
    }
}