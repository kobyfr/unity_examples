using UnityEngine;

public class wheel_movement : MonoBehaviour
{
    private WheelCollider wheelCollider;
    private Transform mesh;
    private Quaternion mesh_base_rotation;

    void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
        // Assuming the visual wheel is the first child
        mesh = transform.GetChild(0);
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
    }
}
