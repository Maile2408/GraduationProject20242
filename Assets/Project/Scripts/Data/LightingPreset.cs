using UnityEngine;

[CreateAssetMenu(menuName = "Lighting/Lighting Preset")]
public class LightingPreset : ScriptableObject
{
    [Header("Ambient Gradient")]
    public Color dayAmbientSkyColor;
    public Color nightAmbientSkyColor;

    public Color dayAmbientEquatorColor;
    public Color nightAmbientEquatorColor;

    public Color dayAmbientGroundColor;
    public Color nightAmbientGroundColor;

    [Header("Directional Light")]
    public Color dayDirectionalColor;
    public Color nightDirectionalColor;

    [Header("Fog")]
    public Color dayFogColor;
    public Color nightFogColor;

    [Header("Sun Intensity")]
    public float daySunIntensity = 1f;
    public float nightSunIntensity = 0.2f;
}
