// 12/30/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShipController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector2 movementInput;

    private void OnMove(InputValue value)
    {
        // Get movement input from the new Input System
        movementInput = value.Get<Vector2>();
    }

    private void Update()
    {
        // Move the player_ship based on input
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        transform.Translate(moveSpeed * Time.deltaTime * movement , Space.World);
    }
}
