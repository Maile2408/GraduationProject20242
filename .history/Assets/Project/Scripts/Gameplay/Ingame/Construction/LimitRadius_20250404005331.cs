using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LimitRadius : SaiBehaviour
{
    [Header("Visual & Detection")]
    [SerializeField] protected Collider detectionCollider;
    [SerializeField] protected MeshRenderer highlightRenderer;

    [Header("Materials")]
    [SerializeField] protected Material canPlaceMat;
    [SerializeField] protected Material cannotPlaceMat;

    [SerializeField] public List<GameObject> collideObjects = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDetectionCollider();
        this.LoadHighlightRenderer();
    }

    protected virtual void LoadDetectionCollider()
    {
        if (this.detectionCollider != null) return;
        this.detectionCollider = GetComponent<Collider>();
        if (this.detectionCollider != null)
        {
            this.detectionCollider.isTrigger = true;
        }
    }

    protected virtual void LoadHighlightRenderer()
    {
        if (this.highlightRenderer != null) return;
        this.highlightRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.ShouldIgnore(other)) return;
        this.collideObjects.Add(other.gameObject);
        this.UpdateHighlight();
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.collideObjects.Contains(other.gameObject))
        {
            this.collideObjects.Remove(other.gameObject);
            this.UpdateHighlight();
        }
    }

    protected virtual bool ShouldIgnore(Collider other)
    {
        return other.isTrigger || other.gameObject == this.gameObject;
    }

    public virtual bool IsCollided()
    {
        return this.collideObjects.Count > 0;
    }

    public virtual void ClearColliders()
    {
        this.collideObjects.Clear();
        this.UpdateHighlight();
    }

    public virtual void UpdateHighlight()
    {
        if (this.highlightRenderer == null || canPlaceMat == null || cannotPlaceMat == null) return;

        Material mat = this.IsCollided() ? cannotPlaceMat : canPlaceMat;
        this.highlightRenderer.material = mat;
    }

    public virtual void ResizeToBounds(Vector3 size)
    {
        if (this.detectionCollider is BoxCollider box)
        {
            box.size = size;
        }

        if (this.highlightRenderer != null)
        {
            this.highlightRenderer.transform.localScale = size;
        }
    }
}
