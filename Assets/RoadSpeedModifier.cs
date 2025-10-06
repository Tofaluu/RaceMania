using UnityEngine;

public class RoadSpeedModifier : MonoBehaviour
{
    [HideInInspector] public float speedMultiplier = 1f;

    [SerializeField] private float roadMultiplier = 1.5f;
    private int roadContactCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Road"))
        {
            roadContactCount++;
            speedMultiplier = roadMultiplier;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Road"))
        {
            roadContactCount = Mathf.Max(0, roadContactCount - 1);
            if (roadContactCount == 0)
            {
                speedMultiplier = 1f;
            }
        }
    }
}