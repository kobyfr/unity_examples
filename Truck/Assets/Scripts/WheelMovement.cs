using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class WheelMovement : MonoBehaviour
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

        // Align the visual wheel's rotation with the wheel collider pose (world) while preserving
        // any steering/yaw applied to the wheel GameObject (the parent).
        Quaternion worldRotation = q * mesh_base_rotation;

        // If the visual mesh is parented to this wheel transform, convert the desired world
        // rotation into the mesh's local rotation so the parent's rotation is preserved.
        if (mesh != null && mesh.parent == transform)
        {
            mesh.localRotation = Quaternion.Inverse(transform.rotation) * worldRotation;
        }
        else if (mesh != null)
        {
            // If the mesh is not parented, just set world rotation directly.
            mesh.rotation = worldRotation;
        }

        // Position the visual wheel at the correct height based on the wheel collider's position and the ground contact point
        mesh.position = pos;
    }
}
