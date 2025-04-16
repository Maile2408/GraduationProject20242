public class AchievementProgress
{
    public AchievementData data;
    public int current;
    public bool isCompleted;
    public bool isRewardClaimed;

    public AchievementProgress(AchievementData data)
    {
        this.data = data;
        current = 0;
        isCompleted = false;
        isRewardClaimed = false;
    }

    public bool AddProgress(int value)
    {
        if (isCompleted) return false;
        current += value;
        if (current >= data.goalAmount)
        {
            current = data.goalAmount;
            isCompleted = true;
            return true;
        }
        return false;
    }

    public bool SetProgress(int value)
    {
        current = value;

        if (current >= data.goalAmount)
        {
            current = data.goalAmount;
            isCompleted = true;
            return true;
        }

        return false;
    }

    public static bool IsCumulativeType(AchievementType type)
    {
        switch (type)
        {
            case AchievementType.FirstBuild:
            case AchievementType.BuildCount:
            case AchievementType.BuildHouse:
            case AchievementType.BuildWarehouse:
            case AchievementType.BuildBuilderHut:
            case AchievementType.WorkerHired:
            case AchievementType.CoinEarned:
            case AchievementType.CoinSpent:
            case AchievementType.TaxCollected:
            case AchievementType.BuildingUnlockedCount:
            case AchievementType.PlayTimeTotal:
                return true;

            case AchievementType.WorkerTotalInCity:
            case AchievementType.MaxWorkerReached:
            case AchievementType.CoinCurrent:
            case AchievementType.ResourceStored:
            case AchievementType.LogwoodStored:
            case AchievementType.PlankStored:
            case AchievementType.StoneStored:
            case AchievementType.WaterStored:
            case AchievementType.GrainStored:
            case AchievementType.FlourStored:
            case AchievementType.LevelReached:
                return false;

            default:
                return true;
        }
    }
} 
