using UnityEngine;

public class RegisterController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Register";

    public void OnLoginButtonTap()
    {
        ScreenManager.Close();
        ScreenManager.Add<LoginController>(LoginController.NAME);
    }

    public void OnSignupButtonTap()
    {
        ScreenManager.Close();
        ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}