using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ItemDrop : MonoBehaviour
{
    PoolingManager pm;
    private void Awake()
    {
        pm = FindObjectOfType<PoolingManager>();
    }
    public void DropItem(Vector3 DropPlace)
    {
        //if (Random.Range(0, 100) < 40)
        //    return;
        Item item = pm.Item.Get(DropPlace, Quaternion.identity, PlayerRef.None, null).GetComponent<Item>();
        item.Init();
    }

}
