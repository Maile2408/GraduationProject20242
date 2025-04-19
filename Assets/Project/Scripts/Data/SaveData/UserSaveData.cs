using System;

[Serializable]
public class UserSaveData
{
    public string playfabID;
    public string userName;
    public string characterType; // "King" or "Queen"

    public CitySaveData city; // Data GamePlay
}