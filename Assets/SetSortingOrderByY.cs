using UnityEngine;

public class SetSortingOrderByY : MonoBehaviour
{
    // A multiplier is used because the Order in Layer must be an integer.
    // Use a NEGATIVE multiplier to make:
    // HIGHER Y (further up) -> LOWER Order in Layer (drawn behind)
    // LOWER Y (further down) -> HIGHER Order in Layer (drawn in front)
    
    // Adjust this value to ensure objects close in Y have distinct sorting orders.
    public float sortingMultiplier = -1f; 
    
    private Renderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        if (myRenderer == null)
        {
            Debug.LogError("Renderer component not found. Cannot set Order in Layer.");
            enabled = false; // Stop the script if no renderer exists
        }
    }

    void Update()
    {
        // Get the Y position
        float yPosition = transform.position.y;

        // Calculate the new Order in Layer value. 
        // Multiplying by a NEGATIVE number creates the desired inverse depth effect.
        int newOrder = (int)(yPosition * sortingMultiplier);

        // Apply the new order to the renderer
        if (myRenderer != null)
        {
            myRenderer.sortingOrder = newOrder;
        }
    }
}
