using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public User User;
    public City City;
    public List<Worker> Workers;
    public List<Resource> Resources;

    public GameData(User user, City city, List<Worker> workers, List<Resource> resources)
    {
        this.User = user;
        this.City = city;
        this.Workers = workers;
        this.Resources = resources;
    }
}
