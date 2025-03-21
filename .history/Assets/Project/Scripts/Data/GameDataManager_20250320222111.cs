using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameDataManager : MonoBehaviour
{
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/GameData.json";
        LoadGame();
    }

    private void Update()
    {
        if (Time.frameCount % 600 == 0) // Auto-save mỗi 10 giây
        {
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        GameData gameData = new GameData(
            GameManager.Instance.GetUser(),
            GameManager.Instance.GetCity(),
            GameManager.Instance.GetWorkers(),
            GameManager.Instance.GetResources()
        );

        string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(savePath);
        GameData gameData = JsonConvert.DeserializeObject<GameData>(json);

        GameManager.Instance.SetUser(gameData.User);
        GameManager.Instance.SetCity(gameData.City);
        GameManager.Instance.SetWorkers(gameData.Workers);
        GameManager.Instance.SetResources(gameData.Resources);

        Debug.Log("Game loaded from: " + savePath);
    }
}
