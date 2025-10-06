using UnityEngine;

public class FollowCarRotation : MonoBehaviour
{
    public Transform target; // The car
    public Vector3 offset = new Vector3(0, 0, -10f); // Keep camera behind
    
    void LateUpdate()
    {
        if (target != null)
        {
            // Match car position + offset
            transform.position = target.position + offset;

            // Match car rotation
            transform.rotation = target.rotation;
        }
    }
}
