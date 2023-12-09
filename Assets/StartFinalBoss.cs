using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinalBoss : MonoBehaviour
{
    public AudioClip BossMusic;
    public GameObject playerAggro;
    public FinalBossMonster boss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerAggro.SetActive(false);
            MusicManager.GetInstance().ChangeSong(BossMusic);
            boss.StartFight();
        }
    }
}
