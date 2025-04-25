using UnityEngine;
using UnityEngine.Networking;

public class HomeController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Home";

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

    public void OnFacebookButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        Application.OpenURL("https://www.facebook.com/maile.tran.2408/");
    }

    public void OnShareButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        string urlToShare = "https://www.facebook.com/maile.tran.2408/";
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

        if (!HomeLoader.IsReadyToPlay) return;

        if (!PlayFabAccountManager.Instance.IsLoggedIn)
        {
            ScreenManager.Add<LoginController>(LoginController.NAME);
            return;
        }

        if (PlayFabProfileManager.Instance.HasCreatedProfile)
        {
            ScreenManager.Load<GamePlayController>(GamePlayController.NAME);
        }
        else
        {
            ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
        }
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