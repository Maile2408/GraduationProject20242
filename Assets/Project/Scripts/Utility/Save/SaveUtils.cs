using UnityEngine;

public static class SaveUtils
{
    public static string GetPrefabName(GameObject go)
    {
        if (go == null) return string.Empty;
        return go.name.Replace("(Clone)", "").Trim();
    }

    public static void AssignID(GameObject go, IDType type)
    {
        var iden = go.GetComponent<Identifiable>();
        if (iden != null)
        {
            iden.SetID(IDGenerator.GenerateID(type));
        }
    }
}
