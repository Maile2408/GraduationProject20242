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

            GUILayout.Space(15);
            EditorGUILayout.LabelField("=== Cheat Game Data ===", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Coin"))
                CurrencyManager.Instance?.AddCoin(cheat.addCoin);

            if (GUILayout.Button("Add XP"))
                CityLevelManager.Instance?.AddXP(cheat.addXP);

            if (GUILayout.Button("Add Resources (to WarehouseWH)"))
            {
                StorageResourceManager.Instance?.AddResourceToAnyWarehouse(ResourceName.logwood, cheat.addWood);
                StorageResourceManager.Instance?.AddResourceToAnyWarehouse(ResourceName.stone, cheat.addStone);
                StorageResourceManager.Instance?.AddResourceToAnyWarehouse(ResourceName.plank, cheat.addPlank);
            }

            if (GUILayout.Button("Force Level Up"))
                CityLevelManager.Instance?.ForceLevelUp();

            GUILayout.Space(15);
            EditorGUILayout.LabelField("=== Save / Load Tools ===", EditorStyles.boldLabel);

            if (GUILayout.Button("Save Game"))
            {
                SaveStateCollector.Instance?.SaveAll();
                Debug.Log("[Cheat] Save Complete");
            }

            if (GUILayout.Button("Upload Save to PlayFab"))
            {
                SaveManager.Instance?.SaveAndUpload();
                Debug.Log("[Cheat] Upload Save Done");
            }

            if (GUILayout.Button("Reload Save From Local"))
            {
                GameLoader.Instance?.StartCoroutine(GameLoader.Instance.LoadAllGameData());
                Debug.Log("[Cheat] Reloaded from Save");
            }

            GUILayout.Space(15);
            EditorGUILayout.LabelField("=== Reset Tools ===", EditorStyles.boldLabel);

            if (GUILayout.Button("Clear Local Save"))
            {
                SaveManager.Instance?.ClearLocalSave();
                Debug.LogWarning("[Cheat] Local save cleared");
            }
        }
    }
#endif
}
