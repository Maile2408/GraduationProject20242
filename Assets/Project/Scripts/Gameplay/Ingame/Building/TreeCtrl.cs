using UnityEngine;

public class TreeCtrl : SaiBehaviour
{
    public ResGenerator resGenerator;
    public TreeLevel treeLevel;
    public WorkerCtrl choper;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTreeLevel();
        this.LoadResGenerator();
    }

    protected virtual void LoadTreeLevel()
    {
        if (this.treeLevel != null) return;
        this.treeLevel = GetComponent<TreeLevel>();
        Debug.Log(transform.name + " LoadTreeLevel", gameObject);
    }

    protected virtual void LoadResGenerator()
    {
        if (this.resGenerator != null) return;
        this.resGenerator = GetComponent<ResGenerator>();
        Debug.Log(transform.name + " LoadResGenerator", gameObject);
    }
}
