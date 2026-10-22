using UnityEngine; // Thêm dòng này vào đầu file

[System.Serializable]
public class NoteData
{
    public float hitTime;
    public int lane;
    public int noteType;
    public float yPos; // Tọa độ Y từ file osu!
    [HideInInspector] public bool hasSpawned = false; 
}