using System;
using System.Collections.Generic;
using UnityEngine;

public class StatsReader : Singleton<StatsReader>
{
    [SerializeField] private string filePath = "Assets/Data/yourfile.csv";
    public List<GameStats> statsList = new List<GameStats>();

    void Awake()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        string[] rows = System.IO.File.ReadAllLines(filePath);

        for (int i = 1; i < rows.Length; i++)
        {
            string[] fields = rows[i].Split(',');
            if (fields.Length >= 6) // ตรวจสอบว่ามีคอลัมน์ครบถ้วน
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

        // ตรวจสอบข้อมูลโดยการพิมพ์ออกมาดู
        foreach (GameStats stat in statsList)
        {
            Debug.Log($"{stat.Name}: Amount={stat.Amount}, Damage={stat.Damage}, HP={stat.HP}, MissedChance={stat.MissedChance}, Second={stat.Second}");
        }
    }

    // ฟังก์ชันเสริมสำหรับการแปลงข้อมูลเป็น int อย่างปลอดภัย
    int ParseIntSafe(string input)
    {
        int value;
        if (int.TryParse(input, out value))
            return value;
        else
            return 0; // คืนค่า 0 หากแปลงไม่สำเร็จ
    }
    public GameStats GetStat(string Name)
    {
        return statsList.Find((x) => x.Name == Name);
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
