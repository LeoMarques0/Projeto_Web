using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Gun : MonoBehaviour
{
    Player_Main main;

    public ParticleSystem[] shot;

    public float shotDelay, gunEnergyCost;

    ParticleSystem.MainModule[] shotMain;

    bool canShoot = true;

    //Sound Variables
    public AudioSource shotSFX;

    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Player_Main>();

        shotMain = new ParticleSystem.MainModule[shot.Length];

        for(int i = 0; i < shot.Length; i++)
            shotMain[i] = shot[i].main;
    }

    public void Shoot()
    {
        if (main.controls.Gameplay.Shoot.phase == UnityEngine.InputSystem.InputActionPhase.Started && canShoot)
        {
            shotSFX.Play();
            StartCoroutine(ShotDelay());
            for (int i = 0; i < shot.Length; i++)
            {
                shotMain[i].startRotationZ = -transform.eulerAngles.z * Mathf.Deg2Rad;
                shot[i].Play();
            }

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
