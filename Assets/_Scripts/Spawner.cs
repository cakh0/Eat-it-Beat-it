using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Dữ liệu Màn chơi hiện tại")]
    public LevelData currentLevel; 

    [Header("Tham chiếu Prefab & Vị trí")]
    public GameObject normalEnemyPrefab; 
    public GameObject holdEnemyPrefab;   
    public Transform groundSpawnPoint;
    public Transform airSpawnPoint;
    public AudioSource audioSource;

    [Header("Cài đặt Vật lý")]
    public float enemySpeed = 10f; 
    public float distanceToHitZone = 14f;

    [Header("Cài đặt Auto Gen")]
    public bool useAutoGenerate = true;
    [Range(0f, 100f)]
    public float holdNoteChance = 20f; 

    private List<NoteData> activeBeatmap; 
    private double dspSongTime;
    private float travelTime;

    void Start()
    {
        if (currentLevel == null)
        {
            Debug.LogError("CHƯA LẮP LEVEL DATA VÀO SPAWNER!");
            return;
        }

        if (audioSource != null)
        {
            audioSource.clip = currentLevel.songAudio;
        }

        activeBeatmap = new List<NoteData>();

        // Lựa chọn sinh tự động hoặc đọc từ Beatmap map tay của LevelData
        if (useAutoGenerate)
        {
            GenerateBeatmap();
        }
        else
        {
            foreach (NoteData note in currentLevel.beatmap)
            {
                NoteData copyNote = new NoteData();
                copyNote.hitTime = note.hitTime;
                copyNote.lane = note.lane;
                copyNote.noteType = note.noteType;
                copyNote.yPos = note.yPos; // Copy cả tọa độ Y để phục vụ âm thanh theo độ cao
                copyNote.hasSpawned = false;
                activeBeatmap.Add(copyNote);
            }
        }

        travelTime = distanceToHitZone / enemySpeed;
        
        if (audioSource != null) 
        {
            audioSource.Play();
            dspSongTime = AudioSettings.dspTime; 
        }
    }

    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying || currentLevel == null) return;

        double currentSongTime = AudioSettings.dspTime - dspSongTime;

        foreach (NoteData note in activeBeatmap)
        {
            if (!note.hasSpawned)
            {
                float spawnTime = note.hitTime - travelTime;

                if (currentSongTime >= spawnTime)
                {
                    SpawnEnemy(note);
                    note.hasSpawned = true; 
                }
            }
        }
    }

    void GenerateBeatmap()
    {
        float secondsPerBeat = 60f / currentLevel.songBPM; 

        for (float i = currentLevel.startDelay; i < currentLevel.songLength; i += secondsPerBeat)
        {
            NoteData newNote = new NoteData();
            newNote.hitTime = i;
            newNote.lane = Random.Range(0, 2); 
            newNote.yPos = (newNote.lane == 1) ? 96f : 288f; // Gán tọa độ Y giả lập nếu dùng Auto Gen
            
            float randomVal = Random.Range(0f, 100f);
            newNote.noteType = (randomVal <= holdNoteChance) ? 1 : 0;
            
            activeBeatmap.Add(newNote);
        }
    }

    void SpawnEnemy(NoteData note)
    {
        Transform spawnPoint = (note.lane == 0) ? groundSpawnPoint : airSpawnPoint;
        GameObject prefabToUse = (note.noteType == 1) ? holdEnemyPrefab : normalEnemyPrefab;
        
        GameObject newEnemy = Instantiate(prefabToUse, spawnPoint.position, Quaternion.identity);
        
        // Truyền tốc độ di chuyển
        EnemyMovement movementScript = newEnemy.GetComponent<EnemyMovement>();
        if (movementScript != null)
        {
            movementScript.moveSpeed = enemySpeed;
        }

        // Truyền tọa độ Y gốc của nốt vào EnemyController để HitZone nhận diện phát đúng âm thanh SFX 16 độ cao
        EnemyController enemyComp = newEnemy.GetComponent<EnemyController>();
        if (enemyComp != null)
        {
            enemyComp.originalYPos = note.yPos;
        }
    }
}