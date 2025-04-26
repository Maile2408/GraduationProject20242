using UnityEngine;

public class TreeLevel : BuildLevel
{
    [SerializeField] protected bool isMaxLevel = false;
    [SerializeField] protected LogwoodGenerator tree;
    [SerializeField] protected float treeTimer = 0;
    [SerializeField] protected float treeDelay = Mathf.Infinity;

    public bool MaxLevel 
    {
        get => isMaxLevel;
        set => isMaxLevel = value;
    }

    public float TreeTimer
    {
        get => treeTimer;
        set => treeTimer = value;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(this.IsMaxLevel()) return;
        this.Growing();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTree();
    }

    protected virtual void LoadTree()
    {
        if (this.tree != null) return;
        this.tree = GetComponent<LogwoodGenerator>();
        this.GetTreeDelay();
        //Debug.Log(transform.name + ": LoadTree");
    }

    protected virtual void GetTreeDelay()
    {
        int levelCount = this.levels.Count - 2;
        this.treeDelay = this.tree.GetCreateDelay() / levelCount;
    }

    protected virtual void Growing()
    {
        this.treeTimer += Time.fixedDeltaTime;
        if (this.treeTimer < this.treeDelay) return;
        this.treeTimer = 0;

        this.ShowNextBuild();
    }

    public virtual bool IsMaxLevel()
    {
        if (this.currentLevel == this.levels.Count - 2) this.isMaxLevel = true;
        else this.isMaxLevel = false;
        return this.isMaxLevel;
    }

    public virtual void ResetTreeLevel()
    {
        HideAllBuild();
        this.currentLevel = 0;
        this.treeTimer = 0f;
        this.isMaxLevel = false;
    }
}
