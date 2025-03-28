using System.Collections.Generic;

public class FarmWH : Warehouse
{
    public override ResHolder ResNeed2Move()
    {
        ResHolder resHolder = this.GetResource(ResourceName.grain);
        if (resHolder.Current() > 0) return resHolder;
        return null;
    }

    public override List<Resource> NeedResoures()
    {
        List<Resource> resources = new List<Resource>();

        ResHolder water = this.GetResource(ResourceName.water);
        Resource resWater = new Resource
        {
            name = water.Name(),
            number = water.Max() - water.Current()
        };

        if (resWater.number > 0) resources.Add(resWater);

        return resources;
    }
}
