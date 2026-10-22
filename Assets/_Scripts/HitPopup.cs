using UnityEngine;
using TMPro;

public class HitPopup : MonoBehaviour
{
    public float moveSpeed = 3f; // Tốc độ bay lên
    public float lifetime = 0.5f; // Thời gian hiển thị
    
    private TextMeshPro textMesh;
    private Color startColor;
    private float timer = 0f;

    public void Setup(string text, Color color)
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = text;
        textMesh.color = color;
        startColor = color;
        
        // Tự động xóa sau khi hiển thị xong
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Hiệu ứng bay lên
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        
        // Hiệu ứng mờ dần
        timer += Time.deltaTime;
        if (textMesh != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / lifetime);
            textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        }
    }
}