using UnityEngine;

public class BuildDestroyConstruction : BuildDestroyable
{
    public ConstructionCtrl constructionCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadConstructionCtrl();
    }

    protected virtual void LoadConstructionCtrl()
    {
        if (constructionCtrl != null) return;
        constructionCtrl = GetComponent<ConstructionCtrl>();
    }

    public override void Destroy()
    {
        if (!isDestructible) return;

        ConstructionManager.Instance.RemoveConstruction(constructionCtrl.abstractConstruction);
        base.Destroy();
    }
}
