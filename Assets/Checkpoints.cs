using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // --- This is the core variable ---
    // Manually set this in the Inspector for each checkpoint (0, 1, 2...)
    public int checkpointID;

    // This is a helpful editor-only function.
    // It draws a wireframe in your Scene view to visualize the trigger.
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Green, semi-transparent
        Gizmos.matrix = transform.localToWorldMatrix;
        
        // --- UPDATED FOR 2D ---
        // Assumes you are using a BoxCollider2D as the trigger
        if (TryGetComponent<BoxCollider2D>(out var boxCollider2D))
        {
            // We still use DrawWireCube, as Gizmos are 3D,
            // but we get the offset and size from the 2D collider.
            Gizmos.DrawWireCube(boxCollider2D.offset, boxCollider2D.size);
        }
        else
        {
            // Fallback if not a box collider
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 5);
        }
    }
}