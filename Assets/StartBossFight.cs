using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    public GameObject boss;
    public GameObject fight;
    public GameObject gate;
    public AudioClip lichBossMusic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gate.SetActive(true);
            MusicManager.GetInstance().ChangeSong(lichBossMusic);
            boss.SetActive(false);
            fight.SetActive(true);
            fight.GetComponent<LichBossFight>().StartFight();
            gameObject.SetActive(false);
        }
    }
}
