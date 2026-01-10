using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI[] countdownTexts;
    public Button playAgainButton; 

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
        
        // Hide play again button initially
        if (playAgainButton != null)
        {
            playAgainButton.gameObject.SetActive(false);
            playAgainButton.onClick.AddListener(RestartGame);
        }
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

        // Show controls for each driver
        if (countdownTexts.Length >= 1 && countdownTexts[0] != null)
            countdownTexts[0].text = "WASD to move";
        if (countdownTexts.Length >= 2 && countdownTexts[1] != null)
            countdownTexts[1].text = "Arrow keys to move";
        
        yield return new WaitForSeconds(3f); // Show instructions for 3 seconds

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
        
        // Show play again option after a short delay
        StartCoroutine(ShowPlayAgainOption());
    }
    
    IEnumerator ShowPlayAgainOption()
    {
        yield return new WaitForSeconds(3f); // Show win message for 3 seconds
        
        // Change text to show play again instruction
        foreach (TextMeshProUGUI text in countdownTexts)
        {
            if (text != null)
            {
                text.text = "Press SPACE to Play Again";
            }
        }
        
        // Show button if it exists
        if (playAgainButton != null)
        {
            playAgainButton.gameObject.SetActive(true);
        }
    }
    
    void Update()
    {
        // Allow restart with spacebar when race is finished
        if (raceFinished)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("Space pressed - restarting game");
                RestartGame();
            }
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("RestartGame called");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}