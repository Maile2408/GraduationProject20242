using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Progression/Achievement Data")]
public class AchievementData : ScriptableObject
{
    public string id;
    public string title;
    [TextArea] public string description;
    public AchievementType type;
    public int goalAmount;
    public int rewardXP;
    public int rewardCoin;

    public Sprite icon;
}
