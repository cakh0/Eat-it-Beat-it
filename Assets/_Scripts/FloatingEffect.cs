using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [Header("Cài đặt hiệu ứng trôi nổi")]
    public float floatSpeed = 5f;    // Tốc độ lắc lư (càng lớn lắc càng nhanh)
    public float floatAmplitude = 0.2f; // Biên độ lắc (độ cao thấp của nhịp trôi nổi)

    private Vector3 startPos;

    void Start()
    {
        // Lưu lại vị trí ban đầu của đối tượng khi xuất hiện
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Dùng hàm Mathf.Sin kết hợp với Time.time để tạo chuyển động lượn sóng mượt mà theo trục Y
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}