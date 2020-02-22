using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    Player_Main main;

    Rigidbody2D rb;
    

    public ParticleSystem[] smokes;
    ParticleSystem.EmissionModule[] smokesEmission;

    [HideInInspector]
    public float ver, hor;

    bool onSlowMotion;

    public float spd, handling;

    // Start is called before the first frame update
    void Start()
    {
        main = GetComponent<Player_Main>();

        rb = GetComponent<Rigidbody2D>();

        smokesEmission = new ParticleSystem.EmissionModule[smokes.Length];
        for(int x = 0; x < smokes.Length; x++)
            smokesEmission[x] = smokes[x].emission;
    }

    public void Updates()
    {
        ver = Input.GetAxisRaw("Vertical");
        hor = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -20, 20), Mathf.Clamp(rb.velocity.y, -20, 20));

        handling = onSlowMotion ? 300 : 150;
    }

    public void Handling()
    {
        if (ver >= 0)
            rb.AddForce(spd * transform.up * ver * Time.deltaTime);
        

        transform.eulerAngles += new Vector3(0, 0, -hor * handling * Time.deltaTime);

        if (ver > 0)
        {
            main.energy -= 1 * Time.deltaTime;

            for (int x = 0; x < smokesEmission.Length; x++)
                smokesEmission[x].rateOverTime = 200;
        }
        else
        {
            for (int x = 0; x < smokesEmission.Length; x++)
                smokesEmission[x].rateOverTime = 0;
        }
    }

    public void SlowMotion()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            Time.timeScale = .5f;
            Time.fixedDeltaTime = .02f * Time.timeScale;

            onSlowMotion = true;
        }
        else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = .02f * Time.timeScale;

            onSlowMotion = false;
        }

        if (onSlowMotion)
            main.energy -= 1 * Time.deltaTime;
    }


}
