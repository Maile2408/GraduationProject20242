using System;

[Serializable]
public class User
{
    public string UserID { get; private set; }
    public string PlayfabID { get; private set; }
    public string UserName { get; private set; }
    public string Character { get; private set; }
    public string CityID { get; private set; }

    public User(string userID, string playfabID, string userName, string character, string cityID)
    {
        this.UserID = userID;
        this.PlayfabID = playfabID;
        this.UserName = userName;
        this.Character = character;
        this.CityID = cityID;
    }
}
