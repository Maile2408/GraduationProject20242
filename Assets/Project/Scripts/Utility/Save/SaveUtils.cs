using UnityEngine;

public static class SaveUtils
{
    public static string GetPrefabName(GameObject go)
    {
        if (go == null) return string.Empty;
        return go.name.Replace("(Clone)", "").Trim();
    }

    public static void AssignID(GameObject go)
    {
        var iden = go.GetComponent<Identifiable>();
        if (iden != null && string.IsNullOrEmpty(iden.id))
        {
            iden.id = System.Guid.NewGuid().ToString();
        }
    }
}
