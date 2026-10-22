using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Tốc độ di chuyển của kẻ địch
    public float moveSpeed = 10f;

    void Update()
    {
        // Di chuyển sang trái đều đặn mỗi frame
        transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
    }
}