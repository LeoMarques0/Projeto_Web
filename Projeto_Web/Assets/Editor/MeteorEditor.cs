using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Meteor))]
public class MeteorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Meteor myMeteor = (Meteor)target;

        myMeteor.meteorScale = EditorGUILayout.FloatField("Scale", myMeteor.meteorScale);
        myMeteor.transform.localScale = Vector3.one * myMeteor.meteorScale;

        myMeteor.speed = EditorGUILayout.FloatField("Speed", myMeteor.speed);

        myMeteor.interceptPlayer = GUILayout.Toggle(myMeteor.interceptPlayer, "Intercept Player");

        if(!myMeteor.interceptPlayer)
            myMeteor.direction = EditorGUILayout.Vector2Field("Direction", myMeteor.direction);

        myMeteor.needsPlayerRange = GUILayout.Toggle(myMeteor.needsPlayerRange, "Needs Player in Range");

        if (myMeteor.needsPlayerRange)
        {
            myMeteor.colliderType = (Meteor.CollidersType)EditorGUILayout.EnumPopup("Collider Type", myMeteor.colliderType);

            switch(myMeteor.colliderType)
            {
                case Meteor.CollidersType.BOX:

                    if (myMeteor.boxCollider == null)
                    {
                        myMeteor.boxCollider = myMeteor.gameObject.AddComponent<BoxCollider2D>();
                        myMeteor.boxCollider.offset = myMeteor.boxOffset;
                        myMeteor.boxCollider.size = myMeteor.boxSize;
                    }

                    myMeteor.boxCollider.isTrigger = true;

                    myMeteor.boxOffset = myMeteor.boxCollider.offset;
                    myMeteor.boxSize = myMeteor.boxCollider.size;

                    CheckColliders(myMeteor.capsuleCollider, myMeteor.circleCollider, myMeteor.polygonCollider);

                    if(GUILayout.Button("Reset Box Collider"))
                    {
                        DestroyImmediate(myMeteor.boxCollider);

                        myMeteor.boxCollider = myMeteor.gameObject.AddComponent<BoxCollider2D>();
                        myMeteor.boxOffset = myMeteor.boxCollider.offset;
                        myMeteor.boxSize = myMeteor.boxCollider.size;
                    }

                    break;

                case Meteor.CollidersType.CAPSULE:

                    if (myMeteor.capsuleCollider == null)
                    {
                        myMeteor.capsuleCollider = myMeteor.gameObject.AddComponent<CapsuleCollider2D>();
                        myMeteor.capsuleCollider.offset = myMeteor.capsuleOffset;
                        myMeteor.capsuleCollider.size = myMeteor.capsuleSize;
                        myMeteor.capsuleCollider.direction = myMeteor.capsuleDirection;
                    }

                    myMeteor.capsuleCollider.isTrigger = true;

                    myMeteor.capsuleOffset = myMeteor.capsuleCollider.offset;
                    myMeteor.capsuleSize = myMeteor.capsuleCollider.size;
                    myMeteor.capsuleDirection = myMeteor.capsuleCollider.direction;

                    CheckColliders(myMeteor.boxCollider, myMeteor.circleCollider, myMeteor.polygonCollider);

                    if (GUILayout.Button("Reset Capsule Collider"))
                    {
                        DestroyImmediate(myMeteor.capsuleCollider);

                        myMeteor.capsuleCollider = myMeteor.gameObject.AddComponent<CapsuleCollider2D>();
                        myMeteor.capsuleOffset = myMeteor.capsuleCollider.offset;
                        myMeteor.capsuleSize = myMeteor.capsuleCollider.size;
                        myMeteor.capsuleDirection = myMeteor.capsuleCollider.direction;
                    }
                    break;

                case Meteor.CollidersType.CIRCLE:

                    if (myMeteor.circleCollider == null)
                    {
                        myMeteor.circleCollider = myMeteor.gameObject.AddComponent<CircleCollider2D>();
                        myMeteor.circleCollider.offset = myMeteor.circleOffset;
                        myMeteor.circleCollider.radius = myMeteor.circleRadius;
                    }

                    myMeteor.circleCollider.isTrigger = true;

                    myMeteor.circleOffset = myMeteor.circleCollider.offset;
                    myMeteor.circleRadius = myMeteor.circleCollider.radius;

                    CheckColliders(myMeteor.boxCollider, myMeteor.capsuleCollider, myMeteor.polygonCollider);

                    if (GUILayout.Button("Reset Circle Collider"))
                    {
                        DestroyImmediate(myMeteor.circleCollider);

                        myMeteor.circleCollider = myMeteor.gameObject.AddComponent<CircleCollider2D>();
                        myMeteor.circleOffset = myMeteor.circleCollider.offset;
                        myMeteor.circleRadius = myMeteor.circleCollider.radius;
                    }

                    break;

                case Meteor.CollidersType.POLYGON:

                    if (myMeteor.polygonCollider == null)
                    {
                        myMeteor.polygonCollider = myMeteor.gameObject.AddComponent<PolygonCollider2D>();
                        myMeteor.polygonCollider.offset = myMeteor.polygonOffset;
                        if (myMeteor.polygonPaths.Count > 0)
                        {
                            if (myMeteor.polygonPaths.Count > myMeteor.polygonCollider.pathCount)
                            {
                                for (int i = 0; i < myMeteor.polygonPaths.Count; i++)
                                    myMeteor.polygonCollider.SetPath(i, myMeteor.polygonPaths[i]);
                            }
                            else
                            {
                                for (int i = 0; i < myMeteor.polygonCollider.pathCount; i++)
                                {
                                    if(i < myMeteor.polygonPaths.Count)
                                        myMeteor.polygonCollider.SetPath(i, myMeteor.polygonPaths[i]);
                                    else
                                        myMeteor.polygonCollider.SetPath(i, new Vector2[0]);
                                }
                            }
                        }
                    }

                    myMeteor.polygonCollider.isTrigger = true;

                    myMeteor.polygonOffset = myMeteor.polygonCollider.offset;
                    if (myMeteor.polygonPaths.Count > myMeteor.polygonCollider.pathCount)
                    {
                        for (int i = myMeteor.polygonPaths.Count; i > 0; i++)
                        {
                            if (i >= myMeteor.polygonCollider.pathCount)
                                myMeteor.polygonPaths.Remove(myMeteor.polygonPaths[i]);
                        }
                    }
                    for(int i = 0; i < myMeteor.polygonCollider.pathCount; i++)
                    {
                        if (i < myMeteor.polygonPaths.Count)
                            myMeteor.polygonPaths[i] = myMeteor.polygonCollider.GetPath(i);
                        else
                            myMeteor.polygonPaths.Add(myMeteor.polygonCollider.GetPath(i));
                    }

                    CheckColliders(myMeteor.boxCollider, myMeteor.capsuleCollider, myMeteor.circleCollider);

                    if (GUILayout.Button("Reset Polygon Collider"))
                    {
                        DestroyImmediate(myMeteor.circleCollider);

                        myMeteor.polygonCollider = myMeteor.gameObject.AddComponent<PolygonCollider2D>();
                        myMeteor.polygonOffset = myMeteor.polygonCollider.offset;
                        if (myMeteor.polygonPaths.Count > myMeteor.polygonCollider.pathCount)
                        {
                            for (int i = myMeteor.polygonPaths.Count; i > 0; i++)
                            {
                                if (i >= myMeteor.polygonCollider.pathCount)
                                    myMeteor.polygonPaths.Remove(myMeteor.polygonPaths[i]);
                            }
                        }
                        for (int i = 0; i < myMeteor.polygonCollider.pathCount; i++)
                        {
                            if (i < myMeteor.polygonPaths.Count)
                                myMeteor.polygonPaths[i] = myMeteor.polygonCollider.GetPath(i);
                            else
                                myMeteor.polygonPaths.Add(myMeteor.polygonCollider.GetPath(i));
                        }
                    }

                    break;
            }
        }
        else
        {
            CheckColliders(myMeteor.boxCollider, myMeteor.capsuleCollider, myMeteor.circleCollider, myMeteor.polygonCollider);
        }
    }

    void CheckColliders(BoxCollider2D boxCol, CapsuleCollider2D capCol, CircleCollider2D circleCol, PolygonCollider2D polyCol)
    {
        if (boxCol != null)
            DestroyImmediate(boxCol);

        if (capCol != null)
            DestroyImmediate(capCol);

        if (circleCol != null)
            DestroyImmediate(circleCol);
        
        if (polyCol != null)
            DestroyImmediate(polyCol);
    }

    void CheckColliders(CapsuleCollider2D capCol, CircleCollider2D circleCol, PolygonCollider2D polyCol)
    {
        if (capCol != null)
            DestroyImmediate(capCol);

        if (circleCol != null)
            DestroyImmediate(circleCol);

        if (polyCol != null)
            DestroyImmediate(polyCol);
    }

    void CheckColliders(BoxCollider2D boxCol, CircleCollider2D circleCol, PolygonCollider2D polyCol)
    {
        if (boxCol != null)
            DestroyImmediate(boxCol);

        if (circleCol != null)
            DestroyImmediate(circleCol);

        if (polyCol != null)
            DestroyImmediate(polyCol);
    }

    void CheckColliders(BoxCollider2D boxCol, CapsuleCollider2D capCol, PolygonCollider2D polyCol)
    {
        if (boxCol != null)
            DestroyImmediate(boxCol);

        if (capCol != null)
            DestroyImmediate(capCol);

        if (polyCol != null)
            DestroyImmediate(polyCol);
    }

    void CheckColliders(BoxCollider2D boxCol, CapsuleCollider2D capCol, CircleCollider2D circleCol)
    {
        if (boxCol != null)
            DestroyImmediate(boxCol);

        if (capCol != null)
            DestroyImmediate(capCol);

        if (circleCol != null)
            DestroyImmediate(circleCol);
    }
}
