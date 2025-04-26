using UnityEngine;

public class HelpController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Help";

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