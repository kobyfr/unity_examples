using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerInput playerInput;
    public float speed = 3f;
    Vector2 input_vector = new Vector2();

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        InputSystem.actions.Disable();
        playerInput.currentActionMap?.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(input_vector.x, 0, input_vector.y);
        transform.position += movement * speed * Time.deltaTime;
    }

    void OnMove(InputValue value)
    {
        input_vector = value.Get<Vector2>();
    }
}
