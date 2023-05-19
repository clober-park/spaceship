using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class MissileBase : NetworkBehaviour
{
    [SerializeField]float damage;
    [SerializeField] float speed;
    [SerializeField] float rotatespeed;
    [SerializeField] string TargetLayername;
    [SerializeField]int collisionLayermask;

    Vector3 pastPos;
    Vector3 curPos;
    Vector3 ViewVec;

    public event Action destroyhandler;

    public bool IsDestroyed => gameObject.activeSelf is false;

    public void Init(Vector3 myPos, Vector3 ViewVec, float damage, float speed,float rotatespeed, string Targetlayername)
    {
        this.transform.position = myPos;
        this.damage = damage;
        this.speed = speed;
        this.rotatespeed = rotatespeed;
        this.TargetLayername = Targetlayername;
        this.ViewVec = ViewVec;
        collisionLayermask = LayerMask.GetMask(TargetLayername, "Wall");
        SetView(ViewVec);
    }
    public void Move()
    {
        pastPos = transform.position;
        this.transform.position += this.transform.forward.normalized * speed * Runner.DeltaTime;
        curPos = transform.position;
    }

    void SetView(Vector3 ViewVec)
    {
        this.transform.rotation = GetQuaternion(ViewVec);
    }
    public void TurnView(Vector3 TargetPos)
    {
        ViewVec = (TargetPos - curPos).normalized;
        this.transform.rotation = Quaternion.Slerp(transform.rotation, GetQuaternion(ViewVec), rotatespeed);
    }

    Quaternion GetQuaternion(Vector3 ViewVec)
    {
        return Quaternion.LookRotation(ViewVec.normalized, Vector3.up);
    }

    public void DealDamage(Health target)
    {
        target.Damage(damage);
        Destroy();
    }

    protected bool HasHit()
    {
        var isHit = Runner.LagCompensation.Raycast(pastPos, ViewVec, speed * Runner.DeltaTime,
                Object.InputAuthority, out var hit, collisionLayermask);
        if (isHit is false) return false;

        Health hitTarget = hit.GameObject.GetComponent<Health>();
        if (hitTarget == null)
        {
            Destroy();
            return true;
        }

        if (hitTarget.isDead is true) return false;

        DealDamage(hitTarget);
        return true;
    }

    public void Destroy()
    {
        destroyhandler?.Invoke();
        Runner.Despawn(Object);
    }

}
