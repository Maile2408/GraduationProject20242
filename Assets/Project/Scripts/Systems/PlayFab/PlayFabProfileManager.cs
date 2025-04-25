using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class PlayFabProfileManager : MonoBehaviour
{
    public static PlayFabProfileManager Instance { get; private set; }

    public string Username { get; private set; }
    public string CityName { get; private set; }
    public string CharacterType { get; private set; } // King or Queen

    public bool HasCreatedProfile => !string.IsNullOrEmpty(Username);

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SaveProfile(string username, string cityName, string characterType, Action onSuccess, Action<string> onError)
    {
        var data = new Dictionary<string, string>
        {
            { "username", username },
            { "cityName", cityName },
            { "characterType", characterType }
        };

        var request = new UpdateUserDataRequest
        {
            Data = data,
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request,
            result =>
            {
                this.Username = username;
                this.CityName = cityName;
                this.CharacterType = characterType;
                onSuccess?.Invoke();
            },
            error => onError?.Invoke(error.ErrorMessage));
    }

    public void LoadProfile(Action onSuccess, Action<string> onError)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
        result =>
        {
            if (result.Data != null)
            {
                Username = result.Data.ContainsKey("username") ? result.Data["username"].Value : "";
                CityName = result.Data.ContainsKey("cityName") ? result.Data["cityName"].Value : "";
                CharacterType = result.Data.ContainsKey("characterType") ? result.Data["characterType"].Value : "";
            }
            onSuccess?.Invoke();
        },
        error => onError?.Invoke(error.ErrorMessage));
    }

    public void Reset()
    {
        Username = null;
        CityName = null;
        CharacterType = "";

        //Debug.Log("[PlayFabProfileManager] Profile reset.");
    }

}
