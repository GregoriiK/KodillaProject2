using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public struct GameSaveData
{
    public float m_timeSinceLastSave;
    public float m_overallTime;
    public int m_lifetimeHits;
    public float m_masterVolume;
}

public class SaveManager : Singleton<SaveManager>
{
    public GameSaveData SaveData;

    public bool UseBinary = true;

    private string m_pathJSON;
    private string m_pathBin;

    private void Start()
    {
        m_pathBin = Path.Combine(Application.persistentDataPath, "save.bin");
        m_pathJSON = Path.Combine(Application.persistentDataPath, "save.json");
        SaveData.m_timeSinceLastSave = 0f;
        SaveData.m_masterVolume = AudioListener.volume;
        LoadSettings();
    }

    private void Update()
    {
        SaveData.m_timeSinceLastSave += Time.deltaTime;
    }

    public void SaveSettings()
    {
        SaveData.m_overallTime += SaveData.m_timeSinceLastSave;
        //Debug.Log("Saving overall time value: " + m_overallTime);
        //PlayerPrefs.SetFloat("OverallTime", m_overallTime);
        SaveData.m_timeSinceLastSave = 0.0f;
        if (UseBinary)
        {
            FileStream file = new FileStream(m_pathBin, FileMode.OpenOrCreate);
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(file, SaveData);
            file.Close();
        }
        else
        {
            string saveData = JsonUtility.ToJson(SaveData);
            File.WriteAllText(m_pathJSON, saveData);
        }
    }

    public void LoadSettings()
    {
        //m_overallTime = PlayerPrefs.GetFloat("OverallTime", 0.0f);
        //Debug.Log("Loaded overall time value: " + m_overallTime);
        if (UseBinary && File.Exists(m_pathBin))
        {
            FileStream file = new FileStream(m_pathBin, FileMode.Open);
            BinaryFormatter binFormat = new BinaryFormatter();
            SaveData = (GameSaveData)binFormat.Deserialize(file);
            file.Close();
            ApplySettings();
        }
        else if (!UseBinary && File.Exists(m_pathJSON))
        {
            string saveData = File.ReadAllText(m_pathJSON);
            SaveData = JsonUtility.FromJson<GameSaveData>(saveData);
            ApplySettings();
        }
        else
        {
            SaveData.m_timeSinceLastSave = 0f;
            SaveData.m_overallTime = 0f;
            SaveData.m_masterVolume = AudioListener.volume;
        }
    }

    public void ApplySettings()
    {
        AudioListener.volume = SaveData.m_masterVolume;
    }
}
