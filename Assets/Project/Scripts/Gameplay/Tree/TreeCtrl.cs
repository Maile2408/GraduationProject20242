using UnityEngine;

public class TreeCtrl : SaiBehaviour, IPoolable, ISaveable<TreeSaveData>
{
    public string treeType;
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
        //Debug.Log(transform.name + " LoadTreeLevel", gameObject);
    }

    protected virtual void LoadResGenerator()
    {
        if (this.logwoodGenerator != null) return;
        this.logwoodGenerator = GetComponent<LogwoodGenerator>();
        //Debug.Log(transform.name + " LoadLogwoodGenerator", gameObject);
    }

    public void OnSpawn() { }

    public void OnDespawn()
    {
        choper = null;
        this.treeLevel.ResetTreeLevel();
        this.logwoodGenerator.ResetRes();
    }

    // ===================== SAVE ======================
    public TreeSaveData Save()
    {
        return new TreeSaveData
        {
            id = GetComponent<Identifiable>().ID,
            type = this.treeType,
            position = transform.position,
            rotation = transform.rotation,
            currentLevel = this.treeLevel.CurrentLevel,
            treeTimer = this.treeLevel.TreeTimer,
            generatorTimer = this.logwoodGenerator.CreateTimer
        };
    }

    // ===================== LOAD ======================
    public void LoadFromSave(TreeSaveData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;

        this.treeLevel.CurrentLevel = data.currentLevel;
        this.treeLevel.TreeTimer = data.treeTimer;
        this.logwoodGenerator.CreateTimer = data.generatorTimer;
    }
} 
