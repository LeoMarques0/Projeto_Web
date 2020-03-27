using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != parent)
            Destroy(gameObject);
    }
}
