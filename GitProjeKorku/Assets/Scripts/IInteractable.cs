using UnityEngine;

public interface IInteractable 
{
    void InteractWithoutPressingButton();
    void InteractWithPressingButton();
    void InteractWithoutPressingButton(bool _isInteracting);
}
