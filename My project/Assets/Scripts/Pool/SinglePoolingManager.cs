using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePoolingManager : SimulationBehaviour
{
    [SerializeField] NetworkPrefabRef prefab = NetworkPrefabRef.Empty;
    readonly private List<NetworkId> pools = new List<NetworkId>();
    private void Awake()
    {
        Runner = FindAnyObjectByType<NetworkRunner>();
    }

    public NetworkObject Get(Vector3 Pos, Quaternion rotation, PlayerRef? Pref = null, NetworkRunner.OnBeforeSpawned onBeforeSpawned = null)
    {
        var newGo = Runner.Spawn(prefab, Pos, rotation, Pref,
            onBeforeSpawned);
        newGo.transform.SetParent(this.transform);
        pools.Add(newGo.Id);
        return newGo;
    }
}