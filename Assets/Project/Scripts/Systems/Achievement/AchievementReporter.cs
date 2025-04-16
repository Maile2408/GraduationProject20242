using UnityEngine;

public static class AchievementReporter
{
    // Build
    public static void FirstBuild() => Report(AchievementType.FirstBuild, 1);
    public static void BuildStructure() => Report(AchievementType.BuildCount, 1);
    public static void BuildHouse() => Report(AchievementType.BuildHouse, 1);
    public static void BuildWarehouse() => Report(AchievementType.BuildWarehouse, 1);
    public static void BuildBuilderHut() => Report(AchievementType.BuildBuilderHut, 1);

    public static void UnlockBuilding() => Report(AchievementType.BuildingUnlockedCount, 1);

    // Worker
    public static void HireWorker() => Report(AchievementType.WorkerHired, 1);
    public static void UpdateTotalWorker(int total) => Report(AchievementType.WorkerTotalInCity, total);
    public static void MaxWorkerReached(int capacity) => Report(AchievementType.MaxWorkerReached, capacity);

    // Coin
    public static void EarnCoin(float amount) => Report(AchievementType.CoinEarned, Mathf.FloorToInt(amount));
    public static void SpendCoin(float amount) => Report(AchievementType.CoinSpent, Mathf.FloorToInt(amount));
    public static void CollectTax(float amount) => Report(AchievementType.TaxCollected, Mathf.FloorToInt(amount));
    public static void UpdateCurrentCoin(float current) => Report(AchievementType.CoinCurrent, Mathf.FloorToInt(current));

    // Resources
    public static void StoreResource(float total) => Report(AchievementType.ResourceStored, Mathf.FloorToInt(total));
    public static void Logwood(float amount) => Report(AchievementType.LogwoodStored, Mathf.FloorToInt(amount));
    public static void Plank(float amount) => Report(AchievementType.PlankStored, Mathf.FloorToInt(amount));
    public static void Water(float amount) => Report(AchievementType.WaterStored, Mathf.FloorToInt(amount));
    public static void Stone(float amount) => Report(AchievementType.StoneStored, Mathf.FloorToInt(amount));
    public static void Grain(float amount) => Report(AchievementType.GrainStored, Mathf.FloorToInt(amount));
    public static void Flour(float amount) => Report(AchievementType.FlourStored, Mathf.FloorToInt(amount));

    // Progress
    public static void ReachLevel(int level) => Report(AchievementType.LevelReached, level);
    public static void PlayTime(float deltaTime) => Report(AchievementType.PlayTimeTotal, Mathf.FloorToInt(deltaTime));

    private static void Report(AchievementType type, int amount)
    {
        if (AchievementManager.Instance == null) return;

        if (AchievementProgress.IsCumulativeType(type))
            AchievementManager.Instance.ReportProgress(type, amount);
        else
        {
            var current = AchievementManager.Instance.GetCurrentProgress(type);
            if (amount > current)
                AchievementManager.Instance.ReportProgress(type, amount);
        }
    }
} 
