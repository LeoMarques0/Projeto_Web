﻿using System.Collections;
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
    Player_UI ui;

    FuelState fuelState = new FuelState();

    public Animator boostAnim;
    public Transform objective, pointer;

    Vector3 objectivePos;
    float angle;
    

    public float maxEnergy;

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
        switch (fuelState)
        {
            case FuelState.FUELED:

                gun.Shoot();

                movement.Updates();
                movement.SlowMotion();

                ui.EnergyBar();

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

    public void TrackObjective()
    {
        objectivePos = objective.position - pointer.position;
        angle = Mathf.Atan2(objectivePos.y, objectivePos.x) * Mathf.Rad2Deg;

        pointer.rotation = Quaternion.Euler(Vector3.forward * angle);
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
