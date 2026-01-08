// 12/30/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


public class PlayerShipController : MonoBehaviour
{
    public float rotation_force = 100f;
    public float forward_movement_force_magnitude = 10f;
    public float side_movement_force_magnitude = 10f;

    private Vector2 movement_input;
    private float rotation_input;

    private Vector2 collision_force = Vector2.zero;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerShipController requires a Rigidbody component.");
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
    }

    private void OnMove(InputValue value)
    {
        // Get movement input from the new Input System
        movement_input = value.Get<Vector2>();

        // Rotate movement direction to be relative to the ship's direction
        // Vector3 localInput = new Vector3(movement_input.x, 0, movement_input.y);
        // Vector3 worldInput = transform.TransformDirection(localInput);
        // movement_input = new Vector2(worldInput.x, worldInput.z);
    }

    private void OnRotate(InputValue value)
    {
        // Get rotation input from the new Input System
        rotation_input = value.Get<float>();
    }

    private void FixedUpdate() 
    {
        if (rb == null)
            return;

        // Strafe the player_ship based on xy input, or according to the collision force if there was a recent collision
        Vector3 movement_force = new Vector3(movement_input.x * side_movement_force_magnitude, 0, movement_input.y * forward_movement_force_magnitude);
        rb.AddRelativeForce(movement_force);

        if (rotation_input != 0)
        {
            // Rotate the player_ship based on rotation input
            Vector3 torque = new Vector3(0, rotation_input * rotation_force, 0);
            rb.AddRelativeTorque(torque);
        }
    }
}
