using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;    
    public TextMeshProUGUI comboText;    
    
    [Header("Prefabs")]
    public GameObject hitPopupPrefab; // Kéo thả HitPopupPrefab vào đây

    private int score = 0;
    private int combo = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    // Yêu cầu truyền thêm Vector3 hitPosition (Vị trí sinh chữ)
    public void AddHit(string judgment, Vector3 hitPosition)
    {
        combo++;
        string popText = "";
        Color popColor = Color.white;

        if (judgment == "Perfect")
        {
            score += 300;
            popText = "PERFECT";
            popColor = Color.yellow;
        }
        else if (judgment == "Great")
        {
            score += 100;
            popText = "GREAT";
            popColor = new Color(0.2f, 0.8f, 1f); 
        }

        UpdateUI();
        SpawnPopup(popText, popColor, hitPosition);

        if (comboText != null)
        {
            StopCoroutine("PunchScale");
            StartCoroutine(PunchScale(comboText.transform));
        }
    }

    public void MissHit(Vector3 missPosition)
    {
        combo = 0;
        UpdateUI();
        SpawnPopup("MISS", Color.gray, missPosition);
    }

    // Hàm sinh chữ
    void SpawnPopup(string text, Color color, Vector3 pos)
    {
        if (hitPopupPrefab != null)
        {
            GameObject popup = Instantiate(hitPopupPrefab, pos, Quaternion.identity);
            HitPopup script = popup.GetComponent<HitPopup>();
            if (script != null) script.Setup(text, color);
        }
    }

    void UpdateUI()
    {
        if (scoreText != null) 
        {
            scoreText.text = "SCORE: " + score.ToString("D6"); 
        }

        if (comboText != null)
        {
            if (combo > 0)
            {
                comboText.text = combo.ToString() + "\nHITS"; 
                comboText.gameObject.SetActive(true);
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator PunchScale(Transform target)
    {
        Vector3 originalScale = Vector3.one;
        Vector3 punchScale = Vector3.one * 1.4f; 
        float duration = 0.08f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale, punchScale, elapsed / duration);
            yield return null;
        }
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.Lerp(punchScale, originalScale, elapsed / duration);
            yield return null;
        }
        target.localScale = originalScale;
    }
}