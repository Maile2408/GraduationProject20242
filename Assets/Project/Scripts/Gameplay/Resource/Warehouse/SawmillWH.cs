using System.Collections.Generic;

public class SawmillWH : Warehouse
{
    public override ResHolder ResNeed2Move()
    {
        ResHolder resHolder = this.GetResource(ResourceName.plank);
        if (resHolder.Current() > 0) return resHolder;
        return null;
    }

    public override List<Resource> NeedResoures()
    {
        List<Resource> resources = new List<Resource>();

        ResHolder logwood = this.GetResource(ResourceName.logwood);
        Resource resLogwood = new Resource
        {
            name = logwood.Name(),
            number = logwood.Max() - logwood.Current()
        };

        if(resLogwood.number > 0) resources.Add(resLogwood);

        return resources;
    }
}
