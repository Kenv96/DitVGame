using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMusicTrigger : MonoBehaviour
{
    public AudioClip music;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.GetInstance().ChangeSong(music);
        }
    }
}
