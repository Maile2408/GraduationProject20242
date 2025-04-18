using TMPro;
using UnityEngine;

public class LoginController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Login";

    [SerializeField] private TMP_InputField inputEmail;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TextMeshProUGUI messageText;

    public void OnSignupButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
        ScreenManager.Add<RegisterController>(RegisterController.NAME);
    }

    public void OnLoginButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        
        string email = inputEmail.text.Trim();
        string password = inputPassword.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Please enter email and password!";
            return;
        }

        PlayFabAccountManager.Instance.Login(email, password,
            onSuccess: () =>
            {
                PlayFabProfileManager.Instance.LoadProfile(
                onSuccess: () =>
                {
                    messageText.text = "";
                    
                    if (PlayFabProfileManager.Instance.HasCreatedProfile)
                    {
                        ScreenManager.Close();
                    }
                    else
                    {
                        ScreenManager.Close();
                        ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
                    }
                },
                onError: error =>
                {
                    messageText.text = "Login OK, but failed to load profile: " + error;
                });

            },
            onError: errorMsg =>
            {
                messageText.text = "Login failed: " + errorMsg;
            });
    }

    public void OnFogotPasswordTap()
    {
        AudioManager.Instance.PlayButtonTap();
        
        string email = inputEmail.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            messageText.text = "Please enter your email!";
            return;
        }

        PlayFabAccountManager.Instance.ForgotPassword(email,
            onSuccess: () =>
            {
                messageText.text = "<color=white>Recovery email sent!</color>";
            },
            onError: msg =>
            {
                messageText.text = "Failed: " + msg;
            });
    }

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