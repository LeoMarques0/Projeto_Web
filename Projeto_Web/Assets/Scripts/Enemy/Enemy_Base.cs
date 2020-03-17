using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMY_STATE
{
    IDLE,
    PATROL,
    PURSUIT,
    DYING
}

public enum PATROL_STATE
{
    MAP,
    AREA,
    WAYPOINTS
}

public class Enemy_Base : MonoBehaviour
{

    public float spd;

    public float handling;
    public float radarRadius;
    public float areaRadius;
    public Transform speedDirection;
    public PolygonCollider2D mapArea;

    public LayerMask playerLayer;

    public ENEMY_STATE state = new ENEMY_STATE();
    public PATROL_STATE patrolState = new PATROL_STATE();

    Rigidbody2D rb;

    Transform target;

    Vector3 startPos;
    Vector3 targetPos;

    //Patrol//

    Vector2 targetDir;
    float velocityDiff;
    public float patrolMaxSpd;


    //Map
    Vector2 mapPositiveLimit = Vector2.zero;
    Vector2 mapNegativeLimit = Vector2.zero;

    public LayerMask mapCollider;

    //Waypoints
    public Transform[] waypoints;
    Transform waypointsParent;

    //Patrol//

    //Pursuit//

    Vector2 playerDir;
    Vector2 targetAbsVelocity;
    Vector2 absPlayerDir;
    Vector2 currentMaxSpd;

    Rigidbody2D targetRb;

    bool targetIsMoving = false;

    public float minDistanceToPlayer;
    public float rangeToFire;
    public float pursuitMaxSpd;
    public float spdSubtractor;

    //Pursuit//

    public float shotDelay;
    public ParticleSystem[] shot;

    bool canShoot = true;
    ParticleSystem.MainModule[] shotMain;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        waypointsParent = transform.Find("Waypoints");
        speedDirection.parent = null;

        switch(patrolState)
        {
            case PATROL_STATE.AREA:

                startPos = transform.position;

                break;

            case PATROL_STATE.MAP:

                GetMapLimits();

                break;

            case PATROL_STATE.WAYPOINTS:

                targetPos = waypoints[Random.Range(0, waypoints.Length)].position;

                break;
        }

        shotMain = new ParticleSystem.MainModule[shot.Length];
        for (int i = 0; i < shot.Length; i++)
            shotMain[i] = shot[i].main;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case ENEMY_STATE.IDLE:

                Idle();

                break;

            case ENEMY_STATE.PATROL:

                Patrol();

                break;

            case ENEMY_STATE.PURSUIT:

                Pursuit();

                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case ENEMY_STATE.IDLE:

                break;

            case ENEMY_STATE.PATROL:

                FixedFollowTarget();

                break;

            case ENEMY_STATE.PURSUIT:

                FixedFollowTarget();

                break;
        }
    }

    void Idle()
    {
        SearchForTarget();

        if (target != null)
        {
            targetRb = target.GetComponent<Rigidbody2D>();
            state = ENEMY_STATE.PURSUIT;
        }
    }

    void Patrol()
    {
        switch(patrolState)
        {
            case PATROL_STATE.AREA:

                if(Vector2.Distance(transform.position, targetPos) < 1.5f)
                    targetPos = (Vector2)startPos + Vector2.ClampMagnitude(new Vector2(Random.Range(-areaRadius, areaRadius), Random.Range(-areaRadius, areaRadius)), areaRadius);

                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -patrolMaxSpd, patrolMaxSpd), Mathf.Clamp(rb.velocity.y, -patrolMaxSpd, patrolMaxSpd));

                break;

            case PATROL_STATE.MAP:

                if (Vector2.Distance(transform.position, targetPos) < 1.5f)
                {
                    bool onMap = false;
                    while (!onMap)
                    {
                        targetPos = new Vector2(Random.Range(mapNegativeLimit.x, mapNegativeLimit.y), Random.Range(mapPositiveLimit.x, mapPositiveLimit.y));
                        onMap = Physics2D.Raycast(targetPos, Vector2.zero, mapCollider);

                    }
                }

                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -patrolMaxSpd, patrolMaxSpd), Mathf.Clamp(rb.velocity.y, -patrolMaxSpd, patrolMaxSpd));

                break;

            case PATROL_STATE.WAYPOINTS:

                if (Vector2.Distance(transform.position, targetPos) < 1.5f)
                    targetPos = waypoints[Random.Range(0, waypoints.Length)].position;

                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -patrolMaxSpd, patrolMaxSpd), Mathf.Clamp(rb.velocity.y, -patrolMaxSpd, patrolMaxSpd));

                break;
        }

        transform.rotation = speedDirection.rotation;

    }

    void Pursuit()
    {
        playerDir = (transform.position - target.position).normalized;
        targetPos = (Vector2)target.position + (playerDir * rangeToFire);

        transform.up = -playerDir;

        float targetDis = Vector2.Distance(transform.position, target.position);

        if (targetRb.velocity.magnitude < 1)
            targetIsMoving = false;
        else
            targetIsMoving = true;

        if (targetDis > rangeToFire)
        {
            currentMaxSpd = new Vector2(pursuitMaxSpd, pursuitMaxSpd);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -currentMaxSpd.x, currentMaxSpd.x), Mathf.Clamp(rb.velocity.y, -currentMaxSpd.y, currentMaxSpd.y));
        }
        else
        {
            if(canShoot)
                StartCoroutine(Shoot());

            targetAbsVelocity = new Vector2(Mathf.Abs(targetRb.velocity.x), Mathf.Abs(targetRb.velocity.y));
            absPlayerDir = new Vector2(Mathf.Abs(playerDir.x), Mathf.Abs(playerDir.y));

            currentMaxSpd = targetAbsVelocity - absPlayerDir * (spdSubtractor * ((targetDis / rangeToFire) - (1 - (targetDis / rangeToFire))));

            if (currentMaxSpd.x < 0.1f && currentMaxSpd.y < 0.1f)
                currentMaxSpd = Vector2.zero;

            print(currentMaxSpd + " : " + targetDis + " : " + rangeToFire);

            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -currentMaxSpd.x, currentMaxSpd.x), Mathf.Clamp(rb.velocity.y, -currentMaxSpd.y, currentMaxSpd.y));

            print((targetDis / rangeToFire) + " - " + (1 - (targetDis / rangeToFire)) + " - " + currentMaxSpd);

        }
    }

    void FixedFollowTarget()
    {
        targetDir = (targetPos - transform.position).normalized;
        velocityDiff = 0;

        velocityDiff = Vector2.SignedAngle(rb.velocity.normalized, targetDir);

        if (state != ENEMY_STATE.PURSUIT && ((velocityDiff > 10 && velocityDiff < 100) || velocityDiff < -10 && velocityDiff > -100))
            LookAt2D(targetPos, transform.position, velocityDiff * (rb.velocity.magnitude / patrolMaxSpd));
        else if(state == ENEMY_STATE.PURSUIT)
            LookAt2D(target.position, transform.position);
        else
            LookAt2D(targetPos, transform.position);

        if (state != ENEMY_STATE.PURSUIT)
            rb.AddForce(spd * speedDirection.up * Time.deltaTime);
        else if (currentMaxSpd != Vector2.zero)
            rb.velocity = Vector2.Lerp(rb.velocity, speedDirection.up * currentMaxSpd, currentMaxSpd.magnitude * Time.deltaTime);
    }

    void SearchForTarget()
    {
        if(Physics2D.OverlapCircle(transform.position, radarRadius, playerLayer))
            target = Physics2D.OverlapCircle(transform.position, radarRadius, playerLayer).transform;
    }

    void LookAt2D(Vector2 targetPos, Vector2 currentPos, float offset)
    {
        Vector3 diff = targetPos - currentPos;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        speedDirection.rotation = Quaternion.Lerp(speedDirection.rotation, Quaternion.Euler(0f, 0f, rot_z - 90 + offset), handling * Time.deltaTime);
    }

    void LookAt2D(Vector2 targetPos, Vector2 currentPos)
    {
        Vector3 diff = targetPos - currentPos;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        speedDirection.rotation = Quaternion.Lerp(speedDirection.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), handling * Time.deltaTime);
    }

    void GetMapLimits()
    {
        for(int x = 0; x < mapArea.points.Length; x++)
        {
            if (x == 0)
            {
                mapNegativeLimit = mapArea.points[x];
                mapPositiveLimit = mapArea.points[x];
            }
            else
            {
                if (mapArea.points[x].x < mapNegativeLimit.x)
                    mapNegativeLimit.x = mapArea.points[x].x;
                if (mapArea.points[x].y < mapNegativeLimit.y)
                    mapNegativeLimit.y = mapArea.points[x].y;
                if (mapArea.points[x].x > mapPositiveLimit.x)
                    mapPositiveLimit.x = mapArea.points[x].x;
                if (mapArea.points[x].y > mapPositiveLimit.y)
                    mapPositiveLimit.y = mapArea.points[x].y;
            }
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        
        yield return new WaitForSeconds(shotDelay);

        for (int i = 0; i < shot.Length; i++)
        {
            shotMain[i].startRotationZ = -transform.eulerAngles.z * Mathf.Deg2Rad;
            shot[i].Play();
        }

        canShoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.up * 5);
        Gizmos.DrawWireCube(targetPos, Vector2.one);
    }
}
