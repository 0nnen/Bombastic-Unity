using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

[System.Serializable]
public class BallSettings
{
    [SerializeField, Tooltip("Delais Minimum avant l'explosion de la balle")]
    private float minExplosionDelay = 3.0f;

    [SerializeField, Tooltip("Delais Maximum avant l'explosion de la balle")]
    private float maxExplosionDelay = 5.0f;

    [SerializeField, Tooltip("Temps dt'attente de respawn de la balle")]
    private float respawnDelay = 3.0f;

    [SerializeField, Tooltip("Distance pour attraper la balle")]
    private float pickupDistance = 5.0f;

    [SerializeField, Tooltip("Force lorsque la balle est lancée")]
    private float throwForce = 50.0f;

    [SerializeField, Tooltip("Effet lorsque la balle explose")]
    private GameObject explosionEffectsContainer;

    public float MinExplosionDelay => minExplosionDelay;
    public float MaxExplosionDelay => maxExplosionDelay;
    public float RespawnDelay => respawnDelay;
    public float PickupDistance => pickupDistance;
    public float ThrowForce => throwForce;
    public GameObject ExplosionEffectsContainer => explosionEffectsContainer;
}

public class BallManager : MonoBehaviour
{
    [SerializeField] private BallSettings ballSettings;
    [SerializeField] private Camera player1Camera;
    [SerializeField] private Camera player2Camera;

    private MovementController playerMovementController;
    private Camera currentCamera;
    private Rigidbody rb;
    private Renderer ballRenderer;
    private Transform playerTransform;
    private Vector3 initialPosition;
    private bool isPickedUp = false;
    private Transform ballHoldPosition;
    private int BallInCamp;

    void Start()
    {
        InitializeComponents();
        initialPosition = transform.position;
        ScheduleExplosion();
    }

    // Appelé au début de chaque round
    public void StartRound()
    {
        ResetBallPosition();
        ScheduleExplosion();
    }


    void LateUpdate()
    {
        HandlePlayerInput();
        if (isPickedUp) UpdateBallPosition();
    }

    // Initialisation des composants essentiels
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        ballRenderer = GetComponent<Renderer>();
        initialPosition = transform.position;
        ballHoldPosition = new GameObject("BallHoldPosition").transform;
    }

    // Gestion des entrees pour les deux joueurs
    private void HandlePlayerInput()
    {
        if (Input.GetButtonDown("FireMouse")) HandleInteraction(1);
        else if (Input.GetButtonDown("FireGamepad"))
        {
            HandleInteraction(2);
        };
    }

    // Gere l'interaction en fonction de l'ID du joueur
    private void HandleInteraction(int playerId)
    {
        if (!isPickedUp) TryPickUp(playerId);
        else if (IsPlayerHoldingBall(playerId)) ThrowBall();
    }

    // Verifie si le joueur tient la balle
    private bool IsPlayerHoldingBall(int playerId)
    {
        return playerTransform && playerTransform.GetComponent<MovementController>().PlayerId == playerId;
    }

    // Ramasse la balle
    private void PickUpBall(Collider hitCollider, int playerId)
    {
        isPickedUp = true;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        playerTransform = hitCollider.transform;
        currentCamera = (playerId == 1) ? player1Camera : player2Camera;

        playerMovementController = hitCollider.GetComponent<MovementController>();
    }

    // Essaie de ramasser la balle
    private void TryPickUp(int playerId)
    {
        // Sphere autour de la balle pour detecter les joueurs a proximite
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ballSettings.PickupDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (IsPickupConditionMet(hitCollider, playerId))
            {
                PickUpBall(hitCollider, playerId);
                break;
            }
        }
    }

    // Verifie si les conditions de ramassage sont remplies
    private bool IsPickupConditionMet(Collider hitCollider, int playerId)
    {
        return hitCollider.CompareTag("Player") && hitCollider.GetComponent<MovementController>().PlayerId == playerId;
    }

    // Mise a jour de la position de la balle lorsqu'elle est tenue
    private void UpdateBallPosition()
    {
        // Calcule la position devant le joueur en se basant sur sa direction actuelle
        Vector3 positionInFront = playerTransform.position + playerTransform.forward * 4f;

        // Calcule la position au-dessus du sol pour que la balle soit tenue a une hauteur realiste
        Vector3 positionAbove = Vector3.up * 4.5f;

        // Definit la position de la balle
        ballHoldPosition.position = positionInFront + positionAbove;

        // Ajuste la rotation de la balle pour qu'elle corresponde a la rotation de la camera et du joueur
        ballHoldPosition.rotation = Quaternion.Euler(currentCamera.transform.eulerAngles.x, playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);
        AttachToPlayer();
    }

    // Attache la balle au joueur
    private void AttachToPlayer()
    {
        // MovePosition pour une mise a jour plus douce
        rb.MovePosition(ballHoldPosition.position);
        rb.MoveRotation(ballHoldPosition.rotation);
    }

    // Lance la balle
    private void ThrowBall()
    {
        if (playerTransform && currentCamera != null && playerMovementController != null)
        {
            ReleaseBall();

            // Calculez la force du lancer basée sur l'endurance restante
            // Inversez la proportion pour que moins d'endurance donne moins de force
            float staminaProportion = 1 - (playerMovementController.CurrentStamina / playerMovementController.StaminaSettings.MaxStamina);
            float scaledThrowForce = ballSettings.ThrowForce * (1 - staminaProportion);

            // Direction du lancer
            Vector3 forwardDirection = currentCamera.transform.forward;
            Vector3 throwDirection = (forwardDirection + Vector3.up * 0.1f).normalized;

            // Appliquez la force
            Debug.Log("Throw force: " + scaledThrowForce);
            rb.AddForce(throwDirection * scaledThrowForce, ForceMode.VelocityChange);

        }
        else
        {
            Debug.LogError("ThrowBall appelée mais currentCamera, playerTransform ou playerMovementController est null.");
        }
    }

    // Libere la balle
    private void ReleaseBall()
    {
        isPickedUp = false;
        rb.isKinematic = false;
        GetComponent<Collider>().enabled = true;
        playerTransform = null;
    }




    // Methodes pour la gestion de l'explosion et de la reapparition de la balle
    // Planifie l'explosion apres un delai
    private void Explode()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Parcourez tous les joueurs
        if (BallInCamp == 1)
        {
            GetComponent<Score>().IncreasePlayer2Score();
            Debug.Log("Ball Explodes in camp 1, Player 2 Win");
        }
        else if (BallInCamp == 2)
        {
            GetComponent<Score>().IncreasePlayer1Score();
            Debug.Log("Ball Explodes in camp 2, Player 1 Win");
        }
        else
        {
            Debug.Log("Où est la balle ???");
        }
        Debug.Log("Explode called");
        GameObject explosion = Instantiate(ballSettings.ExplosionEffectsContainer, transform.position, Quaternion.identity);
        Destroy(explosion, 2.0f);
        StartCoroutine(RespawnAndScheduleNextExplosion());
    }

    public void ResetBallPosition()
    {
        transform.position = initialPosition;
        isPickedUp = false;
        rb.isKinematic = false;
        ballRenderer.enabled = true;
    }

    private IEnumerator RespawnAndScheduleNextExplosion()
    {
        // Explosion et désactivation de la balle
        ballRenderer.enabled = false;
        rb.isKinematic = true;
        isPickedUp = false;

        // Attend le délai de réapparition avant de réinitialiser la balle
        yield return new WaitForSeconds(ballSettings.RespawnDelay);
        ResetBallPosition();
        // Planifier la prochaine explosion pour le nouveau round
        ScheduleExplosion();
    }

    private void ScheduleExplosion()
    {
        // Annule toute explosion précédente planifiée pour éviter les doubles explosions
        CancelInvoke("Explode");

        // Planifier une nouvelle explosion avec un délai aléatoire
        float randomDelay = Random.Range(ballSettings.MinExplosionDelay, ballSettings.MaxExplosionDelay);
        Debug.Log("Explosion scheduled in " + randomDelay + " seconds");
        Invoke("Explode", randomDelay);
    }


    public void SetCamp(int _camp)
    {
        BallInCamp = _camp;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isPickedUp)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Explode();
            }
        }
    }
}