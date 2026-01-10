using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI[] countdownTexts; 

    [Header("Race Settings")]
    public float initialCountdownTime = 3f;
    public GameObject[] raceCars; 
    public float winDecelerationDrag = 2f; // NEW: High drag value for quick stop

    private bool raceFinished = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Assuming all cars (Player 1 and Player 2) are tagged "Player"
        raceCars = GameObject.FindGameObjectsWithTag("Player");
    }

    void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        SetCarControls(false); // Stop cars for countdown

        foreach (TextMeshProUGUI text in countdownTexts)
        {
            if (text != null)
            {
                text.gameObject.SetActive(true);
                text.text = ""; 
            }
        }

        float timer = initialCountdownTime;
        while (timer > 0)
        {
            string currentCount = Mathf.Ceil(timer).ToString();
            
            foreach (TextMeshProUGUI text in countdownTexts)
            {
                if (text != null)
                {
                    text.text = currentCount;
                }
            }
            yield return new WaitForSeconds(1f);
            timer--;
        }

        foreach (TextMeshProUGUI text in countdownTexts)
        {
            if (text != null)
            {
                text.text = "GO!";
            }
        }
        SetCarControls(true); // Enable car movement

        yield return new WaitForSeconds(0.5f);
        foreach (TextMeshProUGUI text in countdownTexts)
        {
            if (text != null)
            {
                text.gameObject.SetActive(false);
            }
        }
    }
    
    // --- SetCarControls METHOD (Unchanged from last version) ---
    void SetCarControls(bool enable)
    {
        foreach (GameObject car in raceCars)
        {
            // Disable/Enable Driver scripts
            MonoBehaviour driverScript = car.GetComponent<Driver>();
            if (driverScript != null) driverScript.enabled = enable;

            MonoBehaviour driver2Script = car.GetComponent<Driver2>(); 
            if (driver2Script != null) driver2Script.enabled = enable;

            // Stop the car's physics instantly when disabling controls (for countdown start)
            if (!enable)
            {
                Rigidbody2D rb = car.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }
            }
        }
    }

    // --- MODIFIED PlayerFinished METHOD ---
    public void PlayerFinished(string playerName)
    {
        if (raceFinished)
        {
            return; 
        }

        raceFinished = true;
        
        // Disable control scripts and apply high drag for deceleration
        foreach (GameObject car in raceCars)
        {
            // Disable LapController to stop lap counting/UI updates
            LapController lapController = car.GetComponent<LapController>();
            if (lapController != null)
            {
                lapController.enabled = false;
            }
            
            // Disable driver scripts so players can't input more speed
            MonoBehaviour driverScript = car.GetComponent<Driver>();
            if (driverScript != null) driverScript.enabled = false;

            MonoBehaviour driver2Script = car.GetComponent<Driver2>(); 
            if (driver2Script != null) driver2Script.enabled = false;

            // Apply high drag to the Rigidbody to cause deceleration
            Rigidbody2D rb = car.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearDamping = winDecelerationDrag;
                rb.angularDamping = winDecelerationDrag;
            }
        }

        string winMessage = playerName.ToUpper() + " WINS!";

        // Display the win message on both screens
        foreach (TextMeshProUGUI text in countdownTexts)
        {
            if (text != null)
            {
                text.gameObject.SetActive(true);
                text.text = winMessage; 
            }
        }
    }
}