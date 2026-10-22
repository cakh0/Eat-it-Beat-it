using UnityEngine;
using TMPro;
using System.Collections;

public class GameFlowManager : MonoBehaviour
{
    // Tạo Singleton để gọi từ mọi nơi
    public static GameFlowManager instance;

    [Header("Chế độ Tự Động")]
    public bool isAutoPlay = false; // Bật/tắt Auto Play tại đây

    [Header("UI Đếm Ngược")]
    public TextMeshProUGUI countdownText;

    [Header("UI Kết Thúc")]
    public GameObject endPopup;
    public TextMeshProUGUI finalScoreText;

    [Header("Thành phần Game")]
    public AudioSource musicSource; 
    public MonoBehaviour spawnerScript;

    private bool hasGameStarted = false;

    void Awake()
    {
        // Gán instance khi game vừa chạy
        if (instance == null) instance = this;
    }

    void Start()
    {
        if (spawnerScript != null) spawnerScript.enabled = false;
        if (endPopup != null) endPopup.SetActive(false);
        
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);

        if (musicSource != null) musicSource.Play();
        if (spawnerScript != null) spawnerScript.enabled = true;
        
        hasGameStarted = true;
    }

    void Update()
    {
        if (hasGameStarted && musicSource != null)
        {
            if (!musicSource.isPlaying)
            {
                ShowEndPopup();
            }
        }
    }

    void ShowEndPopup()
    {
        hasGameStarted = false; 
        if (spawnerScript != null) spawnerScript.enabled = false;
        
        if (endPopup != null) 
        {
            endPopup.SetActive(true);
            if (ScoreManager.instance != null && finalScoreText != null)
            {
                finalScoreText.text = "TRACK CLEAR!\n\n" + ScoreManager.instance.scoreText.text;
            }
        }
    }
}