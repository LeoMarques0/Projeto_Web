using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShipState
{
    FUELED,
    EMPTY,
    PAUSE,
    DEAD
}

public class Player_Main : MonoBehaviour
{

    [HideInInspector]
    public Player_Gun gun;
    [HideInInspector]
    public Player_Movement movement;
    [HideInInspector]
    public Player_UI ui;

    public ShipState shipState = new ShipState();

    public Animator boostAnim;
    public Transform objective, pointer;

    public GameObject explosion;

    Vector3 objectivePos;
    float angle;
    bool dead = false;
    

    public float maxEnergy;
    public float health = 100;

    [HideInInspector]
    public bool paused;

    [HideInInspector]
    public float energy;

    void Awake()
    {
        gun = GetComponent<Player_Gun>();
        movement = GetComponent<Player_Movement>();
        ui = GetComponent<Player_UI>();

        energy = maxEnergy;

        objective = GameObject.FindWithTag("Objective").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (shipState)
        {
            case ShipState.FUELED:

                gun.Shoot();

                movement.Updates();
                movement.SlowMotion();

                ui.EnergyBar();

                if (health <= 0)
                    shipState = ShipState.DEAD;

                break;

            case ShipState.DEAD:

                if(!dead)
                {
                    StartCoroutine(DeathScene());
                    dead = true;
                }

                break;
        }

        AnimationValues();
        StateManager();
        TrackObjective();

        ui.Pause();
        ui.CanvasButtons();

    }

    private void FixedUpdate()
    {
        switch (shipState)
        {
            case ShipState.FUELED:

                movement.Handling();

                break;
        }
    }

    void StateManager()
    {
        switch (shipState)
        {
            case ShipState.FUELED:

                if (energy <= 0)
                {
                    shipState = ShipState.EMPTY;
                    movement.TurnParticlesOnOff(false);
                    movement.ver = 0;
                    StartCoroutine(movement.SlowMotionEffect(false));
                    energy = 0;
                }

                break;

            case ShipState.EMPTY:
                Mute();
                break;

            case ShipState.PAUSE:
                Mute();
                break;
        }
    }

    void Mute()
    {
        gun.shotSFX.Stop();
        movement.engine.Stop();
        movement.slowMotion.Stop();
    }

    public void AnimationValues()
    {
        boostAnim.SetInteger("Ver", Mathf.RoundToInt(movement.ver));
    }

    public void TrackObjective()
    {
        objectivePos = objective.position - pointer.position;
        angle = Mathf.Atan2(objectivePos.y, objectivePos.x) * Mathf.Rad2Deg;

        pointer.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    IEnumerator DeathScene()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        transform.Find("Sprite").gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        ui.GameOver();
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
