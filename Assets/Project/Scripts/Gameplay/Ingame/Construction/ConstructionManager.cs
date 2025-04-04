using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : SaiBehaviour
{
    public static ConstructionManager Instance { get; private set; }

    [Header("All Constructions")]
    [SerializeField] protected List<AbstractConstruction> constructions = new();

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple ConstructionManager in scene");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public virtual void AddConstruction(AbstractConstruction construction)
    {
        if (!this.constructions.Contains(construction))
        {
            this.constructions.Add(construction);
        }
    }

    public virtual void RemoveConstruction(AbstractConstruction construction)
    {
        this.constructions.Remove(construction);
    }

    public virtual AbstractConstruction GetConstruction()
    {
        foreach (var construction in this.constructions)
        {
            if (construction == null) continue;
            if (!construction.isBuilding && !construction.isReadyToBuild)
                return construction;
        }

        return null;
    }

    public virtual List<AbstractConstruction> GetAll()
    {
        return constructions;
    }
}
