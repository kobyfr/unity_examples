using UnityEngine;
using UnityEngine.InputSystem;

public class TruckController : MonoBehaviour
{

    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float maxWheelAngle = 30f;
    [SerializeField] float enginePower;
    [SerializeField] InputActionAsset actions;
    [SerializeField] Transform steeringWheel;
    [SerializeField] Transform leftWheel;
    [SerializeField] Transform rightWheel;

    private Vector2 movement_input;
    private float wheel_initial_rotation;
    private WheelCollider left_wheel_collider;
    private WheelCollider right_wheel_collider;
    private InputAction moveAction;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = actions.FindAction("player/Move");
        if (leftWheel != null)
        {
            left_wheel_collider = leftWheel.GetComponent<WheelCollider>();
            if (left_wheel_collider == null)
                Debug.LogWarning($"TruckController: no WheelCollider found on leftWheel '{leftWheel.name}'");
        }
        else
        {
            Debug.LogError("TruckController: leftWheel Transform is not assigned.");
        }

        if (rightWheel != null)
        {
            right_wheel_collider = rightWheel.GetComponent<WheelCollider>();
            if (right_wheel_collider == null)
                Debug.LogWarning($"TruckController: no WheelCollider found on rightWheel '{rightWheel.name}'");
        }
    }

    private void Start()
    {
        wheel_initial_rotation = leftWheel.localEulerAngles.y;
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        movement_input = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (movement_input != null)
        {
            float current_angle = CurrentSteeringAngle();
            if ((movement_input.x > 0.00001 && current_angle < maxWheelAngle) ||
                (movement_input.x < -0.00001 && current_angle > -maxWheelAngle))
            {
                left_wheel_collider.steerAngle += movement_input.x * turnSpeed * 0.01F;
                right_wheel_collider.steerAngle += movement_input.x * turnSpeed * 0.01F;
            }
        }
    }

    public void Drop()
    {
        rb.useGravity = true;
    }
    private float CurrentSteeringAngle()
    {
        return left_wheel_collider.steerAngle;
    }
}
