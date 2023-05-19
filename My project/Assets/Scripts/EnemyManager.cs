using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] PlaneData data;

    List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> EnemyList => enemies;

    public Transform[] SpawnPoints;

    int enemydeadcounter = 0;
    public int DeadCounter => enemydeadcounter;

    public void Awake()
    {
        data = FindAnyObjectByType<PlayerData>().playerData;
    }

    public Enemy SpawnBodyAttackTypeEnemy()
    {
        PoolingManager pm = FindObjectOfType<PoolingManager>();
        if (pm == null) return null;
        return SpawnEnemy(pm.EnemyA);
    }
    public Enemy SpawnMissileTypeEnemy()
    {
        PoolingManager pm = FindObjectOfType<PoolingManager>();
        if (pm == null) return null;
        return SpawnEnemy(pm.EnemyB);
    }

    public Enemy SpqwnRandomTypeEnemy()
    {
        if (Random.Range(0, 2) % 2 == 0)
            return SpawnBodyAttackTypeEnemy();
        else
            return SpawnMissileTypeEnemy();
    }

    Enemy SpawnEnemy(SinglePoolingManager enemypool)
    {
        Enemy enemy = enemypool.Get(SpawnPoints[Random.Range(0, SpawnPoints.Length)].position,
            Quaternion.identity, PlayerRef.None, null).GetComponent<Enemy>();
        enemy.myPlane.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;
        enemy.Init(Enemy.EnemyState.Normal, data);
        enemy.myPlane.OnDeadHandler += EnemyDeadCount;
        enemies.Add(enemy);
        return enemy;
    }

    void EnemyDeadCount()
    {
        enemydeadcounter++;
    }
}
