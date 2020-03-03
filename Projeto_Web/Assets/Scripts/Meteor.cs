using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Meteor : MonoBehaviour
{
    public enum CollidersType
    {
        BOX,
        CIRCLE,
        CAPSULE,
        POLYGON
    }

    public float meteorScale;
    public float speed;

    public Vector2 direction;

    public bool interceptPlayer;
    public bool needsPlayerRange;
    public CollidersType colliderType = new CollidersType();

    //Box Collider
    public BoxCollider2D boxCollider;
    public Vector2 boxOffset = Vector2.zero, boxSize = Vector2.one;
    //Box Collider

    //Capsule Colldier
    public CapsuleCollider2D capsuleCollider;
    public Vector2 capsuleOffset = Vector2.zero, capsuleSize = Vector2.one;
    public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;
    //Capsule Colldier

    //Circle Collider
    public CircleCollider2D circleCollider;
    public Vector2 circleOffset = Vector2.zero;
    public float circleRadius = .5f;
    //Circle Collider

    //PolygonCollider
    public PolygonCollider2D polygonCollider;
    public Vector2 polygonOffset = Vector2.zero;
    public List<Vector2[]> polygonPaths = new List<Vector2[]>();
    //PolygonCollider

    bool movementActive = false;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!needsPlayerRange || movementActive)
            rb.velocity = speed * direction.normalized * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && needsPlayerRange && !movementActive)
        {
            Vector2 playerPossiblePosition = (Vector2)collision.transform.position + collision.GetComponent<Rigidbody2D>().velocity;
            direction = playerPossiblePosition - (Vector2)transform.position;

            movementActive = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(direction != Vector2.zero)
            Gizmos.DrawRay(transform.position, direction.normalized);
    }
}
