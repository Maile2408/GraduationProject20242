using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgress : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private Image fillImage;

    private int lastXP = -1;
    private int lastLevel = -1;

    private void OnEnable()
    {
        if (CityLevelManager.Instance != null)
        {
            CityLevelManager.Instance.onLevelUp.AddListener(UpdateUI);
            UpdateUI(CityLevelManager.Instance.Level);
        }
    }

    private void OnDisable()
    {
        if (CityLevelManager.Instance != null)
            CityLevelManager.Instance.onLevelUp.RemoveListener(UpdateUI);
    }

    private void Update()
    {
        if (CityLevelManager.Instance == null) return;

        int currentXP = CityLevelManager.Instance.XP;
        int currentLevel = CityLevelManager.Instance.Level;

        // Only update if XP or level has changed
        if (currentXP != lastXP || currentLevel != lastLevel)
        {
            UpdateUI(currentLevel);
            lastXP = currentXP;
            lastLevel = currentLevel;
        }
    }

    private void UpdateUI(int level)
    {
        if (textLevel != null)
            textLevel.text = $"Level {level}";

        UpdateFill();
    }

    private void UpdateFill()
    {
        if (fillImage == null || CityLevelManager.Instance == null) return;

        float xp = CityLevelManager.Instance.XP;
        float toNext = CityLevelManager.Instance.GetXPToNextLevel();
        float ratio = toNext == 0 ? 1f : xp / (xp + toNext);
        fillImage.fillAmount = Mathf.Clamp01(ratio);
    }
}
