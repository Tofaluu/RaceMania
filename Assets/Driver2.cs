using UnityEngine;
using UnityEngine.InputSystem;

public class Driver2 : MonoBehaviour
{
    [SerializeField] float steerSpeed = 200f;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float acceleration = 2f;

    float currentMove = 0f;

    // Update is called once per frame
    void Update()
    {
        float steer = 0;
        float targetMove = 0;
        if (Keyboard.current.upArrowKey.isPressed)
        {
            targetMove = 1;
        }
        else if (Keyboard.current.downArrowKey.isPressed)
        {
            targetMove = -0.5f;
        }

        // Accelerate/decelerate currentMove toward targetMove
        currentMove = Mathf.MoveTowards(currentMove, targetMove, acceleration * Time.deltaTime);

        // Only allow steering if the car is moving
        if (Mathf.Abs(currentMove) > 0.01f)
        {
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                steer = 1;
            }
            else if (Keyboard.current.rightArrowKey.isPressed)
            {
                steer = -1;
            }
        }

        float moveAmount = currentMove * moveSpeed * Time.deltaTime;
        float steerAmount = steer * steerSpeed * Time.deltaTime;

        transform.Rotate(0, 0, steerAmount); 
        transform.Translate(0, moveAmount, 0);
    }
}