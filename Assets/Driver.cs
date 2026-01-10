using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Driver : MonoBehaviour
{
    [Header("Driving")]
    [SerializeField] float steerSpeed = 200f;     // deg/s at full steer
    [SerializeField] float moveSpeed  = 30f;      // m/s at full throttle
    [SerializeField] float acceleration = 6f;     // how fast currentMove chases target

    [Header("Reverse")]
    [SerializeField] float reverseFactor = 0.5f;  // your -0.5f from original

    Rigidbody2D rb;
    RoadSpeedModifier speedMod;

    float currentMove;   // -1..1 (reverse..forward)
    float targetMove;    // desired throttle
    float steer;         // -1..1

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speedMod = GetComponent<RoadSpeedModifier>();
    }

    void Update()
    {
        // --- Read input in Update (per-frame) ---
        targetMove = 0f;
        if (Keyboard.current.wKey.isPressed) targetMove = 1f;
        else if (Keyboard.current.sKey.isPressed) targetMove = -reverseFactor;

        steer = 0f;
        // Only allow steering when moving a bit
        if (Mathf.Abs(currentMove) > 0.01f)
        {
            if (Keyboard.current.aKey.isPressed) steer =  1f;
            else if (Keyboard.current.dKey.isPressed) steer = -1f;
        }
    }

    void FixedUpdate()
    {
        // --- Smooth throttle ---
        currentMove = Mathf.MoveTowards(currentMove, targetMove, acceleration * Time.fixedDeltaTime);

        float roadMultiplier = speedMod != null ? speedMod.speedMultiplier : 1f;

        // Forward direction is +Y in top-down car sprites
        Vector2 fwd = transform.up;

        // Desired velocity from throttle
        Vector2 desiredVel = fwd * (currentMove * moveSpeed * roadMultiplier);

        // Apply velocity directly (simple, responsive)
        rb.linearVelocity = desiredVel;

        // Steering scales with speed so you don’t spin when stopped
        float speed01 = Mathf.Clamp01(rb.linearVelocity.magnitude / (moveSpeed * Mathf.Max(0.01f, roadMultiplier)));
        float steerThisFrame = steer * steerSpeed * speed01 * Time.fixedDeltaTime;

        // Rotate around Z (positive steer = left turn when facing +Y)
        rb.MoveRotation(rb.rotation + steerThisFrame);
    }
}