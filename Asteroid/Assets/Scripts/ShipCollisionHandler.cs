// 1/1/2026 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;

public class ShipCollisionHandler : MonoBehaviour
{
    PlayerShipController player_ship_controller;

    private void Start()
    {
        // Get reference to the PlayerShipController component
        player_ship_controller = GetComponentInParent<PlayerShipController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with the terrain
        if (other.CompareTag("terrain"))
        {
            Debug.Log("trigger enter, with terrain!");
            // Handle collision logic here
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the terrain
        if (collision.collider.CompareTag("terrain"))
        {
            Debug.Log("Collision detected with terrain!");
            player_ship_controller.handle_terrain_collision(collision.collider);
        }

    }
}
