using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Login";

    [Header("UI")]
    [SerializeField] private TMP_InputField inputEmail;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Toggle rememberMeToggle;
    [SerializeField] private TextMeshProUGUI rememberMeLabel;

    [Header("Dropdown Suggestion")]
    [SerializeField] private TMP_Dropdown dropdownSuggestions;
    [SerializeField] private Button buttonShowDropdown;
    [SerializeField] private Image dropdownArrowImage;
    [SerializeField] private Sprite arrowUp;
    [SerializeField] private Sprite arrowDown;

    private bool isDropdownOpen = false;

    private void Start()
    {
        RefreshDropdownEmails();

        if (RememberMeManager.IsRemembered())
        {
            rememberMeToggle.isOn = true;
            inputEmail.text = RememberMeManager.GetEmail();
            inputPassword.text = RememberMeManager.GetPassword();
            UpdateRememberMeLabel(true);
        }
        else
        {
            rememberMeToggle.isOn = false;
            UpdateRememberMeLabel(false);
        }

        buttonShowDropdown.onClick.AddListener(() =>
        {
            isDropdownOpen = !isDropdownOpen;
            if (isDropdownOpen)
            {
                dropdownSuggestions.Show();
                SetDropdownArrowUp();
            }
            else
            {
                dropdownSuggestions.Hide();
                SetDropdownArrowDown();
            }
        });

        dropdownSuggestions.onValueChanged.AddListener(index =>
        {
            string selectedEmail = dropdownSuggestions.options[index].text;
            inputEmail.text = selectedEmail;
            inputPassword.text = RememberMeManager.GetPasswordOf(selectedEmail);
            dropdownSuggestions.Hide();
            isDropdownOpen = false;
            SetDropdownArrowDown();
        });

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((_) =>
        {
            if (isDropdownOpen)
            {
                dropdownSuggestions.Hide();
                isDropdownOpen = false;
                SetDropdownArrowDown();
            }
        });
        trigger.triggers.Add(entry);

        rememberMeToggle.onValueChanged.AddListener(UpdateRememberMeLabel);
    }

    private void OnDisable()
    {
        buttonShowDropdown.onClick.RemoveAllListeners();
        dropdownSuggestions.onValueChanged.RemoveAllListeners();
        rememberMeToggle.onValueChanged.RemoveListener(UpdateRememberMeLabel);
    }

    private void RefreshDropdownEmails()
    {
        var savedEmails = RememberMeManager.GetEmailList();
        dropdownSuggestions.ClearOptions();
        dropdownSuggestions.AddOptions(savedEmails);
    }

    private void SetDropdownArrowUp() => dropdownArrowImage.sprite = arrowUp;
    private void SetDropdownArrowDown() => dropdownArrowImage.sprite = arrowDown;

    private void UpdateRememberMeLabel(bool isOn)
    {
        if (rememberMeLabel != null)
            rememberMeLabel.color = isOn ? Color.green : Color.gray;
    }

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

        PlayFabLoginFlow.Instance.LoginAndLoadData(email, password,
            onComplete: () =>
            {
                if (rememberMeToggle.isOn)
                    RememberMeManager.Save(email, password);
                else
                    RememberMeManager.Clear();

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
            onError: errorMsg =>
            {
                messageText.text = errorMsg;
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
