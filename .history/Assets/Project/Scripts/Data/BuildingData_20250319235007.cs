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
    public float wood { get; set; } = 0;
    public float stone { get; set; } = 0;
    public float gold { get; set; } = 0;
}

[System.Serializable]
public class UpgradeCost
{
    public float? wood { get; set; }
    public float? stone { get; set; }
    public float? gold { get; set; }
}

[System.Serializable]
public class Level
{
    public int level { get; set; }
    public Cost cost { get; set; }
    public int capacity { get; set; } = 0;
    public float currencyProduction { get; set; } = 0;
    public float productionTime { get; set; } = 0;
    public UpgradeCost upgradeCost { get; set; }
    public List<string> produces { get; set; } = new List<string>();
    public float? storageCapacity { get; set; }
    public float? transportThreshold { get; set; }
    public List<string> requires { get; set; } = new List<string>();
}

[System.Serializable]
public class BuildingData
{
    public List<Building> buildings { get; set; }
}
