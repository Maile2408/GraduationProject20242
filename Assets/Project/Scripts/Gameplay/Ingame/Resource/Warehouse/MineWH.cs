public class MineWH : Warehouse
{
    public override ResHolder ResNeed2Move()
    {
        ResHolder resHolder = this.GetResource(ResourceName.stone);
        if (resHolder.Current() > 0) return resHolder;
        return null;
    }
}
