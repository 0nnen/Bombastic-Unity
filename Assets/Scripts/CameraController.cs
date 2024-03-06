using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FovSettings
{
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float runningFOV = 80f;
    [SerializeField] private float fovChangeSpeed = 5f;

    public float NormalFOV => normalFOV;
    public float RunningFOV => runningFOV;
    public float FovChangeSpeed => fovChangeSpeed;
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private FovSettings fovSettings;
    [SerializeField] private int playerId;
    [SerializeField] private float mouseSensitivity = 100.0f; // Sensibilité de la souris
    [SerializeField] private float controllerSensitivity = 100.0f; // Sensibilité du gamepad

    [SerializeField] private MovementController movementController;



    private Camera Pcamera;
    private float xRotation = 0f; // Rotation sur l'axe X pour regarder vers le haut et le bas

    void Start()
    {
        Pcamera = GetComponent<Camera>();
        if (playerId == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (playerId == 1)
        {
            HandleMouseLook();
            UpdateCameraFOV();
        }
        else if (playerId == 2)
        {
            HandleGamepadLook();
            UpdateCameraFOV();
        }

    }

    // Gestion de la rotation de la caméra avec la souris
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        ApplyRotation(mouseX, mouseY);
    }

    // Gestion de la rotation de la caméra avec le gamepad
    void HandleGamepadLook()
    {
        float gamepadX = Input.GetAxis("GamepadRightStickX") * controllerSensitivity * Time.deltaTime;
        float gamepadY = Input.GetAxis("GamepadRightStickY") * controllerSensitivity * Time.deltaTime;

        ApplyRotation(gamepadX, gamepadY);
    }

    // Applique la rotation calculée à la caméra
    void ApplyRotation(float horizontal, float vertical)
    {
        xRotation -= vertical;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limiter la rotation verticale

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotation verticale
        transform.parent.Rotate(Vector3.up * horizontal); // Rotation horizontale
    }



    private void UpdateCameraFOV()
    {
        if (movementController.IsRunning())
        {
            Debug.Log("Player is running.");
            Pcamera.fieldOfView = Mathf.Lerp(Pcamera.fieldOfView, fovSettings.RunningFOV, fovSettings.FovChangeSpeed * Time.deltaTime);
        }
        else
        {
            Pcamera.fieldOfView = Mathf.Lerp(Pcamera.fieldOfView, fovSettings.NormalFOV, fovSettings.FovChangeSpeed * Time.deltaTime);
        }
    }
}
