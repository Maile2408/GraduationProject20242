using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    [SerializeField] protected List<Resource> resources;

    protected void Awake()
    {
        //if (ResourceManager.instance != null) Debug.LogError("On 1 ResourceManager allow");
        //ResourceManager.instance = this;
    }

    public virtual Resource AddResource(ResourceName resourceName, int number)
    {
        Resource res = this.GetResByName(resourceName);
        res.amount += number;
        return res;
    }

    public virtual Resource GetResByName(ResourceName resourceName)
    {
        Resource res = this.resources.Find((x) => x.name == resourceName);

        if (res == null)
        {
            res = new Resource
            {
                name = resourceName
            };

            this.resources.Add(res);
        }

        return res;
    }
}
