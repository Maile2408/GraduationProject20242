using System.Collections.Generic;

public class MillWH : Warehouse
{
    public override ResHolder ResNeed2Move()
    {
        ResHolder resHolder = this.GetResource(ResourceName.flour);
        if (resHolder.Current() > 0) return resHolder;
        return null;
    }

    public override List<Resource> NeedResoures()
    {
        List<Resource> resources = new List<Resource>();

        ResHolder grain = this.GetResource(ResourceName.grain);
        Resource resGrain = new Resource
        {
            name = grain.Name(),
            number = grain.Max() - grain.Current()
        };

        if (resGrain.number > 0) resources.Add(resGrain);

        return resources;
    }
}
