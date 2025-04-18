using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Settings";

    [SerializeField] private Slider Slider_BGM;
    [SerializeField] private Slider Slider_SFX;

    private void OnEnable()
    {
        float bgmVol = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        Slider_BGM.value = bgmVol;
        Slider_SFX.value = sfxVol;

        AudioManager.Instance?.SetBGMVolume(bgmVol);
        AudioManager.Instance?.SetSFXVolume(sfxVol);
    }

    public void OnApplyButtonTap()
    {
        float bgmVol = Slider_BGM.value;
        float sfxVol = Slider_SFX.value;

        AudioManager.Instance?.SetBGMVolume(bgmVol);
        AudioManager.Instance?.SetSFXVolume(sfxVol);

        PlayerPrefs.SetFloat("BGMVolume", bgmVol);
        PlayerPrefs.SetFloat("SFXVolume", sfxVol);
        PlayerPrefs.Save();

        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
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