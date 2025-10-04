using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Singleton instance
    public static GameState Instance;

    // Dictionary to track button presses for each scene
    private Dictionary<int, List<string>> sceneButtonPresses = new Dictionary<int, List<string>>();

    void Awake()
    {
        // Ensure only one instance of GameState exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Record a button press for a specific scene
    public void RecordButtonPress(int sceneIndex, string buttonName)
    {
        if (!sceneButtonPresses.ContainsKey(sceneIndex))
        {
            sceneButtonPresses[sceneIndex] = new List<string>();
        }

        if (!sceneButtonPresses[sceneIndex].Contains(buttonName))
        {
            sceneButtonPresses[sceneIndex].Add(buttonName);
            Debug.Log($"Button '{buttonName}' pressed in Scene {sceneIndex}");
        }
    }

    // Get the list of buttons pressed for a specific scene
    public List<string> GetButtonsPressed(int sceneIndex)
    {
        if (sceneButtonPresses.ContainsKey(sceneIndex))
        {
            return sceneButtonPresses[sceneIndex];
        }
        return new List<string>();
    }
}