using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player_Movement : MonoBehaviour
{

    Player_Main main;

    Rigidbody2D rb;
    ChromaticAberration chromaticAberration;
    

    public ParticleSystem[] smokes;
    ParticleSystem.EmissionModule[] smokesEmission;

    [HideInInspector]
    public float ver, hor;

    bool onSlowMotion;

    public float spd, handling;

    public float moveEnergyCost, slowMoEnergyCost;

    // Start is called before the first frame update
    void Start()
    {
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();

        volume.profile.TryGetSettings(out chromaticAberration);

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
        transform.eulerAngles += new Vector3(0, 0, -hor * handling * Time.deltaTime);

        if (ver > 0)
        {
            rb.AddForce(spd * transform.up * ver * Time.deltaTime);
            main.energy -= moveEnergyCost * Time.deltaTime;

            TurnParticlesOnOff(true);
        }
        else
        {
            TurnParticlesOnOff(false);
        }
    }

    public void TurnParticlesOnOff(bool isOn)
    {
        if(isOn)
        {
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

            if (!onSlowMotion)
            {
                StopAllCoroutines();
                StartCoroutine(SlowMotionEffect(true));
            }

            onSlowMotion = true;
        }
        else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = .02f * Time.timeScale;

            if (onSlowMotion)
            {
                StopAllCoroutines();
                StartCoroutine(SlowMotionEffect(false));
            }

            onSlowMotion = false;
        }

        if (onSlowMotion)
            main.energy -= slowMoEnergyCost * Time.deltaTime;
    }

    public IEnumerator SlowMotionEffect(bool activate)
    {

        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        if (activate)
        {
            while(chromaticAberration.intensity.value < .5f)
            {
                chromaticAberration.intensity.value += 2.5f * Time.deltaTime;
                chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value, 0, .5f);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (chromaticAberration.intensity.value > 0)
            {
                chromaticAberration.intensity.value -= 2.5f * Time.deltaTime;
                chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value, 0, .5f);
                yield return new WaitForFixedUpdate();
            }
        }
    }


}
