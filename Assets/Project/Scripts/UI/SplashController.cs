using UnityEngine;

public class SplashController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Splash";

    private void OnEnable()
    {
        LoadingController.Show(LoadingController.LoadType.ProfileToHome);
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}