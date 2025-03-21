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
}
