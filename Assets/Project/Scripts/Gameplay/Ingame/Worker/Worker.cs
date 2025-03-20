using System;

[Serializable]
public class Worker
{
    public string WorkerID { get; private set; }
    public string Name { get; private set; }
    public string HomeID { get; private set; }

    public Worker(string workerID, string name, string homeID)
    {
        this.WorkerID = workerID;
        this.Name = name;
        this.HomeID = homeID;
    }
}
