using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileA : MissileBase
{
    public override void FixedUpdateNetwork()
    {
        base.Move();
        base.HasHit();
    }

    public void Init(Vector3 StartPos, Vector3 ViewVec ,float damage, float speed, float rotatespeed, string targetlayername = "Enemy")
    {
        base.Init(StartPos, ViewVec, damage, speed, rotatespeed, targetlayername);
    }
}
