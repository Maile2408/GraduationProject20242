using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameDataManager : SaiBehaviour
{
    private string savePath;

    protected override void Awake()
    {
        base.Awake();
        savePath = Application.persistentDataPath + "/GameData.json";
        LoadGame();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        Debug.Log("GameDataManager: Checking game components...");
        
        // Kiểm tra xem GameManager đã tồn tại chưa
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is missing!");
        }
    }

    private void Update()
    {
        if (Time.frameCount % 600 == 0) // Auto-save mỗi 10 giây
        {
            SaveGame();
        }
    }

    protected override void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        User user = GameManager.Instance.GetUser();
        City city = GameManager.Instance.GetCity();
        List<Worker> workers = GameManager.Instance.GetWorkers();
        List<Resource> resources = GameManager.Instance.GetResources();

        GameData gameData = new GameData(user, city, workers, resources);
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
