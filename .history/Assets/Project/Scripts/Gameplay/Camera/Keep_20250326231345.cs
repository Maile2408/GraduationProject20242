using UnityEngine;
using UnityEngine.AI;

public class KeepOnNavMesh : MonoBehaviour
{
    private Vector3 lastSafePosition;

    void Start()
    {
        lastSafePosition = transform.position;
    }

    void Update()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            lastSafePosition = transform.position;
        }
        else
        {
            Debug.Log("Out of bounds! Resetting position.");
            transform.position = lastSafePosition;
        }
    }
}
