using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    Player_Main main;

    public ParticleSystem shot;

    public float shotDelay, gunEnergyCost;

    ParticleSystem.MainModule shotMain;

    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Player_Main>();

        shotMain = shot.main;
    }

    public void Shoot()
    {
        if (Input.GetKey(KeyCode.X) && canShoot)
        {
            StartCoroutine(ShotDelay());
            shotMain.startRotationZ = -transform.eulerAngles.z * Mathf.Deg2Rad;
            shot.Play();

            main.energy -= gunEnergyCost;
        }
    }

    IEnumerator ShotDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
}
