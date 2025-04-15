public class AchievementProgress
{
    public AchievementData data;
    public int current;
    public bool isCompleted;

    public AchievementProgress(AchievementData data)
    {
        this.data = data;
        current = 0;
        isCompleted = false;
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
}
