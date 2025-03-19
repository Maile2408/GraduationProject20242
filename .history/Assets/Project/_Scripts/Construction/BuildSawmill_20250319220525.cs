using UnityEngine;

public class BuildSawmill : BuildBuilding
{
    protected override void LoadResRequires()
    {
        if (this.resRequires.Count > 0) return;
        this.resRequires.Add(new Resource { name = ResourceName.logwood, amount = 3 });
        this.resRequires.Add(new Resource { name = ResourceName.blank, amount = 1 });
        Debug.Log(transform.name + ": LoadResRequires", gameObject);
    }
}
