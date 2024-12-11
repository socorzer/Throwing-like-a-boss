using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // เพิ่มไลบรารีนี้เพื่อใช้ UnityWebRequest

public class StatsReader : Singleton<StatsReader>
{
    [SerializeField] private string fileName = "yourfile.csv"; // ตั้งชื่อไฟล์ CSV
    public List<GameStats> statsList = new List<GameStats>();

    void Awake()
    {
        StartCoroutine(LoadCSVFromStreamingAssets()); // เปลี่ยนจาก LoadCSV() เป็น coroutine ใหม่
    }

    IEnumerator LoadCSVFromStreamingAssets()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest(); // ส่งคำขอและรอจนกว่าจะเสร็จสิ้น

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Failed to load file: " + request.error);
            }
            else
            {
                // โหลดข้อมูลจากไฟล์ CSV
                ParseCSV(request.downloadHandler.text); // แปลงข้อมูลที่ได้จาก request
            }
        }
    }

    void ParseCSV(string csvData)
    {
        string[] rows = csvData.Split('\n');

        for (int i = 1; i < rows.Length; i++)
        {
            string[] fields = rows[i].Split(',');
            if (fields.Length >= 6)
            {
                GameStats stat = new GameStats();
                stat.Name = fields[0];
                stat.Amount = ParseIntSafe(fields[1]);
                stat.Damage = ParseIntSafe(fields[2]);
                stat.HP = ParseIntSafe(fields[3]);
                stat.MissedChance = ParseIntSafe(fields[4]);
                stat.Second = ParseIntSafe(fields[5]);

                statsList.Add(stat);
            }
        }

        foreach (GameStats stat in statsList)
        {
            Debug.Log($"{stat.Name}: Amount={stat.Amount}, Damage={stat.Damage}, HP={stat.HP}, MissedChance={stat.MissedChance}, Second={stat.Second}");
        }
    }

    int ParseIntSafe(string input)
    {
        int value;
        if (int.TryParse(input, out value))
            return value;
        return 0;
    }

    public GameStats GetStat(string name)
    {
        return statsList.Find(x => x.Name == name);
    }
}

[Serializable]
public class GameStats
{
    public string Name;
    public int Amount;
    public int Damage;
    public int HP;
    public int MissedChance;
    public int Second;
}
