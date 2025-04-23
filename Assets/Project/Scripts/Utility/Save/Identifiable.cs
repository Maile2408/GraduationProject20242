using UnityEngine;
using System;

[DisallowMultipleComponent]
public class Identifiable : MonoBehaviour
{
    [SerializeField] private string id;
    public string ID => id;

    public void SetID(string newID)
    {
        if (!string.IsNullOrEmpty(id)) return;
        id = newID;
    }

#if UNITY_EDITOR
    [ContextMenu("Generate New ID")]
    private void GenerateID()
    {
        id = Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"Generated ID for {name}: {id}");
    }
#endif
}
