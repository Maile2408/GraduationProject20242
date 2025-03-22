using System;
using System.Collections.Generic;
using System.ComponentModel;

[Serializable]
public class City
{
    public string cityID;
    public string cityName;
    public string cityLevel;
    public List<Building> buildings;
    public List<Worker> workers;
}
