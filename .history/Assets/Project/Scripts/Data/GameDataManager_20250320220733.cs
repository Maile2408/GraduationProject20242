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
        User user = GameManager.Instance.GetUser();
        City city = GameManager.Instance.GetCity();

        GameData gameData = new GameData(user, city);
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

        Debug.Log("Game loaded from: " + savePath);
    }
}

[Serializable]
public class GameData
{
    public User User;
    public City City;

    public GameData(User user, City city)
    {
        this.User = user;
        this.City = city;
    }
}
