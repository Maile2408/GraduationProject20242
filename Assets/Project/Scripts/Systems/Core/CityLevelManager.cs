using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CityLevelManager : MonoBehaviour
{
    public static CityLevelManager Instance;

    [Header("Level Data")]
    [SerializeField] CityLevelConfig levelConfig;
    private List<LevelData> levelConfigs;

    [Header("Runtime Values")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;

    [System.Serializable]
    public class LevelData
    {
        public int requiredXP;
        public List<int> unlockedBuildings;
        public int rewardCoin;
    }

    public UnityEvent<int> onLevelUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only one CityLevelManager allowed!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        levelConfigs = levelConfig.levelConfigs;
    }

    private void Start()
    {
        UnlockManager.Instance.UnlockInitialBuildings(currentLevel);
        
        //Achievement
        AchievementReporter.ReachLevel(currentLevel);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
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

            foreach (var id in levelData.unlockedBuildings) UnlockManager.Instance.UnlockBuilding(id);

            CurrencyManager.Instance.AddCoin(levelData.rewardCoin);
            GameMessage.Success($"Level up! You reached level {currentLevel} and gained {levelData.rewardCoin} coins!");

            //Achievement
            AchievementReporter.ReachLevel(currentLevel);
            
            Debug.Log($"City leveled up to {currentLevel}! Reward: {levelData.rewardCoin} coins.");
        }
    }

    public int GetCurrentLevel() => currentLevel;
    public int GetCurrentXP() => currentXP;
    public int GetXPToNextLevel()
    {
        if (currentLevel >= levelConfigs.Count) return 0;
        return levelConfigs[currentLevel].requiredXP - currentXP;
    }
}
