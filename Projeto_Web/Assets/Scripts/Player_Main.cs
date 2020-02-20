using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main : MonoBehaviour
{

    Player_Gun gun;
    Player_Movement movement;

    public float energy;

    void Awake()
    {
        gun = GetComponent<Player_Gun>();
        movement = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        gun.Shoot();

        movement.Updates();
        movement.SlowMotion();
    }

    private void FixedUpdate()
    {
        movement.Handling();
    }
}
