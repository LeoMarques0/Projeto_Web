using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public Transform parent;
    public Vector2 shotDir;

    public float spd;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.up = shotDir;
        rb.velocity = shotDir * spd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != parent)
            Destroy(gameObject);
    }
}
