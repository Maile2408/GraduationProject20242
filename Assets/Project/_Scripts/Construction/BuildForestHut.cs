using UnityEngine;

public class BuildForestHut : BuildBuilding
{
    protected override void LoadResRequires()
    {
        if (this.resRequires.Count > 0) return;
        this.resRequires.Add(new Resource { name = ResourceName.logwood, amount = 2 });
        Debug.Log(transform.name + ": LoadResRequires", gameObject);
    }
}
