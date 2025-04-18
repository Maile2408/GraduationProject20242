using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private AudioClip bgm_home, bgm_gameplay;
    private AudioClip sfx_button_tap, sfx_tax_collect, sfx_reward, sfx_message_show, sfx_place, sfx_build_rotate, sfx_build_destroy;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        InitAudioSources();
        LoadClips();
    }

    private void InitAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
    }

    private void LoadClips()
    {
        bgm_home = Resources.Load<AudioClip>("SFX/bgm_home");
        bgm_gameplay = Resources.Load<AudioClip>("SFX/bgm_gameplay");

        sfx_button_tap = Resources.Load<AudioClip>("SFX/sfx_button_tap");
        sfx_tax_collect = Resources.Load<AudioClip>("SFX/sfx_tax_collect");
        sfx_reward = Resources.Load<AudioClip>("SFX/sfx_reward");
        sfx_message_show = Resources.Load<AudioClip>("SFX/sfx_message_show");
        sfx_place = Resources.Load<AudioClip>("SFX/sfx_place");
        sfx_build_rotate = Resources.Load<AudioClip>("SFX/sfx_build_rotate");
        sfx_build_destroy = Resources.Load<AudioClip>("SFX/sfx_build_destroy");
    }

    public void PlayHomeBGM()
    {
        PlayBGM(bgm_home);
    }

    public void PlayGameplayBGM()
    {
        PlayBGM(bgm_gameplay);
    }

    private void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayButtonTap() => PlaySFX(sfx_button_tap);
    public void PlayTaxCollect() => PlaySFX(sfx_tax_collect);
    public void PlayReward() => PlaySFX(sfx_reward);
    public void PlayMessageShow() => PlaySFX(sfx_message_show);
    public void PlayWorkerSpawn() => PlaySFX(sfx_place);
    public void PlayBuildingPlace() => PlaySFX(sfx_place);
    public void PlayBuildRotate() => PlaySFX(sfx_build_rotate);
    public void PlayBuildingDestroy() => PlaySFX(sfx_build_destroy);

    public void SetBGMVolume(float volume) => bgmSource.volume = volume;
    public void SetSFXVolume(float volume) => sfxSource.volume = volume;
}
