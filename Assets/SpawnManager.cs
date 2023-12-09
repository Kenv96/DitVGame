using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawns;
    private static SpawnManager instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
            return;
        }

        // If no instance exists, set this as the instance
        instance = this;

        // Make sure the GameObject doesn't get destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);
    }

    // Use this method to get the singleton instance
    public static SpawnManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        foreach(GameObject p in spawns)
        {
            if(p != null) p.GetComponent<EnemySpawner>().Spawn();
        }
    }
}
