using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveItem : MonoBehaviour
{

    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    void MuteSound()
    {
        GameManager manager = GameManager.singleton;
        float currentSound = manager.soundVolume;

        manager.soundVolume = 0;
        manager.ChangeSoundVolume();

        manager.soundVolume = currentSound;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            MuteSound();
            collision.GetComponent<Player_Main>().paused = true;
            anim.Play("StageComplete");
            Time.timeScale = 0;
        }
    }
}
