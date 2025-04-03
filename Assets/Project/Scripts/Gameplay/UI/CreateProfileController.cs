using UnityEngine;

public class CreateProfileController : MonoBehaviour, IKeyBack
{
    public const string NAME = "CreateProfile";

    public void OnHomeButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnStartButtonTap()
    {
        ScreenManager.Load<GamePlayController>(GamePlayController.NAME);
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}