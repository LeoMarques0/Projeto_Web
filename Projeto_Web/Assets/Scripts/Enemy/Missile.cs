using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Weapon
{
    public Transform target;
    Rigidbody2D rb;

    public GameObject explosion;

    public float spd;
    public float handling;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Invoke("Explode", 5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Utility.LookAt2D(transform.rotation, handling, target.position, transform.position);

        rb.velocity = transform.up * spd;
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        Explode();
    }
}
