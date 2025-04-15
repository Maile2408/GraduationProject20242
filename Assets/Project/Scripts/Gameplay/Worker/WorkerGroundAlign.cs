using UnityEngine;

[RequireComponent(typeof(WorkerCtrl))]
public class WorkerGroundAlign : MonoBehaviour
{
    [SerializeField] float rayHeight = 1.0f;
    [SerializeField] float alignSpeed = 10f;
    [SerializeField] Transform modelTransform;

    private static readonly int groundLayerMask = 1 << 7;

    private void LateUpdate()
    {
        AlignToGround();
    }

    protected virtual void AlignToGround()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeight;
        Ray ray = new Ray(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayHeight * 2f, groundLayerMask))
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, hitInfo.point.y, pos.z);

            Quaternion targetRot = Quaternion.FromToRotation(modelTransform.up, hitInfo.normal) * modelTransform.rotation;
            modelTransform.rotation = Quaternion.Lerp(modelTransform.rotation, targetRot, Time.deltaTime * alignSpeed);
        }
    }

    public void SetModelTransform(Transform model)
    {
        this.modelTransform = model;
    }
}
