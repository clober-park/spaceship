using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.Unicode;

public class MissileManager : NetworkBehaviour
{
    PoolingManager pm;

    float missileplusspeed;
    float Amissilescale;
    int addAmissilecount;
    int Bmissileplusdelay;
    int defaultdelay = 1000;

    public void Init()
    {
        pm = FindObjectOfType<PoolingManager>();
        if (pm == null) return;
    }
    Quaternion GetQuaternion(Vector3 ViewVec)
    {
        return Quaternion.LookRotation(ViewVec.normalized, Vector3.up);
    }
    public void AttackA(Vector3 ShootPos, Vector3 ViewVec, float damage = 10, float speed = 4, float rotatespeed = 4, string targetlayername = "Enemy")
    {
        for(int missilecount= 0; missilecount < 1 + addAmissilecount; missilecount++)
        {
            MissileA missile = pm.MissileA.Get(ShootPos, GetQuaternion(ViewVec)).GetComponent<MissileA>();
            Vector3 turnVec = Quaternion.AngleAxis(360 / (1 + addAmissilecount) * missilecount, Vector3.up) * ViewVec;
            Vector3 ScaleVec = Vector3.one * (1 + Amissilescale) * 0.1f;
            missile.transform.localScale = ScaleVec;
            missile.Init(ShootPos, turnVec, damage, speed + missileplusspeed, rotatespeed, targetlayername);
        }
    }
    public void AttackB(Vector3 ShootPos, Vector3 ViewVec, float damage = 5, float speed = 4, float rotatespeed = 4, string targetlayername = "Enemy")
    {
        MissileB missile = pm.MissileB.Get(ShootPos, GetQuaternion(ViewVec)).GetComponent<MissileB>();
        missile.Init(ShootPos, ViewVec, damage, speed + missileplusspeed, rotatespeed, defaultdelay + Bmissileplusdelay, targetlayername);
    }

    public void UpgradeSpeedMissile(float speed)
    {
        missileplusspeed = speed;
    }

    public void UpgradeSizeAMissile(float scale)
    {
        Amissilescale = scale;
    }
    public void UpgradeCountAMissile(int count)
    {
        addAmissilecount = count;
    }

    public void UpgradeBMissile(int delay)
    {
        Bmissileplusdelay = delay;

    }
}
