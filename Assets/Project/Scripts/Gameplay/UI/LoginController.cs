using UnityEngine;

public class LoginController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Login";

    public void OnSignupButtonTap()
    {
        ScreenManager.Close();
        ScreenManager.Add<RegisterController>(RegisterController.NAME);
    }

    public void OnLoginButtonTap()
    {
        
    }

    public void OnFogotPasswordTap()
    {
        
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