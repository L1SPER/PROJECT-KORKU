using UnityEngine;

public class TrainScene : MonoBehaviour
{
    [SerializeField] private StartButton startButton;
    [SerializeField] private TrainMovement trainMovement;
    [SerializeField] private float waitTime;

    void Start()
    {
        trainMovement.SetCurrentSpeed(0);
        StartCoroutine(startButton.SetIsPressedByTime(waitTime));
    }
}
