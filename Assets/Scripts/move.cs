using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("移动参数")]
    public float speed = 5f; // 移动速度
    public Vector3 moveDirection = Vector3.right; // 移动方向

    [Header("计时控制")]
    public float moveDuration = 3f; // 移动持续时间（秒）
    private float moveTimer = 0f;   // 计时器

    void Update()
    {
        // 如果还没到达移动时间
        if (moveTimer < moveDuration)
        {
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
            moveTimer += Time.deltaTime;
        }
    }
}
