using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{

    Player_Main main;

    Animator canvasAnim;

    public Slider energyBar;
    public Transform canvasObj;
    public GameObject pauseMenu;

    public Image slowMotionButton, shootButton;

    public Sprite[] slowMotion, shoot;

    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Player_Main>();

        canvasObj.SetParent(null, false);

        canvasAnim = canvasObj.GetComponent<Animator>();
    }

    public void CanvasButtons()
    {
        shootButton.sprite = Input.GetKey(KeyCode.X) ? shoot[1] : shoot[0];
        slowMotionButton.sprite = Input.GetKey(KeyCode.Z) ? slowMotion[1] : slowMotion[0];  
    }

    public void EnergyBar()
    {
        energyBar.value = main.energy / main.maxEnergy;
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && main.shipState == ShipState.FUELED)
        {
            main.shipState = ShipState.PAUSE;
            pauseMenu.SetActive(true);
            StartCoroutine(PauseUpdate());
            Time.timeScale = 0;
        }
    }

    IEnumerator PauseUpdate()
    {
        yield return null;
        while (main.shipState == ShipState.PAUSE)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && main.shipState == ShipState.PAUSE)
            {
                main.shipState = ShipState.FUELED;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            yield return null;
        }
    }

    public void GameOver()
    {
        canvasAnim.Play("GameOverFadeIn");
    }
}
