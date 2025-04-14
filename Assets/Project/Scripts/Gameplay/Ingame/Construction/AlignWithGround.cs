using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class AlignWithGround : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float raycastHeight = 5f;
    [SerializeField] private float raycastDistance = 10f;
    [SerializeField] private float maxAllowedSlope = 45f; 

    public void Align()
    {
        Collider col = GetComponent<Collider>();
        Bounds bounds = col.bounds;

        float halfX = bounds.size.x / 2f;
        float halfZ = bounds.size.z / 2f;

        Vector3[] localOffsets = new Vector3[]
        {
            new Vector3(-halfX, 0, -halfZ), 
            new Vector3( halfX, 0, -halfZ), 
            new Vector3(    0f, 0,  halfZ), 
        };

        List<Vector3> hitPoints = new();

        foreach (var offset in localOffsets)
        {
            Vector3 origin = transform.position + offset + Vector3.up * raycastHeight;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance, groundMask))
            {
                hitPoints.Add(hit.point);
            }
        }

        if (hitPoints.Count < 3) return;

        Vector3 a = hitPoints[0];
        Vector3 b = hitPoints[1];
        Vector3 c = hitPoints[2];

        Vector3 normal = Vector3.Cross(b - a, c - a).normalized;

        float angle = Vector3.Angle(normal, Vector3.up);
        if (angle > maxAllowedSlope) return;

        Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, normal);
        transform.rotation = targetRot;

        float averageY = (a.y + b.y + c.y) / 3f;
        transform.position = new Vector3(transform.position.x, averageY, transform.position.z);
    }
}
