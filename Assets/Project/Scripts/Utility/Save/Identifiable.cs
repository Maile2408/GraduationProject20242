using UnityEngine;
using System;

[DisallowMultipleComponent]
public class Identifiable : MonoBehaviour
{
    public string id;

#if UNITY_EDITOR
    [ContextMenu("Generate New ID")]
    private void GenerateID()
    {
        id = Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"Generated ID for {name}: {id}");
    }
#endif

    private void Awake()
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning($"Object '{name}' missing ID");
        }
    }
}
