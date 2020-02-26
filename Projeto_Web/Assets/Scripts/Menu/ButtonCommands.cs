using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour
{

    public GameObject[] menus;

    
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
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
