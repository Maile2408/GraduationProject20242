using System;

public enum IDType
{
    Worker,
    Tree,
    Construction,
    Building
}

public static class IDGenerator
{
    public static string GenerateID(IDType type)
    {
        string prefix = type switch
        {
            IDType.Worker => "worker_",
            IDType.Tree => "tree_",
            IDType.Construction => "construction_",
            IDType.Building => "building_",
            _ => "x_"
        };

        return prefix + Guid.NewGuid().ToString("N").Substring(0, 8);
    }
}
