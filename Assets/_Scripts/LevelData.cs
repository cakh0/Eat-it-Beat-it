using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Rhythm Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string levelName;
    public AudioClip songAudio;
    
    [Header("Cài đặt Nhịp độ")]
    public float songBPM;
    public float startDelay;
    public float songLength;

    // THÊM 2 DÒNG NÀY ĐỂ LEVEL TỰ LƯU THÔNG SỐ VẬT LÝ
    [Header("Cài đặt Vật lý")]
    public float enemySpeed = 12.92f; 
    public float distanceToHitZone = 14f;

    [Header("Kịch bản nốt nhạc")]
    public List<NoteData> beatmap; 
}