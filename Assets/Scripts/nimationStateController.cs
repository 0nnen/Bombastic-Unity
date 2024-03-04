using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private MovementController movementController;

    private float smoothTime = 0.1f; // Lissage pour transition des vitesses d'animation
    private float maxWalkVelocity = 5.0f;
    private float maxRunVelocity = 10.0f;

    Animator animator;
    int velocityXHash;
    int velocityZHash;
    int isJumpingHash;

    float currentVelocityZ;
    float currentVelocityX;

    void Start()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
        velocityXHash = Animator.StringToHash("Velocity X");
        velocityZHash = Animator.StringToHash("Velocity Z");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    void Update()
    {
        ProcessInput(out bool isRunning, out bool isJumping);
        UpdateAnimator(isRunning, isJumping);
    }

    // Traitement des entrées clavier pour les déplacements et les actions
    void ProcessInput(out bool isRunning, out bool isJumping)
    {
        isRunning = Input.GetKey(movementController.PlayerId == 1 ? KeyCode.LeftShift : KeyCode.Keypad3) || Input.GetButton("RunGamepad");
        isJumping = Input.GetKeyDown(movementController.PlayerId == 1 ? KeyCode.Space : KeyCode.Keypad0) || Input.GetButtonDown("JumpGamepad");

        // Calcul des vitesses cibles basées sur les touches pressées
        float targetVelocityZ = GetAxisRawVertical() * (isRunning ? maxRunVelocity : maxWalkVelocity);
        float targetVelocityX = GetAxisRawHorizontal() * (isRunning ? maxRunVelocity : maxWalkVelocity);

        // Changement progresssif des vitesses actuelles vers les vitesses cibles
        currentVelocityZ = Mathf.SmoothDamp(animator.GetFloat(velocityZHash), targetVelocityZ, ref currentVelocityZ, smoothTime);
        currentVelocityX = Mathf.SmoothDamp(animator.GetFloat(velocityXHash), targetVelocityX, ref currentVelocityX, smoothTime);
    }

    // Mise à jour de l'animateur avec les vitesses calculées et l'état de saut
    void UpdateAnimator(bool isRunning, bool isJumping)
    {
        animator.SetFloat(velocityXHash, currentVelocityX);
        animator.SetFloat(velocityZHash, currentVelocityZ);
        animator.SetBool(isJumpingHash, isJumping);
    }

    // Input vertical: Valeur brute (-1, 0, 1)
    float GetAxisRawVertical()
    {
        if (movementController.PlayerId == 1)
        {
            return Input.GetAxisRaw(movementController.VerticalAxis);
        }
        else
        {
            float verticalGamepad = Input.GetAxisRaw("VerticalGamepad");
            float verticalKeyboard = Input.GetAxisRaw("VerticalPlayer2");
            return Mathf.Abs(verticalGamepad) > 0.1f ? verticalGamepad : verticalKeyboard;
        }
    }

    // Input horizontal: Valeur brute (-1, 0, 1)
    float GetAxisRawHorizontal()
    {
        if (movementController.PlayerId == 1)
        {
            return Input.GetAxisRaw(movementController.HorizontalAxis);
        }
        else
        {
            float horizontalGamepad = Input.GetAxisRaw("HorizontalGamepad");
            float horizontalKeyboard = Input.GetAxisRaw("HorizontalPlayer2");
            return Mathf.Abs(horizontalGamepad) > 0.1f ? horizontalGamepad : horizontalKeyboard;
        }
    }
}
