using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Phím Điều Khiển (Phải giống HitZone)")]
    public KeyCode groundKey = KeyCode.F; // Phím chém dưới đất
    public KeyCode airKey = KeyCode.J;    // Phím chém trên không

    [Header("Vị trí 2 làn (Lấy toạ độ Y)")]
    public Transform groundLane; // Kéo object Vạch Đất vào đây
    public Transform airLane;    // Kéo object Vạch Không vào đây

    [Header("Tốc độ bay (Càng lớn càng nhanh)")]
    public float moveSpeed = 25f; 

    private Vector3 targetPos;

    void Start()
    {
        if (groundLane != null) 
        {
            // Bắt đầu game, nhân vật mặc định đứng ở mặt đất
            targetPos = new Vector3(transform.position.x, groundLane.position.y, transform.position.z);
        }
    }

    void Update()
    {
        if (groundLane == null || airLane == null) return;

        // Bấm phím đánh trên -> Gán mục tiêu bay lên làn Air
        if (Input.GetKeyDown(airKey))
        {
            targetPos = new Vector3(transform.position.x, airLane.position.y, transform.position.z);
        }
        // Bấm phím đánh dưới -> Gán mục tiêu bay xuống làn Ground
        else if (Input.GetKeyDown(groundKey))
        {
            targetPos = new Vector3(transform.position.x, groundLane.position.y, transform.position.z);
        }

        // Lệnh Lerp giúp nhân vật lướt đi cực kỳ mượt mà thay vì giật cục (Teleport)
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }
}