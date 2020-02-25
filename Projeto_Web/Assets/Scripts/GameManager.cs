using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager singleton;

    public AudioSource music;
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

        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (singleton == this)
        {
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
            print("ChangeSound");
            foreach (AudioSource aS in sounds)
            {
                if (aS != music)
                    aS.volume = soundVolume;
            }
        }
    }
}
