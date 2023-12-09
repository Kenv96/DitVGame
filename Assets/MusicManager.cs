using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    public AudioSource[] source;

    private bool quieting;
    private int tracktoquiet;
    private int lastTrack = 1;
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

    // Use this method to get the singleton instance
    public static MusicManager GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        if (quieting)
        {
            source[tracktoquiet].volume -= 0.005f;
            if (source[tracktoquiet].volume < 0.1)
            {
                source[tracktoquiet].clip = null;
                source[tracktoquiet].volume = 0.5f;
                quieting = false;
                tracktoquiet = 0;
            }
        }
    }

    public void Quiet(int track)
    {
        tracktoquiet = track;
        quieting = true;
        lastTrack = track;
    }

    public void ChangeSong(AudioClip song)
    {
        if (source[1])
        {
            Quiet(1);
            source[2].clip= song;
            source[2].Play();
        }
        else if (source[2])
        {
            Quiet(2);
            source[1].clip= song;
            source[1].Play();
        }
        else
        {
            
        }
    }
}
