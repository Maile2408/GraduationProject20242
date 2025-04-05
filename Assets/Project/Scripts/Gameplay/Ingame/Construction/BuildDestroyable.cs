using UnityEngine;

public abstract class BuildDestroyable : SaiBehaviour
{
    [Header("Destroy Settings")]
    public bool isDestructible = true;

    public virtual bool CanBeDestroyed()
    {
        return isDestructible;
    }

    public virtual void Destroy()
    {
        if (!isDestructible) return;
        PoolManager.Instance.Despawn(this.gameObject);
    }
}
