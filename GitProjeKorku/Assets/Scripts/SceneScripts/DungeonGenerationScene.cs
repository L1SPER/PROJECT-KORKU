using System.Collections;
using System.Text;
using UnityEngine;

public class DungeonGenerationScene : MonoBehaviour
{
    [SerializeField] private StartButton startButton;
    [SerializeField] private TrainMovement trainMovement;
    [SerializeField] private float startButtonWaitTime;

    [SerializeField] private float decelerationTime;
    [SerializeField] private float stopTime;
    [SerializeField] private float accelerationTime;
    void Start()
    {
        StartCoroutine(SceneStart());
            // 5 sn sonra traine gec
            //SceneManager.Instance.LoadScene(trainSceneName);
        StartCoroutine(startButton.SetIsPressedByTime(startButtonWaitTime));
    }
    IEnumerator SceneStart()
    {
        trainMovement.isDecelerating=true;
        yield return new WaitForSeconds(decelerationTime);
        trainMovement.isDecelerating=false;
        trainMovement.isStopped=true;
        yield return new WaitForSeconds(stopTime);
        trainMovement.isStopped=false;
        trainMovement.isAccelerating=true;
        yield return new WaitForSeconds(accelerationTime); 
        trainMovement.isAccelerating=false;

        SceneLoader.Instance.LoadScene("Train");
    }
}
