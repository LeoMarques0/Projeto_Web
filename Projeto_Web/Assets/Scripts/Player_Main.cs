using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FuelState
{
    FUELED,
    EMPTY
}

public class Player_Main : MonoBehaviour
{

    Player_Gun gun;
    Player_Movement movement;

    FuelState fuelState = new FuelState();

    public Animator boostAnim;
    public Slider energyBar;
    public Transform canvasObj;
    public GameObject pauseMenu;

    public float maxEnergy;

    [HideInInspector]
    public float energy;

    void Awake()
    {
        gun = GetComponent<Player_Gun>();
        movement = GetComponent<Player_Movement>();

        energy = maxEnergy;

        canvasObj.SetParent(null, false);

    }

    // Update is called once per frame
    void Update()
    {
        switch (fuelState)
        {
            case FuelState.FUELED:

                gun.Shoot();

                movement.Updates();
                movement.SlowMotion();

                EnergyBar();

                break;
        }

        AnimationValues();
        StateManager();
        Pause();

    }

    private void FixedUpdate()
    {
        switch (fuelState)
        {
            case FuelState.FUELED:

                movement.Handling();

                break;
        }
    }

    void StateManager()
    {
        switch (fuelState)
        {
            case FuelState.FUELED:

                if (energy <= 0)
                {
                    fuelState = FuelState.EMPTY;
                    movement.TurnParticlesOnOff(false);
                    movement.ver = 0;
                    StartCoroutine(movement.SlowMotionEffect(false));
                    energy = 0;
                }

                break;
        }
    }

    public void AnimationValues()
    {
        boostAnim.SetInteger("Ver", Mathf.RoundToInt(movement.ver));
    }

    void EnergyBar()
    {
        energyBar.value = energy / maxEnergy;
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fuel")
        {
            energy += 50;
            energy = Mathf.Clamp(energy, 0, 100);

            Destroy(collision.gameObject);
        }
    }
}
