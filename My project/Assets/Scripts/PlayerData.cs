using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private string _nickname;
    PlaneData pData;
    PlaneData eData;
    public PlaneData playerData => pData;
    public PlaneData enemyData => eData;

    [Header("Player"), 
     SerializeField] float player_speed;
    [SerializeField] float player_rotatespeed;
    [SerializeField] float player_hp;
    [SerializeField] float player_collisiondamage;
    [SerializeField] float player_missileadamage;
    [SerializeField] float player_missilebdamage;
    [SerializeField] float player_missilespeed;
    [SerializeField] float player_missilerotatespeed; 

    [Header("Enemy"),
     SerializeField] float enemy_speed;
    [SerializeField] float enemy_rotatespeed;
    [SerializeField] float enemy_hp;
    [SerializeField] float enemy_collisiondamage;
    [SerializeField] float enemy_missileadamage;
    [SerializeField] float enemy_missilebdamage;
    [SerializeField] float enemy_missilespeed;
    [SerializeField] float enemy_missilerotatespeed;

    private void Start()
    {
        var count = FindObjectsOfType<PlayerData>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
            return;
        }

        pData = new PlaneData(
                    player_speed,
                    player_rotatespeed,
                    player_hp,
                    player_collisiondamage,
                    player_missileadamage,
                    player_missilebdamage,
                    player_missilespeed,
                    player_missilerotatespeed);

        eData = new PlaneData(
                    enemy_speed,
                    enemy_rotatespeed,
                    enemy_hp,
                    enemy_collisiondamage,
                    enemy_missileadamage,
                    enemy_missilebdamage,
                    enemy_missilespeed,
                    enemy_missilerotatespeed);

        DontDestroyOnLoad(gameObject);
    }

    public void SetNickName(string nickName)
    {
        _nickname = nickName;
    }

    public string GetNickName()
    {
        if (string.IsNullOrWhiteSpace(_nickname))
        {
            _nickname = GetRandomNickName();
        }

        return _nickname;
    }

    public static string GetRandomNickName()
    {
        var rngPlayerNumber = Random.Range(0, 9999);
        return $"Player {rngPlayerNumber.ToString("0000")}";
    }
}
