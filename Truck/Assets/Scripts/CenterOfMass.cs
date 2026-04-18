using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    public Transform comAnchor; // Drag your 'CenterOfMass' empty here
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (comAnchor != null)
        {
            // Center of Mass must be in LOCAL space relative to the Rigidbody
            rb.centerOfMass = comAnchor.localPosition;
        }
    }

    // Optional: Visualizes the center of mass in the Editor
    void OnDrawGizmos()
    {
        if (comAnchor != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(comAnchor.position, 0.2f);
        }
    }
}
