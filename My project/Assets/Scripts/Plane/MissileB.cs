using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Reflection;
using Fusion;


public class MissileB : MissileBase
{
    GameObject Target;
    string targetlayername;
    int delayTime;

    bool HasTarget => Target != null;
    CancellationTokenSource canceleToken;
    public override void FixedUpdateNetwork()
    {
        ReSearchTarget();  
        TurnViewMissile();
        base.Move();
        base.HasHit();
    }

    public void Init(Vector3 StartPos, Vector3 ViewVec, float damage, float speed, float rotatespeed, int delay, string targetlayername = "Enemy")
    {
        this.targetlayername = targetlayername;
        delayTime = delay;
        base.Init(StartPos, ViewVec, damage, speed, rotatespeed, targetlayername);
        canceleToken = new CancellationTokenSource();
        base.destroyhandler += canceleToken.Cancel;
        timeout().Forget();
    }

    void ReSearchTarget()
    {
        if(CollisionDetector.TryGetSearchNearTarget(this.gameObject, 100f, targetlayername, out var targetgo))
        {
            Target = targetgo;
        }
    }

    bool TurnViewMissile()
    {
        if (HasTarget is false) return false;

        base.TurnView(Target.transform.position);
        return true;
    }

    async UniTaskVoid timeout()
    {
        await UniTask.Delay(delayTime, cancellationToken: canceleToken.Token);
        base.Destroy();
    }

}
