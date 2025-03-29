using UnityEngine;

public class TreeCtrl : SaiBehaviour, IPoolable
{
    public LogwoodGenerator logwoodGenerator;
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
        if (this.logwoodGenerator != null) return;
        this.logwoodGenerator = GetComponent<LogwoodGenerator>();
        Debug.Log(transform.name + " LoadLogwoodGenerator", gameObject);
    }

    public void OnSpawn() { }

    public void OnDespawn()
    {
        choper = null;
        this.treeLevel.ResetTreeLevel();
        this.logwoodGenerator.ResetRes();
    }
}
