using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class PoolingManager : MonoBehaviour
{

    [SerializeField] SinglePoolingManager PlayerPool;
    [SerializeField] SinglePoolingManager EnemyAPool;
    [SerializeField] SinglePoolingManager EnemyBPool;
    [SerializeField] SinglePoolingManager MAPool;
    [SerializeField] SinglePoolingManager MBPool;
    [SerializeField] SinglePoolingManager ItemPool;
    [SerializeField] SinglePoolingManager WallPool;

    public SinglePoolingManager Player => PlayerPool;
    public SinglePoolingManager EnemyA => EnemyAPool;
    public SinglePoolingManager EnemyB => EnemyBPool;
    public SinglePoolingManager MissileA => MAPool;
    public SinglePoolingManager MissileB => MBPool;
    public SinglePoolingManager Item => ItemPool;
    public SinglePoolingManager Wall => WallPool;

    void Start()
    {
        //var count = FindObjectsOfType<PoolingManager>().Length;
        //if (count > 1)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //
        //DontDestroyOnLoad(gameObject);
    }    


}
