using Unity.VisualScripting;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("spaceship"))
        {
            // TODO : fix code to find callback, even if "other" is the parent object directly containing the script.
            other.GetComponentInParent<PlayerShipController>()?.OnLandingPadEntered(this);
        }
        else
        {
            LogToFile.Log("Collider with tag " + other.tag);
        }
    }
}
