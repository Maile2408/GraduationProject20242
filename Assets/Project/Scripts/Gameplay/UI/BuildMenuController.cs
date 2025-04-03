using UnityEngine;

public class BuildMenuController : MonoBehaviour, IKeyBack
{
    public const string NAME = "BuildMenu";

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}