using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Quaternion LookAt2D(Quaternion currentRot, float handling, Vector2 targetPos, Vector2 currentPos, float offset)
    {
        Vector3 diff = targetPos - currentPos;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        return Quaternion.Lerp(currentRot, Quaternion.Euler(0f, 0f, rot_z - 90 + offset), handling * Time.deltaTime);
    }

    public static Quaternion LookAt2D(Quaternion currentRot, Vector2 targetPos, Vector2 currentPos, float offset)
    {
        Vector3 diff = targetPos - currentPos;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0f, 0f, rot_z - 90 + offset);
    }
    

    public static Quaternion LookAt2D(Quaternion currentRot, float handling, Vector2 targetPos, Vector2 currentPos)
    {
        Vector3 diff = targetPos - currentPos;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        return Quaternion.Lerp(currentRot, Quaternion.Euler(0f, 0f, rot_z - 90), handling * Time.deltaTime);
    }

    public static Quaternion LookAt2D(Quaternion currentRot, Vector2 targetPos, Vector2 currentPos)
    {
        Vector3 diff = targetPos - currentPos;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
