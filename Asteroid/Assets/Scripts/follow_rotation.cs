using UnityEngine;

public class follow_rotation : MonoBehaviour
{

    public Transform target; // The target object to follow

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           transform.rotation = target.rotation; // Set the rotation of this object to match the target's rotation    
    }
}
