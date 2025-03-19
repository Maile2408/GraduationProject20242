using System.Collections.Generic;

[System.Serializable]
public class Building
{
    public int id { get; set; }
    public string name { get; set; }
    public string category { get; set; }
    public string description { get; set; }
    public List<Level> levels { get; set; }
    public bool unlocked { get; set; }
}

[System.Serializable]
public class Cost
{
    public int wood { get; set; } = 0;
    public int stone { get; set; } = 0;
    public int currency { get; set; } = 0;
}

[System.Serializable]
public class UpgradeCost
{
    public int? wood { get; set; }
    public int? stone { get; set; }
    public int? currency { get; set; }
}

[System.Serializable]
public class Level
{
    public int level { get; set; }
    public Cost cost { get; set; }
    public int capacity { get; set; } = 0;
    public int currencyProduction { get; set; } = 0;
    public int productionTime { get; set; } = 0;
    public UpgradeCost upgradeCost { get; set; }
    public List<string> produces { get; set; } = new List<string>();
    public int? storageCapacity { get; set; }
    public int? transportThreshold { get; set; }
    public List<string> requires { get; set; } = new List<string>();
}

[System.Serializable]
public class BuildingData
{
    public List<Building> buildings { get; set; }
}
