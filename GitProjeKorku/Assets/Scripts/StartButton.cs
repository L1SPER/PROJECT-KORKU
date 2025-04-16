using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class StartButton : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isPressed;
    [SerializeField] private float waitTime;
    public bool IsStarted()
    {
        return isPressed;
    }
    public void SetStarted(bool _isStarted)
    {
        isPressed = _isStarted;
    }
    public void InteractWithoutPressingButton()
    {

    }

    public void InteractWithPressingButton()
    {
        if (isPressed == false && SceneLoader.Instance.GetSceneName() == "Train")
        {
            SceneLoader.Instance.LoadScene("DungeonGeneration");
        }
        else if (isPressed == false && SceneLoader.Instance.GetSceneName() == "DungeonGeneration")
        {
            SceneLoader.Instance.LoadScene("Train");
        }
    }
    public IEnumerator SetIsPressedByTime(float time)
    {
        if (isPressed == true)
            yield break;
        isPressed = true;
        yield return new WaitForSeconds(time);
        isPressed = false;
    }
}
