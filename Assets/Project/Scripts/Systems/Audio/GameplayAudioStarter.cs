using UnityEngine;

public class GameplayAudioStarter : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance?.SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 1f));
        AudioManager.Instance?.SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
        AudioManager.Instance?.StopBGM();
        AudioManager.Instance?.PlayGameplayBGM();
    }
}
