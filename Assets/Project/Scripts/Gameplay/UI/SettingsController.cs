using UnityEngine;

public class SettingsController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Settings";

    public void OnApplyButtonTap()
    {
        ScreenManager.Close();
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