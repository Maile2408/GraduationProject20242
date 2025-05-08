using UnityEngine;

public class SplashController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Splash";

    private void Start()
    {
        LoadingRequest.targetScene = HomeController.NAME;
        LoadingRequest.loadStage = LoadingRequest.LoadStage.LoadProfileToHome;

        ScreenManager.Load<LoadingController>(LoadingController.NAME);
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}