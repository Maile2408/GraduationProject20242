using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/City Level Config")]
public class CityLevelConfig : ScriptableObject
{
    public List<CityLevelManager.LevelData> levelConfigs;
}
