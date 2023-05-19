using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CollisionDetector
{

    public static bool TryGetSphereCollisionObject(Vector3 originPos, Vector3 TargetPos, float radius, int collisionLayermask, int GetGoLayermask, out GameObject collisionGo)
    {
        RaycastHit hit;
        collisionGo = null;
        Vector3 Direction = TargetPos - originPos;
        if (Physics.SphereCast(originPos, radius, Direction.normalized, out hit, Direction.magnitude, collisionLayermask))
        {
                if (hit.transform.gameObject.layer == GetGoLayermask)
                {
                    collisionGo = hit.transform.gameObject;
                }
            return true;
        }
        return false;
    }
    public static bool TryGetBoxCollisionObject(GameObject detector, Vector3 pastPos, int collisionLayermask, int GetGoLayermask, out GameObject collisionGo)
    {
        RaycastHit hit;
        collisionGo = null;
        Vector3 Direction = detector.transform.position - pastPos;
        Transform detectortransform = detector.transform;
        if (Physics.BoxCast(pastPos,
            detectortransform.localScale / 2f,
            Direction.normalized,
            out hit,
            detectortransform.rotation,
            Direction.magnitude,
            collisionLayermask))
        {
                if (hit.transform.gameObject.layer == GetGoLayermask)
                {
                    collisionGo = hit.transform.gameObject;
                }
            return true;
        }
        return false;
    }
    static public bool SearchSphere(GameObject detector, float searchreidus, int TargetLayer, out Collider[] colls)
    {
        if (Physics.CheckSphere(detector.transform.position, searchreidus, TargetLayer))
        {
            colls = Physics.OverlapSphere(detector.transform.position, searchreidus, TargetLayer);
            return true;
        }
        colls = null;
        return false;
    }
    public static bool TryGetSearchNearTarget(GameObject detector,float distance, string TargetLayername , out GameObject searchGo)
    {
        searchGo = null;
        if (SearchSphere(detector, distance, LayerMask.GetMask(TargetLayername), out var colls))
        {
            float sqrdistanc = distance * distance;
            foreach (var v in colls)
            {
                Vector3 Vec = v.transform.position - detector.transform.position;
                if (Vec.sqrMagnitude < (sqrdistanc))
                {
                    sqrdistanc = Vec.sqrMagnitude;
                    searchGo = v.gameObject;
                }
            }
            return true;
        }
        return false;
    }
    public static bool TrySearchFrontViewTarget(GameObject detector, string targetlayername)
    {
        return Physics.Raycast(detector.transform.position, detector.transform.forward, Mathf.Infinity, LayerMask.GetMask(targetlayername));
    }

}
