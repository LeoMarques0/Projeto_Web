﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour
{

    public GameObject[] menus;

    public Player_Main main;

    AudioSource audioS;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeMenu(float index)
    {
        ActivateSelectedMenu(index);
    }

    public void ChangeMusicVolume(Slider slider)
    {
        GameManager.singleton.music.volume = slider.value;
    }

    public void ChangeSoundsVolume(Slider slider)
    {
        GameManager.singleton.soundVolume = slider.value;
        GameManager.singleton.ChangeSoundVolume();
    }

    public void PlayButtonSound(AudioClip sound)
    {
        audioS.clip = sound;
        audioS.Play();
    }

    void ActivateSelectedMenu(float index)
    {
        for(int x = 0; x < menus.Length; x++)
        {
            if (x == index)
                menus[x].SetActive(true);
            else
                menus[x].SetActive(false);
        }
    }

    public void Resume(GameObject menu)
    {
        main.shipState = ShipState.FUELED;
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
