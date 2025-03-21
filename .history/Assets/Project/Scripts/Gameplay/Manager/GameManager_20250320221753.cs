using System.Collections.Generic;
using UnityEngine;

public class GameManager : SaiBehaviour
{
    public static GameManager Instance;

    private User user;
    private City city;
    private List<Worker> workers;
    private List<Resource> resources;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        Debug.Log("GameManager: Loading game components...");
        // Load dữ liệu game nếu cần
    }

    public void SetUser(User newUser) => user = newUser;
    public void SetCity(City newCity) => city = newCity;
    public void SetWorkers(List<Worker> newWorkers) => workers = newWorkers;
    public void SetResources(List<Resource> newResources) => resources = newResources;

    public User GetUser() => user;
    public City GetCity() => city;
    public List<Worker> GetWorkers() => workers;
    public List<Resource> GetResources() => resources;
}
