using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCal : MonoBehaviour
{

    public static Vector3 GetNormalVector(Vector3 Pos, Collider collider)
    {
        Vector3 collidernearpoint = collider.ClosestPoint(Pos);

        return (Pos - collidernearpoint).normalized;
    }

    public static Vector3 GetReflectVector(Vector3 objectDirect, Vector3 NormalVec)
    {
        float dot = Vector3.Dot(-objectDirect.normalized, NormalVec);
        return objectDirect + 2 * dot * NormalVec;
    }
}
