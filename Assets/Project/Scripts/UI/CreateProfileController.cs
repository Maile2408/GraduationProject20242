using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateProfileController : MonoBehaviour, IKeyBack
{
    public const string NAME = "CreateProfile";

    [SerializeField] private TMP_InputField inputYourName;
    [SerializeField] private TMP_InputField inputCityName;
    [SerializeField] private Toggle toggleKing;
    [SerializeField] private Toggle toggleQueen;
    [SerializeField] private TextMeshProUGUI messageText;

    public void OnStartButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

        string yourName = inputYourName.text.Trim();
        string cityName = inputCityName.text.Trim();
        string character = toggleKing.isOn ? "King" : "Queen";

        if (string.IsNullOrEmpty(yourName) || string.IsNullOrEmpty(cityName))
        {
            messageText.text = "Fill in all profile info!";
            return;
        }

        PlayFabProfileManager.Instance.SaveProfile(yourName, cityName, character,
            onSuccess: () =>
            {
                messageText.text = "";

                var saveData = SaveDataFactory.CreateDefaultSaveData(
                    PlayFabAccountManager.Instance.PlayFabId,
                    yourName,
                    cityName,
                    character
                );

                SaveManager.Instance.CurrentData = saveData;
                SaveManager.Instance.SaveAndUpload();

                LoadingController.Show(LoadingController.LoadType.GameDataToGamePlay);
            },
            onError: msg =>
            {
                messageText.text = "Failed to save profile: " + msg;
            });
    }

    public void OnHomeButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}
