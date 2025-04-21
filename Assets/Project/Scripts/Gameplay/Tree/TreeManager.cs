using System.Collections.Generic;
using UnityEngine;

public class TreeManager : SaiBehaviour
{
    public static TreeManager Instance;

    [SerializeField] protected List<TreeCtrl> trees = new();

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null) Debug.LogError("Only 1 TreeManager allowed");
        Instance = this;
    }

    public virtual void LoadTrees()
    {
        this.trees.Clear();
        foreach (Transform child in transform)
        {
            TreeCtrl tree = child.GetComponent<TreeCtrl>();
            if (tree == null) continue;
            this.trees.Add(tree);
        }
    }

    public virtual void TreeAdd(TreeCtrl tree)
    {
        if (this.trees.Contains(tree)) return;
        this.trees.Add(tree);
        tree.transform.parent = transform;
    }

    public virtual bool TreeRemove(TreeCtrl tree)
    {
        return this.trees.Remove(tree);
    }

    public virtual List<TreeCtrl> Trees()
    {
        return this.trees;
    }
} 
