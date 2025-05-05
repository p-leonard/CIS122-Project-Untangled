using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity Settings")]
    public float mouseSensitivity = 100f;

    [Header("Camera")]
    public Transform playerBody; // reference to Player GameObject
    private float xRotation = 0f;

    private PlayerControls controls;
    private Vector2 lookInput;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        Vector2 mouseDelta = lookInput * mouseSensitivity * Time.deltaTime;

        // Vertical camera (pitch)
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal player rotation (yaw)
        playerBody.Rotate(Vector3.up * mouseDelta.x);
    }
}
