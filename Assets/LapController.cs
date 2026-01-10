using UnityEngine;
using UnityEngine.UI; // Include this if you want to update a UI Text element

public class LapController : MonoBehaviour
{
    // --- Public Race Settings ---
    public int totalLaps = 3;
    
    // --- UI Display (Optional) ---
    // Drag your "Lap 1 / 3" Text element here
    public Text lapDisplay; 

    // --- Private Lap State ---
    private int lapsCompleted = 0;
    private int nextCheckpointID = 0;
    private int totalCheckpointsInRace;

    void Start()
    {
        // Find out how many checkpoints are in the scene
        totalCheckpointsInRace = FindObjectsOfType<Checkpoint>().Length;

        // --- !! NEW SAFETY CHECK !! ---
        // This stops the script from crashing if you forgot to set up checkpoints.
        if (totalCheckpointsInRace == 0)
        {
            Debug.LogError("No checkpoints found! Did you add the 'Checkpoint.cs' script to your checkpoint objects?");
            // Disable this script to prevent errors
            this.enabled = false; 
            return;
        }
        // --- End of safety check ---

        Debug.Log($"Found {totalCheckpointsInRace} checkpoints in the scene.");
        
        // Start the race on lap 1
        lapsCompleted = 0;
        nextCheckpointID = 0; // The car starts by looking for checkpoint 0
        UpdateLapUI();
    }

    // This is the core logic. It's called by Unity when
    // this GameObject (the car) enters a trigger.
    
    // --- UPDATED FOR 2D ---
    // Renamed from OnTriggerEnter to OnTriggerEnter2D
    // Changed parameter from (Collider other) to (Collider2D other)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Try to get a Checkpoint component from the object we hit
        if (other.TryGetComponent<Checkpoint>(out Checkpoint cp))
        {
            // --- CHECKPOINT LOGIC ---

            // Is this the checkpoint we were looking for?
            if (cp.checkpointID == nextCheckpointID)
            {
                Debug.Log($"Hit correct checkpoint: {cp.checkpointID}");

                // --- LAP COUNTING LOGIC ---
                // If this was the Start/Finish line (ID 0)
                if (cp.checkpointID == 0)
                {
                    // This logic is simplified to handle a single checkpoint (where totalCheckpointsInRace is 1)
                    // or a full track.
                    
                    // If we're on lap 0, this is the start.
                    if (lapsCompleted == 0)
                    {
                        lapsCompleted = 1; 
                        Debug.Log("Race Started! On Lap 1.");
                    }
                    // If we are on any lap *other* than 0, it means we just completed a lap.
                    else if (lapsCompleted > 0)
                    {
                        lapsCompleted++;
                        Debug.Log($"Lap {lapsCompleted} Completed!");

                        // --- RACE FINISH LOGIC ---
                        if (lapsCompleted > totalLaps) // Use > not >= to show "3/3" and then "Finished!"
                        {
                            EndRace();
                        }
                    }
                }

                // Update the next checkpoint we're looking for.
                nextCheckpointID = (nextCheckpointID + 1) % totalCheckpointsInRace;

                UpdateLapUI();
            }
            else
            {
                // Hit the wrong checkpoint.
                Debug.Log($"Hit WRONG checkpoint. Hit {cp.checkpointID}, but was looking for {nextCheckpointID}.");
            }
        }
    }

    void UpdateLapUI()
    {
        if (lapDisplay != null)
        {
            if (lapsCompleted > totalLaps)
            {
                GameManager.Instance.PlayerFinished(gameObject.name); 
            
                // Disable this script instance to stop the car from triggering more laps
                this.enabled = false;
            }
            else
            {
                int lapToShow = lapsCompleted;
                
                // This logic makes the UI display "1/3" at the start, not "0/3"
                if (lapsCompleted == 0) 
                    lapToShow = 1;

                lapDisplay.text = $"LAP: {lapToShow} / {totalLaps}";
            }
        }
    }

    void EndRace()
    {
        Debug.Log("--- RACE FINISHED ---");
        // UpdateLapUI() will now handle showing the "FINISHED!" text
    }
}