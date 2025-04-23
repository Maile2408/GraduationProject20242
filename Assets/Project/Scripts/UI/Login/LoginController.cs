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

        // Auto-fill if remembered
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

        // Toggle dropdown visibility
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

        // Handle email selection
        dropdownSuggestions.onValueChanged.AddListener(index =>
        {
            string selectedEmail = dropdownSuggestions.options[index].text;
            inputEmail.text = selectedEmail;

            string password = RememberMeManager.GetPasswordOf(selectedEmail);
            inputPassword.text = password;

            dropdownSuggestions.Hide();
            isDropdownOpen = false;
            SetDropdownArrowDown();
        });

        // Click outside to hide dropdown
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((eventData) =>
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

    private void RefreshDropdownEmails()
    {
        var savedEmails = RememberMeManager.GetEmailList();
        dropdownSuggestions.ClearOptions();
        dropdownSuggestions.AddOptions(savedEmails);
    }

    private void OnDisable()
    {
        buttonShowDropdown.onClick.RemoveAllListeners();
        dropdownSuggestions.onValueChanged.RemoveAllListeners();
        rememberMeToggle.onValueChanged.RemoveListener(UpdateRememberMeLabel);
    }

    private void SetEmailBackgroundColor(Color color)
    {
        var bg = inputEmail.GetComponent<Image>();
        if (bg != null) bg.color = color;
    }

    private void SetPasswordBackgroundColor(Color color)
    {
        var bg = inputPassword.GetComponent<Image>();
        if (bg != null) bg.color = color;
    }

    private void SetDropdownArrowUp()
    {
        if (dropdownArrowImage != null && arrowUp != null)
            dropdownArrowImage.sprite = arrowUp;
    }

    private void SetDropdownArrowDown()
    {
        if (dropdownArrowImage != null && arrowDown != null)
            dropdownArrowImage.sprite = arrowDown;
    }

    private void UpdateRememberMeLabel(bool isOn)
    {
        if (rememberMeLabel == null) return;
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

        PlayFabAccountManager.Instance.Login(email, password,
            onSuccess: () =>
            {
                if (rememberMeToggle.isOn)
                    RememberMeManager.Save(email, password);
                else
                    RememberMeManager.Clear();

                PlayFabProfileManager.Instance.LoadProfile(
                    onSuccess: () =>
                    {
                        PlayFabManager.Instance.DownloadUserData(data =>
                        {
                            if (data != null)
                            {
                                SaveManager.Instance.CurrentData = data;
                                SaveManager.Instance.SaveToDisk(); // Backup local
                                messageText.text = "";
                            }
                            else
                            {
                                SaveManager.Instance.CurrentData = new UserSaveData();
                                messageText.text = "No cloud save found, using default.";
                            }

                            if (PlayFabProfileManager.Instance.HasCreatedProfile)
                            {
                                ScreenManager.Close();
                            }
                            else
                            {
                                ScreenManager.Close();
                                ScreenManager.Add<CreateProfileController>(CreateProfileController.NAME);
                            }
                        });
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
