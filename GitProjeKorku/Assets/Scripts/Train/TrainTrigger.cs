using UnityEngine;

public class TrainTrigger : MonoBehaviour
{
    private TrainMovement trainMovement;
    private void Start()
    {
        trainMovement= GetComponentInParent<TrainMovement>();
        if (trainMovement == null)
        {
            Debug.LogError("TrainMovement component not found in parent object.");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                characterMovement.SetIsInsideOfTrain(true);
                characterMovement.SetTrainMovement(trainMovement);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                characterMovement.SetIsInsideOfTrain(true);
                characterMovement.SetTrainMovement(trainMovement);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterMovement characterMovement = other.GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                characterMovement.SetIsInsideOfTrain(false);
                characterMovement.SetTrainMovement(null);
            }
        }
    }
}
