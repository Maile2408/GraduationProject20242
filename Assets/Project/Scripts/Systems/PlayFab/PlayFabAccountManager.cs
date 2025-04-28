using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayFabAccountManager : MonoBehaviour
{
    public static PlayFabAccountManager Instance { get; private set; }

    public string PlayFabId { get; private set; }
    public bool IsLoggedIn => !string.IsNullOrEmpty(PlayFabId);
    public string CurrentEmail { get; private set; }


    [SerializeField] private string titleId = "1D709F";

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        PlayFabSettings.staticSettings.TitleId = titleId;
    }

    public void Login(string email, string password, Action onSuccess, Action<string> onError)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, result =>
        {
            PlayFabId = result.PlayFabId;
            CurrentEmail = email;
            PlayerPrefs.SetString("PlayFabID", PlayFabId);
            onSuccess?.Invoke();
        },
        error => onError?.Invoke(error.ErrorMessage));
    }

    public void Register(string email, string password, Action onSuccess, Action<string> onError)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false,
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, result =>
        {
            PlayFabId = result.PlayFabId;
            PlayerPrefs.SetString("PlayFabID", PlayFabId);
            onSuccess?.Invoke();
        },
        error => onError?.Invoke(error.ErrorMessage));
    }

    public void ForgotPassword(string email, Action onSuccess, Action<string> onError)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = titleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request,
            result => onSuccess?.Invoke(),
            error => onError?.Invoke(error.ErrorMessage));
    }

    public void Logout()
    {
        PlayFabId = "";
        PlayerPrefs.DeleteKey("PlayFabID");
    }
}
