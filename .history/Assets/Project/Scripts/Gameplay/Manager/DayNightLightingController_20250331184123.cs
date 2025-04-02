using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightLightingController : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField] LightingPreset preset;
    [SerializeField] Light sun;
    [SerializeField] float transitionSpeed = 1f;

    [Header("Post Processing")]
    [SerializeField] Volume postProcessingVolume;
    [SerializeField] float dayExposure = -0.5f;
    [SerializeField] float nightExposure = 0.4f;

    Color curSky, curEquator, curGround;
    Color targetSky, targetEquator, targetGround;

    Color curFog, targetFog;
    Color curDirColor, targetDirColor;
    float curIntensity, targetIntensity;

    ColorAdjustments colorAdjustments;
    float currentExposure, targetExposure;

    private void OnEnable()
    {
        TimeManager.OnDayStart += SetDayTarget;
        TimeManager.OnNightStart += SetNightTarget;
    }

    private void OnDisable()
    {
        TimeManager.OnDayStart -= SetDayTarget;
        TimeManager.OnNightStart -= SetNightTarget;
    }

    private void Start()
    {
        if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            currentExposure = colorAdjustments.postExposure.value;
        }

        if (TimeManager.Instance.IsDay)
            SetDayTarget();
        else
            SetNightTarget();

        curSky = RenderSettings.ambientSkyColor;
        curEquator = RenderSettings.ambientEquatorColor;
        curGround = RenderSettings.ambientGroundColor;
        curFog = RenderSettings.fogColor;
        curDirColor = sun.color;
        curIntensity = sun.intensity;
    }

    private void Update()
    {
        float t = Time.deltaTime * transitionSpeed;

        // Lighting
        curSky = Color.Lerp(curSky, targetSky, t);
        curEquator = Color.Lerp(curEquator, targetEquator, t);
        curGround = Color.Lerp(curGround, targetGround, t);
        curFog = Color.Lerp(curFog, targetFog, t);
        curDirColor = Color.Lerp(curDirColor, targetDirColor, t);
        curIntensity = Mathf.Lerp(curIntensity, targetIntensity, t);

        RenderSettings.ambientSkyColor = curSky;
        RenderSettings.ambientEquatorColor = curEquator;
        RenderSettings.ambientGroundColor = curGround;
        RenderSettings.fogColor = curFog;

        if (sun != null)
        {
            sun.color = curDirColor;
            sun.intensity = curIntensity;
        }

        // Exposure
        if (colorAdjustments != null)
        {
            currentExposure = Mathf.Lerp(currentExposure, targetExposure, t);
            colorAdjustments.postExposure.value = currentExposure;
        }
    }

    private void SetDayTarget()
    {
        targetSky = preset.dayAmbientSkyColor;
        targetEquator = preset.dayAmbientEquatorColor;
        targetGround = preset.dayAmbientGroundColor;

        targetFog = preset.dayFogColor;
        targetDirColor = preset.dayDirectionalColor;
        targetIntensity = preset.daySunIntensity;

        targetExposure = dayExposure;
    }

    private void SetNightTarget()
    {
        targetSky = preset.nightAmbientSkyColor;
        targetEquator = preset.nightAmbientEquatorColor;
        targetGround = preset.nightAmbientGroundColor;

        targetFog = preset.nightFogColor;
        targetDirColor = preset.nightDirectionalColor;
        targetIntensity = preset.nightSunIntensity;

        targetExposure = nightExposure;
    }
}
