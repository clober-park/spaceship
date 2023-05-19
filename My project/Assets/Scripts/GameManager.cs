using Cysharp.Threading.Tasks;
using Doozy.Runtime.Signals;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class PlaneData
{
    float speed;
    float rotatespeed;
    float hp;
    float collisiondamage;
    float missileadamage;
    float missilebdamage;
    float missilespeed;
    float missilerotatespeed;
    public float Speed => this.speed;
    public float RSpeed => this.rotatespeed;
    public float HP => this.hp;
    public float ColDamage => this.collisiondamage;
    public float MADamage => this.missileadamage;
    public float MBDamage => this.missilebdamage;
    public float MSpeed => this.missilespeed;
    public float MRSpeed => this.missilerotatespeed;

    public PlaneData(float speed = 1,
    float rotatespeed = 1,
    float hp = 10,
    float collisiondamage = 1,
    float missileadamage = 1,
    float missilebdamage = 1,
    float missilespeed = 1,
    float missilerotatespeed = 1)
    {
        this.speed              = speed; 
        this.rotatespeed        = rotatespeed;
        this.hp                 = hp;
        this.collisiondamage    = collisiondamage;
        this.missileadamage     = missileadamage;
        this.missilebdamage     = missilebdamage;
        this.missilespeed       = missilespeed;
        this.missilerotatespeed = missilerotatespeed;
    }
}


public class GameManager : SimulationBehaviour ,IPlayerJoined, IPlayerLeft, ISpawned
{
    PlaneData playerdata;
    public Dictionary<PlayerRef, Player> Players;
    public ItemDrop itemdroper;
    public EnemyManager enemyM;
    public RecordRanking record;
    [SerializeField]Transform[] spawnpoints;
    PoolingManager pm;
    float starttime;
    int enemydelaytime = 1000;
    int EnemyLimitCount = 2;
    bool gameIsReady = false;
    bool gameisEnd = false;

    public float gameTime => Time.realtimeSinceStartup - starttime;
    public int Score => enemyM.DeadCounter;
    private void Awake()
    {
        pm = FindAnyObjectByType<PoolingManager>();
        Runner = FindAnyObjectByType<NetworkRunner>();
        playerdata = FindAnyObjectByType<PlayerData>().playerData;
    }
    void Start()
    {
        starttime = Time.realtimeSinceStartup;
        record.OutRankingHandler += OutRanking;
        record.RecordCompleteHandler += CompleteRecord;

        pm.Wall.Get(Vector3.zero, Quaternion.identity, PlayerRef.None);
        StartPlayerSpawner();
        EnemySpawn().Forget();
    }

    public void StartPlayerSpawner()
    {
        gameIsReady = true;
        foreach (var player in Runner.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }
    public void PlayerJoined(PlayerRef player)
    {
        if (gameIsReady is false) return; 
        GameEnd();
        SpawnPlayer(player);
        Debug.Log(player.ToString());
    }
    public void PlayerLeft(PlayerRef player)
    {
        DespawnSpaceship(player);
    }

    public void Spawned()
    {
        if (Object.HasStateAuthority == false) return;
        // Collect all spawn points in the scene.
    }

    private void SpawnPlayer(PlayerRef player)
    {
        int index = player % spawnpoints.Length;
        var spawnPosition = spawnpoints[index].position;

        var playerObject = pm.Player.Get(spawnPosition, Quaternion.identity, player);
        playerObject.GetComponent<Player>().Init(playerdata);
        playerObject.GetComponent<Player>().myPlane.OnDeadHandler += CheckGameEnd;
        // Set Player Object to facilitate access across systems.
        Runner.SetPlayerObject(player, playerObject);
    }
    void CheckGameEnd()
    {
        gameisEnd = true;
        foreach(var playerref in Runner.ActivePlayers)
        {
            if (Runner.GetPlayerObject(playerref) != null)
            {
                gameisEnd = false;
                break;
            }
        }
        GameEnd();
    }
    private void DespawnSpaceship(PlayerRef player)
    {
        if (Runner.TryGetPlayerObject(player, out var spaceshipNetworkObject))
        {
            Runner.Despawn(spaceshipNetworkObject);
        }

        // Reset Player Object
        Runner.SetPlayerObject(player, null);
    }



    async UniTaskVoid EnemySpawn()
    {
        if (enemyM == null) return;
        while (true)
        {
            await UniTask.Delay(enemydelaytime, cancellationToken: this.GetCancellationTokenOnDestroy());
            if (enemyM.EnemyList.Count < EnemyLimitCount)
            {
                Enemy newEnemy = enemyM.SpawnMissileTypeEnemy();
                void itemdrop()
                {
                    itemdroper.DropItem(newEnemy.myPlane.transform.position);
                }
                newEnemy.myPlane.OnDeadHandler += itemdrop;
            }
        }
    }


    void Record()
    {
        gameisEnd = true;
        Signal.Send("InGame", "Record");
    }
    void GameOver()
    {
        gameisEnd = true;
        Signal.Send("InGame", "GameOver");
    }
    void CompleteRecord()
    {
        Signal.Send("Record", "Record");
    }
    void OutRanking()
    {
        Signal.Send("Record", "OutRanking");
    }
    public void GameEnd()
    {
        if (gameisEnd is false) return;
        Record();
    }
    public void GoGameOver()
    {
        GameOver();
    }
    public void OnclickTitle()
    {
        Runner.Shutdown();
        SceneManager.LoadScene("Title");
    }
    public void OnclickRestart()
    {
        SceneManager.LoadScene("InGame");
    }
}



