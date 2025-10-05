using UnityEngine;
using System;

public class MoveObject : MonoBehaviour
{
    [Header("移动参数")]
    public float speed = 5f;                        // 移动速度
    public Vector3 moveDirection = Vector3.right;   // 移动方向
    public float moveDuration = 3f;                 // 移动持续时间（秒）
    public float startDelay = 0f;                   // ⏳ 开始移动前的等待时间（秒）

    private float moveTimer = 0f;
    private float delayTimer = 0f;
    private bool isMoving = false;
    private bool hasFinished = false;

    // ✅ 移动完成事件
    public event Action OnMoveFinished;

    void Update()
    {
        // 如果还在等待开始移动
        if (!isMoving)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= startDelay)
            {
                isMoving = true;
                Debug.Log($"{name} 等待 {startDelay} 秒后开始移动！");
            }
            return;
        }

        // 开始移动逻辑
        if (moveTimer < moveDuration)
        {
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
            moveTimer += Time.deltaTime;
        }
        else if (!hasFinished)
        {
            hasFinished = true;
            Debug.Log($"{name} 移动完成，抛出事件！");
            OnMoveFinished?.Invoke();
        }
    }
}
