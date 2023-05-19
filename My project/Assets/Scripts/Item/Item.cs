using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Item : NetworkBehaviour
{
    public enum ItemType
    {
        HPUP,
        SPEEDUP,
        MISSILESPEEDUP,
        MISSILEASIZEUP,
        MISSILEACOUNTUP,
        MISSILEBTIMEUP, 
        END
    }

    ItemType myType;

    public void Init()
    {
        myType = (ItemType)Random.Range(0, (int)ItemType.END);
    }

    void Use(Upgrade target)
    {
        target.UpGrade(myType);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Upgrade upgrade = other.gameObject.GetComponent<Upgrade>();
        if(upgrade != null )
        {
            Use(upgrade);
        }
    }
}
