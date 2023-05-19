using Cysharp.Threading.Tasks;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static Unity.Collections.Unicode;


public class Enemy : NetworkBehaviour
{
    public enum EnemyState
    {
        Normal = 0,
        Chase,
        Avoid
    }
    public Plane myPlane;
    public float damage;
    public EnemyState state = EnemyState.Normal;

    [Networked] TickTimer delay { get; set; }
    [SerializeField]Plane Target;
    bool isActive = false;
    bool isableShoot = true;

    EnemyState GetRandomState ()
    {
        return (EnemyState)Random.Range(0, 3);
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (isActive is false) return;

        GetTarget();

        if (Target == null) return;

        TurnView();
        ShootMissile();
    }

    void GetTarget()
    {
        if (Runner.IsPlayer is false) return;

        if (Target != null && Target.gameObject.activeSelf is false)
            {
                { Target = null; }
            }

        var targetdistance_min = 200f;
        foreach (var p in Runner.ActivePlayers)
        {
            if(Runner.GetPlayerObject(p) == null) continue;

            var targetpos = Runner.GetPlayerObject(p).gameObject.GetComponent<Player>().myPlaneTransform.position;
            var mypos = myPlane.transform.position;
            var distance = (targetpos - mypos).magnitude;
            if(Target == null)
            {
                targetdistance_min = distance;
                Target = Runner.GetPlayerObject(p).gameObject.GetComponent<Player>().myPlane;
            }
            else if (distance <= targetdistance_min)
            {
                targetdistance_min = distance;
                Target = Runner.GetPlayerObject(p).gameObject.GetComponent<Player>().myPlane;
            }
        }
    }

    void TurnView()
    {
        if (Target != null)
        {
            if (Target.gameObject.activeSelf is false) { Target = null; }
        }
        var dir = GetMoveDir();
        dir.y = 0;
        myPlane.TurnView(dir);
    }

    public void Init(EnemyState state, PlaneData data)
    {
        this.state = state;
        myPlane.gameObject.SetActive(true);
        myPlane.Init(data, transform.position, "Player");
        myPlane.OnDeadHandler += Dead;
        transform.position = Vector3.zero;
        isActive = true;
    }

    public void ChangeState (EnemyState state)
    {
        this.state = state;
    }

    Vector3 GetMoveDir ()
    {
        Vector3 moveVec;
        if (Target == null)
            state = (EnemyState) (- 1);
        switch (state)
        {
            case EnemyState.Normal:
                moveVec = Normal();
                break;
            case EnemyState.Chase:
                moveVec = Chase();
                break;
            case EnemyState.Avoid:
                moveVec = Avoid();
                break;
            default:
                moveVec = Vector3.zero;
                break;
        }
        return moveVec;
    }

    public Vector3 Normal()
    {
        return  (Target.TM.position - myPlane.TM.position);
    }

    public Vector3 Chase()
    {
        Vector3 Vec = Target.TM.position - myPlane.TM.position;
        float lookaheadtime = Vec.sqrMagnitude / (float)((myPlane.Speed + Target.Speed) * (myPlane.Speed + Target.Speed));
        return Vec + Target.Forward * lookaheadtime;
    }

    public Vector3 Avoid()
    {
        return myPlane.TM.position - Target.TM.position;
    }

    public void ShootMissile()
    {
        if (delay.ExpiredOrNotRunning(Runner))
        {
            if (HasLookTarget() is false) return;

            delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                myPlane.ShootA();
        }
    }

    bool HasLookTarget()
    {
        if (myPlane.hasMissile is false) return false;
        if (isableShoot is false) return false;
        if (Target == null) return false;
        var hitAsteroid = Runner.LagCompensation.Raycast(transform.position, transform.forward, Mathf.Infinity,
            Object.InputAuthority, out var hit, LayerMask.GetMask("Player"));
        if (hitAsteroid is false) return false;

        return true;
    }
    void Dead()
    {
        Runner.Despawn(Object);
    }

    private void OnDisable()
    {
        isActive = false;
    }
}
