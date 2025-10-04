using UnityEngine;

public class CameraTeleport : MonoBehaviour
{
    // 你想瞬移到的位置
    public Vector3 teleportPosition;

    void Update()
    {
        // 按下空格键触发闪现
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TeleportCamera(teleportPosition);
        }
    }

    // 闪现函数
    public void TeleportCamera(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
