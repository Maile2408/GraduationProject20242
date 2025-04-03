using UnityEngine;

public class AlignWithGround : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float raycastDistance = 10f;

    public void Align()
    {
        Vector3 start = transform.position + Vector3.up * 5f;

        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, raycastDistance, groundMask))
        {
            transform.position = hit.point;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}
