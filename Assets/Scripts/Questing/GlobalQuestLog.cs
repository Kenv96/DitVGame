using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalQuestLog : MonoBehaviour
{
    private static GlobalQuestLog instance;

    public List<QuestScriptObj> GQL = new List<QuestScriptObj>();

    // Make sure this script is attached to a GameObject in the scene.
    // This method is called automatically when the script starts.
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

    private void Start()
    {
        //if (ES3.KeyExists("globalQuestLog")) ES3.LoadInto("globalQuestLog", GQL);
    }

    // Use this method to get the singleton instance
    public static GlobalQuestLog GetInstance()
    {
        return instance;
    }
}
