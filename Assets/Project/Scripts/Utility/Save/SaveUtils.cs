using UnityEngine;

public static class SaveUtils
{
    public static void AssignID(GameObject go, IDType type)
    {
        var iden = go.GetComponent<Identifiable>();
        if (iden != null)
        {
            iden.SetID(IDGenerator.GenerateID(type));
        }
    }
}
