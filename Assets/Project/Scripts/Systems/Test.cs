#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Cheat Settings")]
    public int addXP = 300;
    public int addCoin = 500;
    public int addWood = 100;
    public int addStone = 100;
    public int addPlank = 100;

#if UNITY_EDITOR
    [CustomEditor(typeof(Test))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Test cheat = (Test)target;

            GUILayout.Space(10);
            EditorGUILayout.LabelField("=== Cheat Actions ===", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Coin"))
            {
                CurrencyManager.Instance?.AddCoin(cheat.addCoin);
            }

            if (GUILayout.Button("Add XP"))
            {
                CityLevelManager.Instance?.AddXP(cheat.addXP);
            }

            if (GUILayout.Button("Add Resources (to WarehouseWH)"))
            {
                StorageResourceManager.Instance?.AddResourceToAnyWarehouse(ResourceName.logwood, cheat.addWood);
                StorageResourceManager.Instance?.AddResourceToAnyWarehouse(ResourceName.stone, cheat.addStone);
                StorageResourceManager.Instance?.AddResourceToAnyWarehouse(ResourceName.plank, cheat.addPlank);
            }

            if (GUILayout.Button("Force Level Up"))
            {
                CityLevelManager.Instance?.ForceLevelUp();
            }
        }
    }
#endif
}
