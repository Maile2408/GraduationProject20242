using System;

[Serializable]
public class Worker
{
    public string WorkerID;
    public string Name;
    public string HomeID;

    public Worker(string workerID, string name, string homeID)
    {
        this.WorkerID = workerID;
        this.Name = name;
        this.HomeID = homeID;
    }
}
