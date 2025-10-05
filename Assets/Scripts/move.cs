using UnityEngine;
using System;

public class MoveObject : MonoBehaviour
{
    [Header("移动参数")]
    public float speed = 5f;                   // 移动速度
    public Vector3 moveDirection = Vector3.right; // 移动方向
    public float moveDuration = 3f;            // 移动持续时间（秒）

    private float moveTimer = 0f;
    private bool hasFinished = false;

    // ✅ 新增事件，完成移动时抛出
    public event Action OnMoveFinished;

    void Update()
    {
        if (moveTimer < moveDuration)
        {
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
            moveTimer += Time.deltaTime;
        }
        else if (!hasFinished)
        {
            hasFinished = true;
            Debug.Log($"{name} 移动完成，抛出事件！");
            OnMoveFinished?.Invoke(); // 抛出事件
        }
    }

}

