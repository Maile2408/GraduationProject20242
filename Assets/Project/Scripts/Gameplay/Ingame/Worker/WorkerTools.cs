using System.Collections.Generic;
using UnityEngine;

public class WorkerTools : SaiBehaviour
{
    [Header("Tool Holders")]
    [SerializeField] private Transform toolHolder;

    private GameObject currentTool;

    private static readonly Dictionary<WorkingType, string> toolMap = new()
    {
        { WorkingType.chopping, "Axe" },
        { WorkingType.building, "Hammer" },
        { WorkingType.sawing, "Saw" },

        // TODO: Extend support for additional WorkingTypes with specific tool prefabs if needed.
    };

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (toolHolder != null) return;

        toolHolder = transform.Find("ToolHolder");
        if (toolHolder == null)
        {
            Debug.LogError($"{transform.name}: ToolHolder not found!");
        }
    }

    public void UpdateTool(MovingType movingType, WorkingType? workingType)
    {
        ClearTool();

        if (workingType.HasValue)
        {
            AttachToolByWorkingType(workingType.Value);
        }
        else if (movingType == MovingType.carrying)
        {
            AttachToolByName("Tools/Box");
        }
    }

    private void AttachToolByWorkingType(WorkingType type)
    {
        if (!toolMap.TryGetValue(type, out string toolName)) return;
        AttachToolByName($"Tools/{toolName}");
    }

    private void AttachToolByName(string prefabPath)
    {
        GameObject obj = PoolManager.Instance.Spawn(prefabPath, toolHolder);
        if (obj == null) return;

        currentTool = obj;
    }

    public void ClearTool()
    {
        if (currentTool != null)
        {
            PoolManager.Instance.Despawn(currentTool);
            currentTool = null;
        }
    }

    public bool HasTool() => currentTool != null;
}
