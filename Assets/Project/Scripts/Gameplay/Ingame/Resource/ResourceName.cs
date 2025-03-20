using System;

public class ResNameParser
{
    public static ResourceName FromString(string name)
    {
        return (ResourceName)Enum.Parse(typeof(ResourceName), name);
    }
}

public enum ResourceName
{
    noResource = 0,

    // Currency
    gold = 1,       
    diamond = 2,    

    // Raw materials level 1 
    water = 1000,
    logwood = 1001,
    stone = 1002,
    ironOre = 1003,
    grain = 1004,

    // Raw materials level 2 
    plank = 2001, 
    brick = 2002, 
    ironIngot = 2003,
    flour = 2004, 

    // Raw materials level 2 
    bread = 3001, 
    weapon = 3002, 
}
