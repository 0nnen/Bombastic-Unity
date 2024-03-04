using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[System.Serializable]
public class BallSettings
{
    [SerializeField, Tooltip("Minimum delay before the ball can explode.")]
    private float minExplosionDelay = 10.0f;

    [SerializeField, Tooltip("Maximum delay before the ball can explode.")]
    private float maxExplosionDelay = 60.0f;

    [SerializeField, Tooltip("Time to wait before respawning the ball.")]
    private float respawnDelay = 3.0f;

    [SerializeField, Tooltip("How close must the player be to pick up the ball.")]
    private float pickupDistance = 5.0f;

    [SerializeField, Tooltip("The force applied when the ball is thrown.")]
    private float throwForce = 50.0f;

    [SerializeField, Tooltip("The effects to show when the ball explodes.")]
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

    // Gestion des entr�es pour les deux joueurs
    private void HandlePlayerInput()
    {
        if (Input.GetButtonDown("FireMouse")) HandleInteraction(1);
        else if (Input.GetButtonDown("FireGamepad")) HandleInteraction(2);
    }

    // G�re l'interaction en fonction de l'ID du joueur
    private void HandleInteraction(int playerId)
    {
        if (!isPickedUp) TryPickUp(playerId);
        else if (IsPlayerHoldingBall(playerId)) ThrowBall();
    }

    // V�rifie si le joueur tient la balle
    private bool IsPlayerHoldingBall(int playerId)
    {
        return playerTransform && playerTransform.GetComponent<MovementController>().PlayerId == playerId;
    }

    // Essaie de ramasser la balle
    private void TryPickUp(int playerId)
    {
        // Sph�re autour de la balle pour d�tecter les joueurs � proximit�
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

    // V�rifie si les conditions de ramassage sont remplies
    private bool IsPickupConditionMet(Collider hitCollider, int playerId)
    {
        return hitCollider.CompareTag("Player") && hitCollider.GetComponent<MovementController>().PlayerId == playerId;
    }

    // Ramasse la balle
    private void PickUpBall(Collider hitCollider, int playerId)
    {
        isPickedUp = true;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        playerTransform = hitCollider.transform;
        currentCamera = (playerId == 1) ? player1Camera : player2Camera;
    }

    // Mise � jour de la position de la balle lorsqu'elle est tenue
    private void UpdateBallPosition()
    {
        // Calcule la position devant le joueur en se basant sur sa direction actuelle
        Vector3 positionInFront = playerTransform.position + playerTransform.forward * 3f;

        // Calcule la position au-dessus du sol pour que la balle soit tenue � une hauteur r�aliste
        Vector3 positionAbove = Vector3.up * 3.5f;

        // D�finit la position de la balle
        ballHoldPosition.position = positionInFront + positionAbove;

        // Ajuste la rotation de la balle pour qu'elle corresponde � la rotation de la cam�ra et du joueur
        ballHoldPosition.rotation = Quaternion.Euler(currentCamera.transform.eulerAngles.x, playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);
        AttachToPlayer();
    }

    // Attache la balle au joueur
    private void AttachToPlayer()
    {
        // MovePosition pour une mise � jour plus douce
        rb.MovePosition(ballHoldPosition.position);
        rb.MoveRotation(ballHoldPosition.rotation);
    }

    // Lance la balle
    private void ThrowBall()
    {
        if (playerTransform && currentCamera != null)
        {
            ReleaseBall();

            // Direction de la camera
            Vector3 forwardDirection = currentCamera.transform.forward;

            // Trajectoire de lancer
            Vector3 throwDirection = (forwardDirection + Vector3.up * 0.3f).normalized;

            // Applique la force dans la direction du lancer
            rb.AddForce(throwDirection * ballSettings.ThrowForce, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogError("ThrowBall called but currentCamera or playerTransform is null.");
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




    // M�thodes pour la gestion de l'explosion et de la r�apparition de la balle
    // Planifie l'explosion apr�s un d�lai
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
            Debug.Log("Ou est la balle");
        }
        GameObject explosion = Instantiate(ballSettings.ExplosionEffectsContainer, transform.position, Quaternion.identity);
        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        // D�sactivation et r�activation de la balle
        ballRenderer.enabled = false;
        rb.isKinematic = true;
        yield return new WaitForSeconds(ballSettings.RespawnDelay);
        ResetBallPosition();
        ballRenderer.enabled = true;
        rb.isKinematic = false;
    }

    public void ResetBallPosition()
    {
        transform.position = initialPosition; // R�initialise la position de la balle
        isPickedUp = false; // Assure que la balle n'est pas consid�r�e comme ramass�e
    }

    private void ScheduleExplosion()
    {
        float randomDelay = Random.Range(ballSettings.MinExplosionDelay, ballSettings.MaxExplosionDelay);
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