using UnityEngine;

public static class AchievementReporter
{
    // Build
    public static void FirstBuild() => Report(AchievementType.FirstBuild, 1);
    public static void BuildStructure() => Report(AchievementType.BuildCount, 1);
    public static void BuildSpecial() => Report(AchievementType.SpecialBuildingBuilt, 1);
    public static void BuildHouse() => Report(AchievementType.SpecialBuildingBuilt, 1);
    public static void UnlockBuildingCount(int count) => Report(AchievementType.BuildingUnlockedCount, count);

    // Worker
    public static void HireWorker() => Report(AchievementType.WorkerHired, 1);
    public static void AssignWorkerToHouse() => Report(AchievementType.WorkerHired, 1); // if separate from hire logic
    public static void UpdateTotalWorker(int total) => Report(AchievementType.WorkerTotalInCity, total);
    public static void MaxWorkerReached(int capacity) => Report(AchievementType.MaxWorkerReached, capacity);

    // Coin
    public static void EarnCoin(int amount) => Report(AchievementType.CoinEarned, amount);
    public static void SpendCoin(int amount) => Report(AchievementType.CoinSpent, amount);
    public static void CollectTax(int amount) => Report(AchievementType.TaxCollected, amount);
    public static void UpdateCurrentCoin(int current) => Report(AchievementType.CoinCurrent, current);

    // Resource
    public static void StoreResource(int total) => Report(AchievementType.ResourceStored, total);
    public static void StoreSpecificResource(AchievementType resourceType, int amount) => Report(resourceType, amount);

    // Progress
    public static void ReachLevel(int level) => Report(AchievementType.LevelReached, level);
    public static void PlayTime(float deltaTime) => Report(AchievementType.PlayTimeTotal, Mathf.FloorToInt(deltaTime));

    // Helper
    private static void Report(AchievementType type, int amount)
    {
        if (AchievementManager.Instance != null)
            AchievementManager.Instance.ReportProgress(type, amount);
    }
}
