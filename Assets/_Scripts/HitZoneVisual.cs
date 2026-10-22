using UnityEngine;

public class HitZoneVisual : MonoBehaviour
{
    [Header("Cài đặt Xoay")]
    [Tooltip("Tốc độ xoay (để số âm sẽ xoay cùng chiều kim đồng hồ)")]
    public float rotationSpeed = -40f; 

    [Header("Cài đặt Nhịp đập (Scale)")]
    [Tooltip("Tốc độ phóng to thu nhỏ")]
    public float pulseSpeed = 4f; 
    [Tooltip("Kích thước nhỏ nhất")]
    public float minScale = 0.95f; 
    [Tooltip("Kích thước lớn nhất")]
    public float maxScale = 1.15f; 

    private Vector3 originalScale;

    void Start()
    {
        // Lưu lại kích thước ban đầu của hình ảnh để làm chuẩn
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 1. Xử lý hiệu ứng xoay liên tục
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // 2. Xử lý hiệu ứng phóng to thu nhỏ nhịp nhàng bằng sóng Sin
        // Hàm Sin(Time.time) tạo ra giá trị lượn sóng mượt mà
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; 
        
        // Tính toán độ lớn hiện tại
        float currentScale = Mathf.Lerp(minScale, maxScale, pulse);
        
        // Áp dụng vào vật thể
        transform.localScale = originalScale * currentScale;
    }
}