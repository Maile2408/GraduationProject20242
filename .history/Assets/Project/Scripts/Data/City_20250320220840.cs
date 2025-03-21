using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class City
{
    public string CityID { get; private set; }
    public string CityName { get; private set; }
    public int Level { get; private set; }
    public int Currency { get; private set; }
    public List<Building> Buildings { get; private set; }
    public List<Worker> Workers { get; private set; }
    public List<Resource> Resources { get; private set; }

    public City(string cityID, string cityName, int level, int currency)
    {
        this.CityID = cityID;
        this.CityName = cityName;
        this.Level = level;
        this.Currency = currency;
        this.Buildings = new List<Building>();
        this.Workers = new List<Worker>();
        this.Resources = new List<Resource>();
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public static City FromJson(string json)
    {
        return JsonConvert.DeserializeObject<City>(json);
    }
}
