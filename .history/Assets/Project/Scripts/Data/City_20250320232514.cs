using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class City
{
    public string CityID;
    public string CityName;
    public int Level;
    public int Currency;
    public List<Building> Buildings;
    public List<Worker> Workers;
    public List<Resource> Resources;

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
