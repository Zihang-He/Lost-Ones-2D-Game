using UnityEngine;

public class CameraTeleport : MonoBehaviour
{
    // Positions for teleportation
    public Vector3[] scenePositions;
    private int currentSceneIndex = 0;

    // Camera zoom settings
    public float zoomedInSize = 5f; // Zoomed-in orthographic size
    public float normalSize = 10f; // Normal orthographic size
    public float zoomSpeed = 2f; // Speed of zooming in/out

    private Camera mainCamera;
    private bool isZooming = false;
    private float targetSize;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        targetSize = normalSize; // Start with normal size
    }

    void Update()
    {
        // Handle zooming
        if (isZooming)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
            if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < 0.01f)
            {
                mainCamera.orthographicSize = targetSize;
                isZooming = false;
            }
        }

        // Trigger teleportation and zooming with space key (for testing)
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     StartSceneTransition();
        // }
    }

    // Start the scene transition
    public void StartSceneTransition()
    {
        StartCoroutine(SceneTransition());
    }
    public void OnButtonPressed(string buttonName)
    {
        // Record the button press in the GameState
        GameState.Instance.RecordButtonPress(currentSceneIndex, buttonName);

        // Start the scene transition
        // StartSceneTransition();
        StartCoroutine(SceneTransition());
    }

    // Coroutine to handle zooming and teleportation
    private System.Collections.IEnumerator SceneTransition()
    {
        // Step 1: Zoom in
        targetSize = zoomedInSize;
        isZooming = true;
        yield return new WaitUntil(() => !isZooming);

        // Step 2: Teleport to the next scene
        TeleportToNextScene();

        // Step 3: Zoom out
        targetSize = normalSize;
        isZooming = true;
        yield return new WaitUntil(() => !isZooming);
    }

    // Teleport to the next scene
    private void TeleportToNextScene()
    {
        if (scenePositions.Length == 0) return;

        currentSceneIndex = (currentSceneIndex + 1) % scenePositions.Length;
        transform.position = scenePositions[currentSceneIndex];
    }
}