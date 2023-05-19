using Cysharp.Threading.Tasks;
using Fusion;
using System;
using System.Threading;
using UnityEngine;

// 이 비행기는 오직 자동으로 직진을 한다.
// 그리고 외부에서는 방향과 미사일만 입력을 받는다.
// 데미지도 받는다. 데미지를 받으면 터진다.
[RequireComponent(typeof(Health))]
public class Plane : NetworkBehaviour
{
     public Health HP;    
     [SerializeField]float speed;
     float rotatespeed; //0~1
     float collisiondamage;

    [Header("Missile")]
    [SerializeField, Tooltip("이 값을 넣으면 미사일 쏘고, 안 넣으면 안 쏜다.")] MissileManager missileManager;
    Transform mytransform;

    float missilespeed;
    float missilerotatespeed;
    float missileAdamage;
    float missileBdamage;
    string EnemyTargetname;
    string ReflectTargetname;
    Vector3 ViewVec;
    Vector3 pastPos; 
    Rigidbody rigid;
    CancellationTokenSource canceleToken;
    bool isablecontrol = true;

    float upgradehp;
    float upgradespeed;


    public event Action OnDeadHandler;

    public float Speed => (speed + upgradespeed);
    public Transform TM => mytransform;
    public Vector3 Forward => mytransform.forward;
    public Vector3 Direct => ViewVec;
    public bool hasMissile => missileManager is not null;

    private void Reset()
    {
        if (HP == null)
            HP = GetComponent<Health>();
        upgradehp = 0;
        upgradespeed = 0;
    }

    private void Awake()
    {
        if(HP == null)
            HP = GetComponent<Health>();
        rigid = GetComponent<Rigidbody>();
        Runner = FindAnyObjectByType<NetworkRunner>();
        upgradehp = 0;
        upgradespeed = 0;
    }

    void Start()
    {
        mytransform = transform;
    }
    public override void FixedUpdateNetwork()
    {
        Move();
        CollisionDamage();
        Reflect();
    }
    public void Init(PlaneData data, Vector3 Pos, string EnemyTargetname = "")
    {
        this.speed = data.Speed;
        this.rotatespeed = data.RSpeed;
        this.HP.Init(data.HP);
        this.collisiondamage = data.ColDamage;
        this.missileAdamage = data.MADamage;
        this.missileBdamage = data.MBDamage;
        this.missilespeed = data.MSpeed;
        this.missilerotatespeed = data.MRSpeed;
        this.EnemyTargetname = EnemyTargetname;
        ReflectTargetname = "Wall";
        HP.OnDamageHandler += DeadCheck;
        mytransform = transform;
        mytransform.position = new Vector3(mytransform.position.x + Pos.x, 0, mytransform.position.z + Pos.z);
        canceleToken = new CancellationTokenSource();
        if (missileManager != null)
        {
            missileManager.Init();
        }
    }

    void Move()
    {
        pastPos = mytransform.position;
        Vector3 MoveVec = mytransform.forward * (speed + upgradespeed) * Runner.DeltaTime;
        mytransform.position += MoveVec;
        ViewVec = (mytransform.position - pastPos).normalized;
    }
    public void TurnView(Vector3 ViewVec)
    {
        if (isablecontrol is false)
            return;
        this.ViewVec = ViewVec;
        Quaternion qua = Quaternion.LookRotation(ViewVec, Vector3.up);
        this.mytransform.rotation = Quaternion.Slerp(this.mytransform.rotation, qua, rotatespeed);
    }
    async UniTaskVoid DisControl()
    {
        isablecontrol = false;
        await UniTask.Delay(100, cancellationToken: canceleToken.Token);
        isablecontrol = true;
    }
    void Explosion()
    {
        this.gameObject.SetActive(false);
        OnDeadHandler?.Invoke();
    }
    void DeadCheck()
    {
        if (HP.isDead)
        {
            Explosion();
        }
    }

    public void ShootA()
    {
        missileManager.AttackA(transform.position, ViewVec, missileAdamage, missilespeed, missilerotatespeed, EnemyTargetname);
    }
    public void ShootB()
    {
        missileManager.AttackB(transform.position, ViewVec, missileBdamage, missilespeed, missilerotatespeed, EnemyTargetname);
    }
    bool HasHit(string targetname, out GameObject HitObject)
    {
        HitObject = null;
        if (Runner == null) return false;

        var isHit = Runner.LagCompensation.Raycast(pastPos, ViewVec, speed * Runner.DeltaTime,
                Object.InputAuthority, out var hit, LayerMask.GetMask(targetname));
        if (isHit is false) return false; 

        HitObject = hit.GameObject;
        if (HitObject == null) return false;

        return true;
    }
    void CollisionDamage()
    {
        if (HasHit(EnemyTargetname, out var target) is false) return;

        Health targethp = target.GetComponent<Health>();
        if (targethp.isDead is true)
            return;
        HP.Damage(collisiondamage);
        targethp.Damage(collisiondamage);
    }
    public void Reflect()
    {
        if (HasHit(ReflectTargetname, out var wall) is false) return;

        Vector3 ReflectVec = ReflectionCal.GetReflectVector(ViewVec, ReflectionCal.GetNormalVector(pastPos, wall.GetComponent<BoxCollider>()));
        pastPos = mytransform.position;
        Quaternion qua = Quaternion.LookRotation(ReflectVec.normalized, Vector3.up);
        this.mytransform.rotation = qua;
        Vector3 MoveVec = ReflectVec.normalized * (speed + upgradespeed) * Runner.DeltaTime;
        MoveVec.y = 0;
        this.mytransform.position += MoveVec;
        DisControl().Forget();
    }

    public void HealHP()
    {
        this.HP.Heal();
    }
    public void UpgradeHP(float upgradestatus)
    {
        upgradehp = upgradestatus;
        this.HP.Upgrade(upgradehp);
        HealHP();
    }
    public void UpgradeSpeed(float upgradestatus)
    {
        upgradespeed = upgradestatus;
    }
    public void UpgradeMissileSpeed(float speed)
    {
        if (hasMissile is false) return;
        missileManager.UpgradeSpeedMissile(speed);
    }
    public void UpgradeSizeAMissile(float scale)
    {
        if (hasMissile is false) return;
        missileManager.UpgradeSizeAMissile(scale);
    }
    public void UpgradeCountAMissile(int count)
    {
        if (hasMissile is false) return;
        missileManager.UpgradeCountAMissile(count);
    }
    public void UpgradeTimeBMissile(int delay)
    {
        if (hasMissile is false) return;
        missileManager.UpgradeBMissile(delay);
    }

    private void OnDisable()
    {
        canceleToken.Cancel();
    }
}


