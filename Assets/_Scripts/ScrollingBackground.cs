using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 2f; // Tốc độ cuộn (nên chậm hơn tốc độ quái)
    public float endX = -20f;      // Điểm mà ảnh sẽ biến mất khỏi màn hình
    public float startX = 20f;     // Điểm ảnh sẽ xuất hiện lại (nối đuôi ảnh kia)

    void Update()
    {
        // Di chuyển ảnh sang trái
        transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0f, 0f);

        // Nếu ảnh đã chạy quá giới hạn bên trái, dịch chuyển nó về bên phải
        if (transform.position.x <= endX)
        {
            Vector3 newPos = transform.position;
            newPos.x = startX;
            transform.position = newPos;
        }
    }
}