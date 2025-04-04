using UnityEngine;

public class LimitRadius : SaiBehaviour
{
    [Header("Visual & Detection")]
    public MeshRenderer highlightRenderer;

    [Header("Materials")]
    [SerializeField] private Material canPlaceMat;
    [SerializeField] private Material cannotPlaceMat;

    [Header("Detection Settings")]
    [SerializeField] private Vector3 boxSize = Vector3.one;
    [SerializeField] private Vector3 boxOffset = Vector3.zero;
    [SerializeField] private Vector3 extraSize = new(0.1f, 1f, 0.1f); 
    [SerializeField] private LayerMask collisionMask = ~0;

    [Header("Highlight Scale")]
    [SerializeField] private float highlightYScale = 0.5f;

    [SerializeField] private bool isCollided = false;

    protected override void Update()
    {
        this.CheckOverlap();
        this.UpdateHighlight();
    }

    protected virtual void CheckOverlap()
    {
        Vector3 center = transform.position + boxOffset;
        Vector3 halfExtents = (boxSize + extraSize) * 0.5f;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation, collisionMask);
        int validHits = 0;

        foreach (var col in hits)
        {
            if (col.gameObject == this.gameObject) continue;
            if (col.isTrigger) continue;

            validHits++;
        }

        isCollided = validHits > 0;
    }

    public bool IsCollided()
    {
        return isCollided;
    }

    public void ResizeToBounds(Vector3 targetSize)
    {
        boxSize = targetSize;

        if (highlightRenderer != null)
        {
            MeshFilter mf = highlightRenderer.GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
            {
                Vector3 meshSize = mf.sharedMesh.bounds.size;
                meshSize.x = Mathf.Approximately(meshSize.x, 0) ? 1f : meshSize.x;
                meshSize.z = Mathf.Approximately(meshSize.z, 0) ? 1f : meshSize.z;

                Vector3 newScale = new(
                    targetSize.x / meshSize.x,
                    highlightYScale,
                    targetSize.z / meshSize.z
                );

                highlightRenderer.transform.localScale = newScale;
            }
        }
    }

    protected virtual void UpdateHighlight()
    {
        if (highlightRenderer == null || canPlaceMat == null || cannotPlaceMat == null) return;

        Material mat = isCollided ? cannotPlaceMat : canPlaceMat;
        highlightRenderer.material = mat;
    }
}