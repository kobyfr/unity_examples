// 12/30/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;
// Logging handled by RuntimeLogger


public class PlayerShipController : MonoBehaviour
{
    public float rotation_force = 100f;
    public float forward_movement_force_magnitude = 10f;
    public float side_movement_force_magnitude = 10f;
    public float up_down_movement_force = 10f;
    public InputActionAsset actions;

    private Vector2 movement_input;
    private float movement_up_down_input;

    private Vector2 collision_force = Vector2.zero;

    private Rigidbody rb;
    private Vector2 look = Vector2.zero;
    private readonly float min_look_delta = 0.000001f;

    private InputAction moveAction;
    private InputAction moveUpDownAction;


    private void OnEnable()
    {
        moveAction.Enable();
        moveUpDownAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        moveUpDownAction.Disable();
    }

    private void Awake()
    {
        moveAction = actions.FindAction("player_ship/Move");
        moveUpDownAction = actions.FindAction("player_ship/MoveUpDown");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerShipController requires a Rigidbody component.");
        }
        // Note controller start using Unity's Debug logging
        Debug.LogFormat("PlayerShipController Start: time:{0}", Time.time);

        // Log input devices and watch for device changes
        try
        {
            InputSystem.onDeviceChange += OnDeviceChange;
            Debug.LogFormat("Current devices: {0}", string.Join(", ", InputSystem.devices));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to subscribe to InputSystem.onDeviceChange: " + e.Message);
        }
    }

    internal void handle_terrain_collision(Collider other)
    {
        Vector3 dir = rb.linearVelocity.normalized;
        if (dir.sqrMagnitude < 0.001f)
            return;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, 5f))
        {
            Vector3 normal = hit.normal;
            collision_force = new Vector2(normal.x, normal.z);
        }
        // TODO: do something with collision_force
    }
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        Debug.LogFormat("Device change: {0} {1}", device, change);
    }

    private void OnMove(InputValue value)
    {
        // Work is done on Update()
    }

    public void OnLook(InputValue value)
    {
        // Mouse movement (pointer delta) from the new Input System
        Vector2 pointer_delta = value.Get<Vector2>();

        // map vertical mouse movement to pitch (invert so moving mouse up looks up)
        float pitch = -pointer_delta.y;
        float yaw = pointer_delta.x;
        Vector2 temp_look = new Vector2(pitch, yaw).normalized;
        if ( temp_look.magnitude > min_look_delta)
        {
            look = new Vector2(pitch, yaw).normalized;
        }
    }

    void Update()
    {
        movement_input = moveAction.ReadValue<Vector2>();
        movement_up_down_input = moveUpDownAction.ReadValue<float>();
        Debug.Log(movement_input);
        Debug.Log(movement_up_down_input);
    }

    private void FixedUpdate() 
    {
        if (rb == null)
            return;

        // Strafe the player_ship based on xy input
        // Strafe the player_ship based on z input
        Vector3 movement_force = new Vector3(movement_input.x       * side_movement_force_magnitude, 
                                             movement_up_down_input * up_down_movement_force, 
                                             movement_input.y       * forward_movement_force_magnitude);
        rb.AddRelativeForce(movement_force);

        // Apply rotation of the look vector, and zeroize the look input
        ApplyLook(look);
        look = Vector3.zero;
    }

    void ApplyLook(Vector2 look)
    {
        if (look.magnitude > min_look_delta)
        {
            Debug.LogFormat("look.magnitude > 0.000001f {0}", look);
            float force_delta = Time.deltaTime * rotation_force;
            Vector3 torque = new Vector3(look.x * force_delta, look.y * force_delta, 0);
            rb.AddRelativeTorque(torque, ForceMode.Acceleration);
        }
        else
        {
            // Debug.Log("look vector too small");
        }
    }
}
