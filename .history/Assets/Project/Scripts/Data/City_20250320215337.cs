using System;
using System.Collections.Generic;
using UnityEngine;

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

    public void UpgradeCity()
    {
        Level++;
        Debug.Log($"City {CityName} upgraded to level {Level}");
    }

    public void AddBuilding(Building building) => Buildings.Add(building);
    public void RemoveBuilding(string buildingID) => Buildings.RemoveAll(b => b.BuildingID == buildingID);
    public Building GetBuilding(string buildingID) => Buildings.Find(b => b.BuildingID == buildingID);

    public void AddWorker(Worker worker) => Workers.Add(worker);
    public void RemoveWorker(string workerID) => Workers.RemoveAll(w => w.WorkerID == workerID);

    public void AddResource(Resource resource)
    {
        Resource existing = Resources.Find(r => r.Name == resource.Name);
        if (existing != null) existing.Amount += resource.Amount;
        else Resources.Add(resource);
    }

    public bool UseResource(Resource resource)
    {
        Resource existing = Resources.Find(r => r.Name == resource.Name);
        if (existing != null && existing.Amount >= resource.Amount)
        {
            existing.Amount -= resource.Amount;
            return true;
        }
        return false;
    }
}
