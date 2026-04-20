using UnityEngine;
using UnityEngine.InputSystem;

public class TruckController : MonoBehaviour
{

    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float maxWheelAngle = 30f;
    [SerializeField] float enginePower = 1000f;
    [SerializeField] float brakeForce = 1500f;
    [SerializeField] float stopSpeedThreshold = 0.5f; // m/s considered "stopped"
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

    const float movement_epsilon = 0.00001f;

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
        if (movement_input == null) return;

        ApplySteering();

        ApplyMotorAndBreaks();
    }

    private void ApplySteering()
    {
        // -- Steering -------------------------------------------------
        if (left_wheel_collider != null && right_wheel_collider != null)
        {
            float steerDelta = movement_input.x * turnSpeed * 0.01f;

            // Apply steering input and clamp to max angle
            left_wheel_collider.steerAngle = Mathf.Clamp(left_wheel_collider.steerAngle + steerDelta, -maxWheelAngle, maxWheelAngle);
            right_wheel_collider.steerAngle = Mathf.Clamp(right_wheel_collider.steerAngle + steerDelta, -maxWheelAngle, maxWheelAngle);

            // If no steering input and the truck is moving, let wheels return slowly to center
            float forwardSpeed = rb != null ? Vector3.Dot(rb.linearVelocity, transform.forward) : 0f;
            if (Mathf.Abs(movement_input.x) <= movement_epsilon && Mathf.Abs(forwardSpeed) > stopSpeedThreshold)
            {
                left_wheel_collider.steerAngle = Mathf.MoveTowards(left_wheel_collider.steerAngle, 0f, turnSpeed * 0.02f);
                right_wheel_collider.steerAngle = Mathf.MoveTowards(right_wheel_collider.steerAngle, 0f, turnSpeed * 0.02f);
            }

            // Update visual steering wheel if assigned
            if (steeringWheel != null)
            {
                // Rotate steering wheel around its local Y to match steering angle (adjust axis as needed)
                steeringWheel.localRotation = Quaternion.Euler(0f, left_wheel_collider.steerAngle, 0f);
            }
        }
    }

    private void ApplyMotorAndBreaks()
    {
        // -- Throttle / Braking --------------------------------------
        if (left_wheel_collider == null || right_wheel_collider == null)
        {
            return;
        }

        if (Mathf.Abs(movement_input.y) > movement_epsilon)
        {
            float desired = movement_input.y; // + forward, - reverse
            float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

            bool movingForward = forwardSpeed > stopSpeedThreshold;
            bool movingBackward = forwardSpeed < -stopSpeedThreshold;

            // If desired direction is opposite current motion, apply brakes until nearly stopped
            if ((desired > 0f && movingBackward) || (desired < 0f && movingForward))
            {
                left_wheel_collider.motorTorque = 0f;
                right_wheel_collider.motorTorque = 0f;
                left_wheel_collider.brakeTorque = brakeForce;
                right_wheel_collider.brakeTorque = brakeForce;
            }
            else
            {
                // Release brakes and apply motor torque
                left_wheel_collider.brakeTorque = 0f;
                right_wheel_collider.brakeTorque = 0f;
                float motor = desired * enginePower;
                left_wheel_collider.motorTorque = motor;
                right_wheel_collider.motorTorque = motor;
            }
        }
        else
        {
            // No throttle input: coast (no motor), release brakes
            left_wheel_collider.motorTorque = 0f;
            right_wheel_collider.motorTorque = 0f;
            left_wheel_collider.brakeTorque = 0f;
            right_wheel_collider.brakeTorque = 0f;
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
