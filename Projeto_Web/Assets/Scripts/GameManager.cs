﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager singleton;

    public AudioSource music;
    public AudioClip[] musicClips;
    public AudioSource[] sounds;
    public float soundVolume = 1;

    // Start is called before the first frame update
    void Awake()
    {
        print("Awake");

        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        music = GetComponent<AudioSource>();
        sounds = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        if(singleton == this)
            MusicChange(SceneManager.GetActiveScene().buildIndex);

        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (singleton == this)
        {
            MusicChange(level);
            StartCoroutine(WaitForStart());
        }
    }

    IEnumerator WaitForStart()
    {
        yield return null;
        ChangeSoundVolume();
    }

    public void ChangeSoundVolume()
    {
        sounds = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        if (sounds.Length > 1)
        {
            foreach (AudioSource aS in sounds)
            {
                if (aS != music)
                    aS.volume = soundVolume;
            }
        }
    }

    public void MusicChange(int level)
    {
        if (level != 0 && music.clip != musicClips[1])
        {
            music.clip = musicClips[1];
            music.Play();
        }
        else if (level == 0 && music.clip != musicClips[0])
        {
            music.clip = musicClips[0];
            music.Play();
        }
    }
}
