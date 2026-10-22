using UnityEngine;
using TMPro;
using System.Collections;

public class GameFlowManager : MonoBehaviour
{
    [Header("UI Đếm Ngược")]
    public TextMeshProUGUI countdownText;

    [Header("UI Kết Thúc")]
    public GameObject endPopup; // Kéo Panel EndPopup vào đây
    public TextMeshProUGUI finalScoreText; // Kéo FinalScoreText vào đây

    [Header("Thành phần Game")]
    public AudioSource musicSource; 
    public MonoBehaviour spawnerScript; // Kéo script Spawner vào đây

    private bool hasGameStarted = false;

    void Start()
    {
        // 1. Tắt Spawner để quái không sinh ra khi đang đếm ngược
        if (spawnerScript != null) spawnerScript.enabled = false;
        
        // 2. Chắc chắn Popup kết thúc đang ẩn
        if (endPopup != null) endPopup.SetActive(false);
        
        // 3. Bắt đầu đếm ngược
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f); // Đợi 1 giây
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f); // Bật chữ GO nửa giây
        countdownText.gameObject.SetActive(false); // Ẩn text đếm ngược

        // BẮT ĐẦU GAME: Bật nhạc và bật Spawner
        if (musicSource != null) musicSource.Play();
        if (spawnerScript != null) spawnerScript.enabled = true;
        
        hasGameStarted = true;
    }

    void Update()
    {
        // Kiểm tra nếu game đang chạy và bài nhạc đã dừng lại (hết bài)
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
        hasGameStarted = false; // Ngừng kiểm tra để tránh gọi liên tục
        
        // Dừng sinh quái
        if (spawnerScript != null) spawnerScript.enabled = false;
        
        // Hiển thị bảng tổng kết
        if (endPopup != null) 
        {
            endPopup.SetActive(true);
            
            // Lấy trực tiếp dòng Text điểm số từ ScoreManager để hiển thị
            if (ScoreManager.instance != null && finalScoreText != null)
            {
                finalScoreText.text = "TRACK CLEAR!\n\n" + ScoreManager.instance.scoreText.text;
            }
        }
    }
}