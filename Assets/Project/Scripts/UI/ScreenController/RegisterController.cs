using TMPro;
using UnityEngine;

public class RegisterController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Register";

    [SerializeField] private TMP_InputField inputEmail;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TMP_InputField inputConfirm;
    [SerializeField] private TextMeshProUGUI messageText;

    public void OnLoginButtonTap()
    {
        ScreenManager.Close();
        ScreenManager.Add<LoginController>(LoginController.NAME);
    }

    public void OnSignupButtonTap()
    {
        string email = inputEmail.text.Trim();
        string password = inputPassword.text.Trim();
        string confirm = inputConfirm.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
        {
            messageText.text = "Please fill in all fields!";
            return;
        }

        if (password.Length < 6)
        {
            messageText.text = "Password must be at least 6 characters!";
            return;
        }

        if (password != confirm)
        {
            messageText.text = "Passwords do not match!";
            return;
        }

        PlayFabAccountManager.Instance.Register(email, password,
            onSuccess: () =>
            {
                messageText.text = "";
                ScreenManager.Close();
                ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
            },
            onError: msg =>
            {
                messageText.text = "Register failed: " + msg;
            });
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}