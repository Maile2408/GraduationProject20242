using System.Collections.Generic;
using UnityEngine;

public class LimitRadius : SaiBehaviour
{
    [Header("Limit Radius Settings")]
    [SerializeField] protected Collider detectionCollider;

    [SerializeField] public List<GameObject> collideObjects = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDetectionCollider();
    }

    protected virtual void LoadDetectionCollider()
    {
        if (this.detectionCollider != null) return;
        this.detectionCollider = GetComponent<Collider>();
        if (this.detectionCollider != null)
            this.detectionCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.collideObjects.Contains(other.gameObject)) return;

        if (other.isTrigger || other.gameObject == this.gameObject) return;

        this.collideObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.collideObjects.Contains(other.gameObject))
        {
            this.collideObjects.Remove(other.gameObject);
        }
    }

    public virtual bool IsCollided()
    {
        return this.collideObjects.Count > 0;
    }

    public virtual void ClearColliders()
    {
        this.collideObjects.Clear();
    }
}
