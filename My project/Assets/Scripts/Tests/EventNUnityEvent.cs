using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewBehaviourScript2 : MonoBehaviour
{
    public UnityEvent OnDeadEventHandler = new UnityEvent();
    public event System.Action Onss;
    public System.Action T;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Dead ()
    {
        OnDeadEventHandler.Invoke();
    }

    public void IsCalled ()
    {

    }
}

public class GameMa
{
    NewBehaviourScript2 player;
    System.Action OnStartEventHandler;

    void StartCall()
    {
        OnStartEventHandler?.Invoke();
    }

    void TGest ()
    {     
        player.OnDeadEventHandler.AddListener(OnDead);

        player.Onss += OnDead;
        player.Onss -= OnDead;    
    }

    void OnDead ()
    {
        // callback
        // Game Á¾·á

    }
}
