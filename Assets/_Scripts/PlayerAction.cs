using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Cài đặt Phím (Phải giống trong HitZone)")]
    public KeyCode airKey; 
    public KeyCode groundKey; 

    [Header("Vị trí lao tới (Kéo thả HitZone vào đây)")]
    public Transform airHitZone;
    public Transform groundHitZone;

    [Header("Cài đặt Tốc độ")]
    public float dashSpeed = 25f; // Tốc độ lao lên (số càng to càng nhanh)
    public float returnSpeed = 15f; // Tốc độ lùi về

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    void Start()
    {
        // Lưu lại vị trí đứng ban đầu của nhân vật làm gốc
        originalPosition = transform.position;
        targetPosition = originalPosition;
    }

    void Update()
    {
        // Khi bấm phím, đặt mục tiêu là vị trí của HitZone tương ứng
        if (Input.GetKeyDown(airKey))
        {
            targetPosition = airHitZone.position;
        }
        else if (Input.GetKeyDown(groundKey))
        {
            targetPosition = groundHitZone.position;
        }

        // Khi nhả phím, kiểm tra xem có đang giữ phím nào khác không
        if (Input.GetKeyUp(airKey) || Input.GetKeyUp(groundKey))
        {
            // Nếu đã nhả hết cả 2 phím, đặt mục tiêu quay về chỗ cũ
            if (!Input.GetKey(airKey) && !Input.GetKey(groundKey))
            {
                targetPosition = originalPosition;
            }
        }

        // Thực hiện di chuyển nhân vật mượt mà bằng Lerp
        float currentSpeed = (targetPosition == originalPosition) ? returnSpeed : dashSpeed;
        transform.position = Vector3.Lerp(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }
}