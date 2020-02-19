using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    Rigidbody2D rb;
    float ver, hor;

    bool onSlowMotion;

    public float spd, handling;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ver = Input.GetAxisRaw("Vertical");
        hor = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -30, 30), Mathf.Clamp(rb.velocity.y, -30, 30));

        handling = onSlowMotion ? 300 : 150;
            
        SlowMotion();
    }

    private void FixedUpdate()
    {
        Handling();
    }

    void Handling()
    {
        rb.AddForce(spd * transform.up * ver * Time.deltaTime);

        transform.eulerAngles += new Vector3(0, 0, -hor * handling * Time.deltaTime);
    }

    void SlowMotion()
    {
        if(Input.GetKey(KeyCode.Space))
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
    }


}
