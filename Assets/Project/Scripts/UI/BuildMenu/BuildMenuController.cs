using UnityEngine;

public class BuildMenuController : MonoBehaviour, IKeyBack
{
    public const string NAME = "BuildMenu";

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