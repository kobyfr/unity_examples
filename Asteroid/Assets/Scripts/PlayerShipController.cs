// 12/30/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShipController : MonoBehaviour
{
    public float move_speed = 10f;
    public float rotation_speed = 100f;

    private Vector2 movement_input;
    private float rotation_input;

    private void OnMove(InputValue value)
    {
        // Get movement input from the new Input System
        movement_input = value.Get<Vector2>();
    }

    private void OnRotate(InputValue value)
    {
        // Get rotation input from the new Input System
        rotation_input = value.Get<float>();
    }

    private void Update()
    {
        // Strafe the player_ship based on xy input
        Vector3 movement = new Vector3(movement_input.x, 0, movement_input.y);
        transform.Translate(move_speed * Time.deltaTime * movement , Space.Self);

        // Rotate the player_ship based on rotation input
        transform.Rotate(0, rotation_speed * Time.deltaTime * rotation_input, 0);
    }
}
