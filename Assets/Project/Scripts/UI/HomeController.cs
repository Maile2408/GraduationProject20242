using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HomeController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Home";

    public static HomeController Instance;

    [SerializeField] Button playButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button logoutButton;

    [SerializeField] TextMeshProUGUI accountInfoText;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void UpdateState()
    {
        bool isLoggedIn = PlayFabAccountManager.Instance.IsLoggedIn;

        playButton.gameObject.SetActive(!isLoggedIn);
        continueButton.gameObject.SetActive(isLoggedIn);
        logoutButton.gameObject.SetActive(isLoggedIn);

        if (isLoggedIn)
        {
            accountInfoText.gameObject.SetActive(true);
            string email = PlayFabAccountManager.Instance.CurrentEmail;
            if (string.IsNullOrEmpty(email)) email = "Guest";
            accountInfoText.text = $"Account: {email}";
        }
        else
        {
            accountInfoText.gameObject.SetActive(true);
            accountInfoText.text = $"Account: Guest";
        }


    }

    public void OnAchievementsButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        if (PlayFabAccountManager.Instance.IsLoggedIn)
        {
            if (PlayFabProfileManager.Instance.HasCreatedProfile)
                ScreenManager.Add<AchievementBoardController>(AchievementBoardController.NAME);
            else
                ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
        }
        else
        {
            ScreenManager.Add<LoginController>(LoginController.NAME);
        }
    }

    public void OnLeaderboardButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        if (PlayFabAccountManager.Instance.IsLoggedIn)
        {
            if (PlayFabProfileManager.Instance.HasCreatedProfile)
                ScreenManager.Add<LeaderboardController>(LeaderboardController.NAME);
            else
                ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
        }
        else
        {
            ScreenManager.Add<LoginController>(LoginController.NAME);
        }
    }

    public void OnGameButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        Application.OpenURL("https://maile2408.itch.io/realm-builder");
    }

    public void OnShareButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        string urlToShare = "https://maile2408.itch.io/realm-builder";
        string facebookShareUrl = $"https://www.facebook.com/sharer/sharer.php?u={UnityWebRequest.EscapeURL(urlToShare)}";
        Application.OpenURL(facebookShareUrl);
    }

    public void OnSettingsButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Add<SettingsController>(SettingsController.NAME);
    }

    public void OnPlayButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        if (!PlayFabAccountManager.Instance.IsLoggedIn)
        {
            ScreenManager.Add<LoginController>(LoginController.NAME);
            return;
        }
    }

    public void OnContinueButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        if (!PlayFabProfileManager.Instance.HasCreatedProfile)
        {
            ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
            return;
        }

        ScreenManager.Load<GamePlayController>(GamePlayController.NAME);
    }

    public void OnLogoutButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        if (PlayFabAccountManager.Instance.IsLoggedIn)
        {
            string email = PlayFabAccountManager.Instance.CurrentEmail;
            if (string.IsNullOrEmpty(email)) email = "Guest";

            ConfirmationPopupController.OnYesCallback = OnConfirmYes;
            ConfirmationPopupController.OnNoCallback = OnConfirmNo;
            ConfirmationPopupController.Message = $"Are you sure you want to log out from {email}?";

            ScreenManager.Add<ConfirmationPopupController>(ConfirmationPopupController.NAME);
        }
    }

    private void OnConfirmYes()
    {
        SaveManager.Instance.SaveAndUpload();
        PlayFabLoginFlow.Instance.Logout();
        ClearCallbacks();
    }

    private void OnConfirmNo()
    {
        ClearCallbacks();
    }

    private void ClearCallbacks()
    {
        ConfirmationPopupController.OnYesCallback = null;
        ConfirmationPopupController.OnNoCallback = null;
    }

    public void OnQuitButtonTap()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            AudioManager.Instance.PlayButtonTap();
            Application.Quit();
#endif
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}