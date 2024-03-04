using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMovementSettings
{
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float maxJumpHeight = 3.0f;
    [SerializeField] private float maxJumpTime = 1.5f;

    public float RunSpeed => runSpeed;
    public float WalkSpeed => walkSpeed;
    public float MaxJumpHeight => maxJumpHeight;
    public float MaxJumpTime => maxJumpTime;
}

[System.Serializable]
public class PlayerStaminaSettings
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDepletionRate = 10f;
    [SerializeField] private float staminaRecoveryRate = 10f;

    public float MaxStamina => maxStamina;
    public float StaminaDepletionRate => staminaDepletionRate;
    public float StaminaRecoveryRate => staminaRecoveryRate;
}

[System.Serializable]
public class PlayerMagicSettings
{
    [SerializeField] private float maxMagic = 100f;
    [SerializeField] private float magicDepletionRate = 5f;
    [SerializeField] private float magicRecoveryRate = 10f;

    public float MaxMagic => maxMagic;
    public float MagicDepletionRate => magicDepletionRate;
    public float MagicRecoveryRate => magicRecoveryRate;
}

[System.Serializable]
public class PlayerJetpackSettings
{
    [SerializeField] private float jetpackForce = 5f;
    [SerializeField] private float jetpackMagicCost = 10f;

    public float JetpackForce => jetpackForce;
    public float JetpackMagicCost => jetpackMagicCost;
}

public class MovementController : MonoBehaviour
{
    [SerializeField] private PlayerMovementSettings movementSettings;
    [SerializeField] private PlayerStaminaSettings staminaSettings;
    [SerializeField] private PlayerMagicSettings magicSettings;
    [SerializeField] private PlayerJetpackSettings jetpackSettings;

    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private int playerId;

    public int PlayerId => playerId;
    public string HorizontalAxis => horizontalAxis;
    public string VerticalAxis => verticalAxis;


    private CharacterController characterController;
    private Vector3 currentMovement;
    private float currentStamina;
    private float currentMagic;
    private float gravity;
    private float groundedGravity = -0.05f;
    private float initialJumpVelocity;
    private bool isRunning;
    private bool isJumping;
    private bool isUsingJetpack;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentStamina = staminaSettings.MaxStamina;
        currentMagic = magicSettings.MaxMagic;
        SetupJumpVariables();
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleJumpingAndGravity();
        HandleRunning();
        HandleJetpack();
        RecoverStamina();
        RecoverMagic();
    }

    // Récupère les entrées de l'utilisateur et gère le mouvement, le saut et le jetpack
    private void HandleInput()
    {
        isRunning = (playerId == 1 && Input.GetKey(KeyCode.LeftShift)) ||
                    (playerId == 2 && Input.GetKey(KeyCode.Keypad1)) ||
                    Input.GetButton("RunGamepad");

        isJumping = (playerId == 1 && Input.GetKeyDown(KeyCode.Space)) ||
                    (playerId == 2 && Input.GetKeyDown(KeyCode.Keypad0)) ||
                    Input.GetButtonDown("JumpGamepad");

        // Contrôles séparés pour le jetpack
        if (playerId == 1)
        {
            isUsingJetpack = currentMagic > 0 && Input.GetButton("JetpackMouse");
        }
        else if (playerId == 2)
        {
            isUsingJetpack = currentMagic > 0 && Input.GetButton("JetpackGamepad");
        }
    }

    // Gère le mouvement horizontal et vertical du joueur
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis(horizontalAxis) + (PlayerId == 2 ? Input.GetAxis("HorizontalGamepad") : 0);
        float verticalInput = Input.GetAxis(verticalAxis) + (PlayerId == 2 ? Input.GetAxis("VerticalGamepad") : 0);

        // Calcule la direction du mouvement
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection *= isRunning && currentStamina > 0 ? movementSettings.RunSpeed : movementSettings.WalkSpeed;
        currentMovement.x = moveDirection.x;
        currentMovement.z = moveDirection.z;

        // Applique le mouvement
        characterController.Move(currentMovement * Time.deltaTime);
    }

    // Gère la logique de saut et d'application de la gravité
    private void HandleJumpingAndGravity()
    {
        if (characterController.isGrounded && isJumping)
        {
            // Applique la vélocité initiale de saut
            currentMovement.y = initialJumpVelocity;
        }
        else if (!characterController.isGrounded || isUsingJetpack)
        {
            // Applique la gravité ou la force du jetpack
            currentMovement.y += (isUsingJetpack ? jetpackSettings.JetpackForce : gravity) * Time.deltaTime;
        }
        else
        {
            // Applique une petite gravité quand au sol pour maintenir le personnage sur le sol
            currentMovement.y = groundedGravity;
        }
    }

    // Diminue l'endurance lors de la course et la récupère lorsque le joueur ne court pas
    private void HandleRunning()
    {
        if (isRunning)
        {
            // Stamina se vide en 5 secondes
            currentStamina -= staminaSettings.MaxStamina / 5f * Time.deltaTime;
        }
        else if (currentStamina < staminaSettings.MaxStamina)
        {
            currentStamina += staminaSettings.StaminaRecoveryRate * Time.deltaTime;
        }
        currentStamina = Mathf.Clamp(currentStamina, 0, staminaSettings.MaxStamina);
    }

    // Gère le jetpack en utilisant la magie
    private void HandleJetpack()
    {
        // Activation du jetpack pour chaque joueur séparément
        isUsingJetpack = (playerId == 1 && currentMagic > 0 && Input.GetButton("JetpackMouse")) ||
                         (playerId == 2 && currentMagic > 0 && Input.GetButton("JetpackGamepad"));

        if (isUsingJetpack)
        {
            // La magie se vide en 3 secondes
            currentMagic -= magicSettings.MaxMagic / 3f * Time.deltaTime;
            // Applique la force du jetpack
            currentMovement.y += jetpackSettings.JetpackForce * Time.deltaTime;
        }
        // Applique la gravité si le jetpack n'est pas utilisé
        else if (!characterController.isGrounded)
        {
            currentMovement.y += gravity * Time.deltaTime;
        }
    }

    // Récupère l'endurance si le joueur ne court pas
    private void RecoverStamina()
    {
        if (!isRunning && currentStamina < staminaSettings.MaxStamina)
        {
            currentStamina += staminaSettings.StaminaRecoveryRate * Time.deltaTime;
        }
    }

    // Récupère la magie si le jetpack n'est pas utilisé
    private void RecoverMagic()
    {
        if (!isUsingJetpack && currentMagic < magicSettings.MaxMagic)
        {
            currentMagic += magicSettings.MagicRecoveryRate * Time.deltaTime;
        }
    }

    // Configure les variables nécessaires pour calculer la force de saut et la gravité
    private void SetupJumpVariables()
    {
        float timeToApex = movementSettings.MaxJumpTime / 2;
        gravity = (-2 * movementSettings.MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * movementSettings.MaxJumpHeight) / timeToApex;
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
