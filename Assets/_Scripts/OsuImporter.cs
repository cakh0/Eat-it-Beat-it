using UnityEngine;
using System.Collections.Generic;

public class OsuImporter : MonoBehaviour
{
    [Header("File osu! (đã đổi đuôi .txt)")]
    public TextAsset osuFile;

    [Header("Level Data đích")]
    public LevelData targetLevel; 

    // Tính năng này cho phép bấm nút chạy code ngay trên giao diện Unity mà không cần Play game
    [ContextMenu("Import Từ File Osu")]
    public void ParseOsuFile()
    {
        if (osuFile == null || targetLevel == null)
        {
            Debug.LogError("Thiếu file osu hoặc LevelData đích!");
            return;
        }

        string[] lines = osuFile.text.Split('\n');
        bool isHitObjectSection = false;
        List<NoteData> importedBeatmap = new List<NoteData>();

        foreach (string line in lines)
        {
            string cleanLine = line.Trim();
            if (string.IsNullOrEmpty(cleanLine)) continue;

            // Dò tìm đến khu vực chứa nốt nhạc trong file txt
            if (cleanLine == "[HitObjects]")
            {
                isHitObjectSection = true;
                continue;
            }

            // Bắt đầu đọc dữ liệu nốt
            if (isHitObjectSection)
            {
                string[] data = cleanLine.Split(',');
                if (data.Length >= 3)
                {
                    float yPos = float.Parse(data[1]); // Tọa độ Y
                    float timeMs = float.Parse(data[2]); // Thời gian (mili-giây)

                    NoteData newNote = new NoteData();
                    
                    // 1. Chuyển đổi thời gian từ mili-giây sang giây
                    newNote.hitTime = timeMs / 1000f; 
                    
                    // 2. Phân làn dựa vào tọa độ Y (Nửa trên = Air, Nửa dưới = Ground)
                    newNote.lane = (yPos < 192) ? 1 : 0; 
                    
                    // 3. Mặc định là nốt thường
                    newNote.noteType = 0; 

                    // 4. LƯU LẠI TỌA ĐỘ Y ĐỂ DÙNG CHO ÂM THANH 16 ĐỘ CAO (QUAN TRỌNG)
                    newNote.yPos = yPos; 

                    importedBeatmap.Add(newNote);
                }
            }
        }

        // Ghi đè danh sách nốt vào LevelData
        targetLevel.beatmap = importedBeatmap;
        Debug.Log("Import thành công " + importedBeatmap.Count + " nốt nhạc vào Level: " + targetLevel.name);
    }
}