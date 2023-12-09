using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //Data to Save
    public GameObject LichBoss;
    public GameObject LichBossTalk;
    public GameObject NecroBossTalk;
    public GameObject NecroBoss;


    private static SaveManager instance;
    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static SaveManager GetInstance()
    {
        return instance;
    }

    public void Save()
    {
        ES3.Save("playerTransform", PlayerMove.GetInstance().gameObject.transform);
        //ES3.Save("globalQuestLog", GlobalQuestLog.GetInstance().GQL);
        //ES3.Save("necroBossTalk", NecroBossTalk.activeSelf);
        //ES3.Save("necroBoss", NecroBoss.activeSelf);
        Debug.Log("Saved");
    }

    public void Load()
    {
        if (ES3.KeyExists("playerTransform")) ES3.LoadInto("playerTransform", PlayerMove.GetInstance().gameObject.transform);
        Debug.Log("Loaded");
    }
}
