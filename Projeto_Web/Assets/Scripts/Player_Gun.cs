using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    Player_Main main;

    public ParticleSystem shot;

    public float shotDelay;

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
            print(transform.eulerAngles.z);
            shotMain.startRotationZ = -transform.eulerAngles.z * Mathf.Deg2Rad;
            shot.Play();

            main.energy -= 1;
        }
    }

    IEnumerator ShotDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
}
