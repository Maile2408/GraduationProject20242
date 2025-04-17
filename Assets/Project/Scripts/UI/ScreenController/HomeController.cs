using UnityEngine;
using UnityEngine.Networking;

public class HomeController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Home";

    public void OnAchievementsButtonTap()
    {
        ScreenManager.Add<AchievementBoardController>(AchievementBoardController.NAME);
    }

    public void OnLeaderboardButtonTap()
    {

    }

    public void OnFacebookButtonTap()
    {
        Application.OpenURL("https://www.facebook.com/maile.tran.2408/");
    }

    public void OnShareButtonTap()
    {
        string urlToShare = "https://www.facebook.com/maile.tran.2408/";
        string facebookShareUrl = $"https://www.facebook.com/sharer/sharer.php?u={UnityWebRequest.EscapeURL(urlToShare)}";
        Application.OpenURL(facebookShareUrl);
    }

    public void OnSettingsButtonTap()
    {
        ScreenManager.Add<SettingsController>(SettingsController.NAME);
    }

    public void OnPlayButtonTap()
    {
        if (PlayFabAccountManager.Instance.IsLoggedIn)
        {
            if (PlayFabProfileManager.Instance.HasCreatedProfile)
                ScreenManager.Load<GamePlayController>(GamePlayController.NAME);
            else
                ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
        }
        else
        {
            ScreenManager.Add<LoginController>(LoginController.NAME);
        }
    }

    public void OnQuitButtonTap()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}