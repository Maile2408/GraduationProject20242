using System.Collections.Generic;

public static class SaveDataFactory
{
    public static UserSaveData CreateDefaultSaveData(string playfabID, string userName, string cityName, string characterType)
    {
        return new UserSaveData
        {
            playfabID = playfabID,
            userName = userName,
            characterType = characterType,
            city = new CitySaveData
            {
                cityName = cityName,
                cityLevel = 1,
                xp = 0,
                coin = 2000f,

                timeState = "Day",
                timeCounter = 0f,

                buildings = new List<BuildingSaveData>(),
                constructions = new List<ConstructionSaveData>(),
                workers = new List<WorkerSaveData>(),
                trees = new List<TreeSaveData>(),

                achievements = new List<AchievementSaveData>()
            }
        };
    }
}
