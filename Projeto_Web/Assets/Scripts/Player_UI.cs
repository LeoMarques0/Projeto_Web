using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{

    Player_Main main;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
