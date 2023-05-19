using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : NetworkBehaviour
{
    NetworkObject[] wallparts;
    private void Start()
    {
        wallparts = GetComponentsInChildren<NetworkObject>();
        if (Runner == null)
            Runner = FindAnyObjectByType<NetworkRunner>();
    }
}
