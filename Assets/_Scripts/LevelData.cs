using UnityEngine;
using System.Collections.Generic;

// Dòng này giúp bạn có thể click chuột phải tạo Level mới ngay trong Editor
[CreateAssetMenu(fileName = "NewLevel", menuName = "Rhythm Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string levelName;
    public AudioClip songAudio; // File nhạc của level này
    
    [Header("Cài đặt Nhịp độ")]
    public float songBPM;
    public float startDelay;
    public float songLength;

    [Header("Kịch bản nốt nhạc")]
    public List<NoteData> beatmap; 
}