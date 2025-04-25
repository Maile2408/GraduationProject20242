using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CityLevelManager : MonoBehaviour
{
    public static CityLevelManager Instance;

    [Header("Level Data")]
    [SerializeField] private CityLevelConfig levelConfig;
    private List<LevelData> levelConfigs;

    [Header("Runtime Values")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;

    public UnityEvent<int> onLevelUp;
    public UnityEvent<int> onLevelChanged;
    public UnityEvent<int> onXPChanged;

    [System.Serializable]
    public class LevelData
    {
        public int requiredXP;
        public List<int> unlockedBuildings;
        public int rewardCoin;
    }

    public int Level
    {
        get => currentLevel;
        set
        {
            currentLevel = Mathf.Max(1, value);
            onLevelChanged?.Invoke(currentLevel);
            AchievementReporter.ReachLevel(currentLevel);
        }
    }

    public int XP
    {
        get => currentXP;
        set
        {
            currentXP = Mathf.Max(0, value);
            onXPChanged?.Invoke(currentXP);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        levelConfigs = levelConfig.levelConfigs;
    }

    private void Start()
    {
        onLevelChanged?.Invoke(currentLevel);
        onXPChanged?.Invoke(currentXP);
    }

    public void AddXP(int amount)
    {
        XP += amount;
        TryLevelUp();
    }

    private void TryLevelUp()
    {
        while (currentLevel < levelConfigs.Count && currentXP >= levelConfigs[currentLevel].requiredXP)
        {
            currentXP -= levelConfigs[currentLevel].requiredXP;
            currentLevel++;

            var levelData = levelConfigs[currentLevel - 1];

            onLevelUp?.Invoke(currentLevel);
            onLevelChanged?.Invoke(currentLevel);

            foreach (var id in levelData.unlockedBuildings)
            {
                bool isNew = !UnlockManager.Instance.IsUnlocked(id);
                UnlockManager.Instance.UnlockBuilding(id);

                //Achievement
                if (isNew) AchievementReporter.UnlockBuilding();
            }

            CurrencyManager.Instance.AddCoin(levelData.rewardCoin);
            GameMessage.Success($"Level up! You reached level {currentLevel} and gained {levelData.rewardCoin} coins!");

            AchievementReporter.ReachLevel(currentLevel);
        }
    }

    public int GetXPToNextLevel()
    {
        if (currentLevel >= levelConfigs.Count) return 0;
        return levelConfigs[currentLevel].requiredXP - currentXP;
    }

    public void SetLevelAndXP(int level, int xp)
    {
        this.Level = level;
        this.XP = xp;
    }
}
