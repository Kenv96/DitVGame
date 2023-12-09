using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepAtInn : MonoBehaviour
{
    public AudioClip[] music;
    public GameObject darkenScreen;
    public void SleepyTime()
    {
        //SleepTest();
        StartCoroutine(Sleep());
    }

    IEnumerator Sleep()
    {
        PlayerMove.GetInstance().control = false;
        MusicManager.GetInstance().ChangeSong(music[0]);
        darkenScreen.GetComponent<Animator>().Play("ScreenFadeOUT");
        yield return new WaitForSeconds(5);
        SpawnManager.GetInstance().Spawn();
        PlayerMove.GetInstance().control = true;
        yield return new WaitForSeconds(3);
        MusicManager.GetInstance().ChangeSong(music[1]);
    }

    private void SleepTest()
    {
        SpawnManager.GetInstance().Spawn();
        Debug.Log("Respawning enemies");
    }
}
