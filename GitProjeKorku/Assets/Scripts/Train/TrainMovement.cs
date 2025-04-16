using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainMovement : MonoBehaviour
{
    [SerializeField] private float initialSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private float acceleration;
    private float currentSpeed;
    private bool isMoving;
    public bool isAccelerating;
    public bool isDecelerating;
    public bool isStopped;
    void Start()
    {
        currentSpeed = initialSpeed;
        isMoving = false;
    }

    void Update()
    {
        TrainMove();
    }
    private void TrainMove()
    {
        if(isDecelerating)
        {
            DecelarateTrain();
        }
        else if(isAccelerating)
        {
            AccelarateTrain();
        }
        else if (isStopped)
        {
            StopTrain();
        }
        transform.position += Vector3.right * currentSpeed * Time.deltaTime;

        if (currentSpeed > 0)
            isMoving = true;
        else
            isMoving = false;
    }
    public void DecelarateTrain()
    {
        if (currentSpeed > 0)
            currentSpeed += deceleration * Time.deltaTime;

        if (currentSpeed < 0)
            currentSpeed = 0;
    }
    public void StopTrain()
    {
        currentSpeed = 0;
    }
    public void AccelarateTrain()
    {
        if (currentSpeed < initialSpeed)
            currentSpeed += acceleration * Time.deltaTime;

        if (currentSpeed > initialSpeed)
            currentSpeed = initialSpeed;
    }
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    public void SetCurrentSpeed(float _currentSpeed)
    {
        currentSpeed -= _currentSpeed;
    }
    public bool IsMoving()
    {
        return isMoving;
    }
    public bool IsCurrentSpeedBiggerThanInitialSpeed()
    {
        if (currentSpeed >= initialSpeed)
            return true;
        else
            return false;
    }
}