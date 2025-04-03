using UnityEngine;

public class ProfileMenuController : MonoBehaviour, IKeyBack
{
    public const string NAME = "ProfileMenu";

    public void OnAchievementsButtonTap()
    {

    }

    public void OnLeaderboardButtonTap()
    {

    }

    public void OnHomeButtonTap()
    {
        ScreenManager.Load<HomeController>(HomeController.NAME);
    }

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}