using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed = 2f;  // 相机移动速度

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}