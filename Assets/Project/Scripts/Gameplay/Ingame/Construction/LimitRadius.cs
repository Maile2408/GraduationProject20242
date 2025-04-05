using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class LimitRadius : SaiBehaviour
{
    [Header("Visual & Highlight")]
    public MeshRenderer highlightRenderer;

    [Header("Materials")]
    [SerializeField] private Material canPlaceMat;
    [SerializeField] private Material cannotPlaceMat;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask detectionMask = default;
    [SerializeField] private Vector3 padding = new(0.5f, 0f, 0.5f);
    [SerializeField] private float highlightYScale = 0.5f;

    [Header("Runtime")]
    [SerializeField] private List<GameObject> collideObjects = new();

    private BoxCollider boxCollider;
    private Rigidbody rb;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCollider();
        LoadRigidbody();
    }

    protected virtual void LoadCollider()
    {
        if (boxCollider != null) return;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = Vector3.zero;
    }

    protected virtual void LoadRigidbody()
    {
        if (rb != null) return;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldIgnore(other)) return;

        if (!collideObjects.Contains(other.gameObject))
        {
            collideObjects.Add(other.gameObject);
            UpdateHighlight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collideObjects.Remove(other.gameObject))
        {
            UpdateHighlight();
        }
    }

    protected virtual bool ShouldIgnore(Collider other)
    {
        if (other == null) return true;
        if (other.isTrigger) return true;
        if (other.gameObject == this.gameObject) return true;

        int otherLayer = other.gameObject.layer;
        return (detectionMask & (1 << otherLayer)) == 0;
    }

    public bool IsCollided() => collideObjects.Count > 0;

    public void ClearColliders()
    {
        collideObjects.Clear();
        UpdateHighlight();
    }

    public void ResizeToBounds(Vector3 targetSize)
    {
        Vector3 finalSize = targetSize + padding;

        // Resize BoxCollider
        if (boxCollider != null)
        {
            boxCollider.size = finalSize;
        }

        // Resize highlight
        if (highlightRenderer != null)
        {
            MeshFilter mf = highlightRenderer.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
            {
                Vector3 meshSize = mf.sharedMesh.bounds.size;
                meshSize.x = Mathf.Approximately(meshSize.x, 0f) ? 1f : meshSize.x;
                meshSize.z = Mathf.Approximately(meshSize.z, 0f) ? 1f : meshSize.z;

                Vector3 newScale = new Vector3(
                    finalSize.x / meshSize.x,
                    highlightYScale,
                    finalSize.z / meshSize.z
                );

                highlightRenderer.transform.localScale = newScale;
            }
        }

        UpdateHighlight();
    }

    protected virtual void UpdateHighlight()
    {
        if (highlightRenderer == null || canPlaceMat == null || cannotPlaceMat == null) return;

        Material mat = IsCollided() ? cannotPlaceMat : canPlaceMat;
        highlightRenderer.material = mat;
    }
}
