using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class wheel_movement : MonoBehaviour
{
    private WheelCollider wheelCollider;
    private Quaternion mesh_base_rotation;
    [SerializeField] Transform mesh;
    void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
        // Assuming the visual wheel is the first child
        Vector3 unused = new Vector3();
        mesh.GetLocalPositionAndRotation(out unused, out mesh_base_rotation);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        Quaternion q = new Quaternion();

        wheelCollider.GetWorldPose(out pos, out q);

        // Align the visual wheel's rotation with the ground normal (plus its initial rotations)
        mesh.rotation = q * mesh_base_rotation;

        // Position the visual wheel at the correct height based on the wheel collider's position and the ground contact point
        mesh.position = pos;
    }
}
