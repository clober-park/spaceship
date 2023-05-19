using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.Signals;
using System;
using Fusion;

public class Player : NetworkBehaviour
{
    [Networked] TickTimer delay { get; set; }

    public Plane myPlane;
    public Transform myPlaneTransform => myPlane.transform;
    FollowCamera followCamera;
    
    Vector3 _forward;
    [Networked] private NetworkButtons _buttonsPrevious { get; set; }
    [Networked] private TickTimer _shootCooldown { get; set; }

    private void Awake()
    {
        Camera camera = FindAnyObjectByType<Camera>();
        if(camera.name == "Main Camera")
        {
            followCamera = camera.GetComponent<FollowCamera>();
        }
        myPlane.OnDeadHandler += Dead;
    }
    public override void FixedUpdateNetwork()
    {
        if(Runner.TryGetInputForPlayer<PlaneInput>(Object.InputAuthority, out var input))
        {
            TurnViewPlane(input);
            ShootMissile(input);
        }
            SetCameraSpeed();
        gameObject.SetActive(myPlane.gameObject.activeSelf);
    }

    public void Init(PlaneData data)
    {
        myPlane.Init(data, transform.position, "Enemy");
        transform.position = Vector3.zero;
        _forward = myPlane.Forward;
        if (followCamera != null)
            followCamera.Init(myPlane);
    }

    void SetCameraSpeed()
    {
        if (followCamera != null)
            followCamera.ChangeMove(myPlane.Speed);
    }
    void TurnViewPlane(PlaneInput input)
    {
        _forward = input.ViewVec;
        myPlane.TurnView(_forward);
    }
    void ShootMissile(PlaneInput input)
    {
        if (delay.ExpiredOrNotRunning(Runner))
        {
            if (input.Buttons .WasPressed(_buttonsPrevious, PlaneButtons.Fire1))
            {
                delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                myPlane.ShootA();
            }
            else if (input.Buttons.WasPressed(_buttonsPrevious, PlaneButtons.Fire2))
            {
                delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                myPlane.ShootB();
            }
        }
    }
    void Dead()
    {
        Runner.Despawn(Object);
    }
}
