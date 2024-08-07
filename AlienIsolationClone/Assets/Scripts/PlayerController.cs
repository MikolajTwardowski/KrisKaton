using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    PlayerControls controls;
    [SerializeField] Transform head;
    public bool isGravityActive = true;
    public float gravityForce = 9.81f;
    Vector2 lookInput, moveInput;
    public float moveSpeed = 1f;
    public float sensitivity = 0.1f;
    float xRotation = 0f;


    private void Awake() {
        controls = new PlayerControls();

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        MovePlayer();
        ApplyGravity();
    }

    private void RotatePlayer()
    {
        // Pobierz wartości ruchu myszy
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        // Obrót wokół osi X (góra-dół)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        head.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Obrót wokół osi Y (lewo-prawo)
        transform.Rotate(Vector3.up * mouseX);
    }

    private void MovePlayer()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;
        move.Normalize();
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if(isGravityActive)
            characterController.Move(Vector3.down * gravityForce * Time.deltaTime);
    }

    private void OnEnable() 
    {
        controls.Enable();
    }

    private void OnDisable() 
    {
        controls.Disable();
    }
}
