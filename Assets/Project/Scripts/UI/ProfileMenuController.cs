using TMPro;
using UnityEngine;

public class ProfileMenuController : MonoBehaviour, IKeyBack
{
    public const string NAME = "ProfileMenu";

    [SerializeField] private TextMeshProUGUI textYourName;
    [SerializeField] private TextMeshProUGUI textCityName;
    [SerializeField] private TextMeshProUGUI textCharacter;

    private void OnEnable()
    {
        var profile = PlayFabProfileManager.Instance;
        textYourName.text = $"Your Name: <color=green>{profile.Username}</color>";
        textCityName.text = $"City Name: <color=green>{profile.CityName}</color>";
        textCharacter.text = $"Character: <color=green>{profile.CharacterType}</color>";
    }

    public void OnAchievementsButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
        ScreenManager.Add<AchievementBoardController>(AchievementBoardController.NAME);
    }

    public void OnLeaderboardButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
        ScreenManager.Add<LeaderboardController>(LeaderboardController.NAME);
    }

    public void OnHomeButtonTap()
    {
        SaveStateCollector.Instance.SaveAll();
        SaveManager.Instance.SaveAndUpload();

        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Load<HomeController>(HomeController.NAME);
    }

    public void OnLogoutButtonTap()
    {
        SaveStateCollector.Instance.SaveAll();
        SaveManager.Instance.SaveAndUpload();

        AudioManager.Instance.PlayButtonTap();
        PlayFabLoginFlow.Instance.Logout();
        ScreenManager.Load<HomeController>(HomeController.NAME);

    }

    public void OnCloseButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}
