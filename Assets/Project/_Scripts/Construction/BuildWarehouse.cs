using UnityEngine;

public class BuildWarehouse : BuildBuilding
{
    protected override void LoadResRequires()
    {
        if (this.resRequires.Count > 0) return;
        this.resRequires.Add(new Resource { name = ResourceName.logwood, amount = 2 });
        this.resRequires.Add(new Resource { name = ResourceName.blank, amount = 5 });
        Debug.Log(transform.name + ": LoadResRequires", gameObject);
    }
}
