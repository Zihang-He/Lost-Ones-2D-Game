using UnityEngine;
using UnityEngine.Events;

public class ButtonHandler : MonoBehaviour
{
    // Event to broadcast button clicks
    public UnityEvent<string> OnButtonClicked;

    private int currentSceneIndex = 0;

    void Start()
    {
        // Initialize the UnityEvent if not set in the Inspector
        if (OnButtonClicked == null)
        {
            OnButtonClicked = new UnityEvent<string>();
        }
    }

    // Called when a button is pressed
    public void OnButtonPressed(string buttonName)
    {
        // Record the button press in the GameState
        GameState.Instance.RecordButtonPress(currentSceneIndex, buttonName);

        // Broadcast the button click event
        OnButtonClicked?.Invoke(buttonName);
        Debug.Log($"Button {buttonName} clicked, invoked event.");
    }
}