using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{

    Plane upgradetarget;
    [Header("State")]
    public float speedUPState;
    public float hpUPState;
    public float missileSpeedUPState;
    public int missileACountUPState;
    public float missileASizeUPState;
    public int missileBTimeUPState;
    [Header("Max")]
    public int SpeedUPMax;
    public int HPUPMax;
    public int missileSpeedUPMax;
    public int missileASizeUPMax;
    public int missileACountUPMax;
    public int missileBTimeUPMax;
    int SpeedUPCount;
    int HPUpCount;
    int missileSpeedUPCount;
    int missileASizeUPCount;
    int missileACountUPCount;
    int missileBTimeUPCount;

    public event Action SpeedUpHandler;
    public event Action HPUpHandler;
    public event Action MSpeedUpHandler;
    public event Action MAsizeUpHandler;
    public event Action MAcountUpHandler;
    public event Action MBtimeUpHandler;

    private void Start()
    {
        upgradetarget = GetComponent<Plane>();  
    }
    public void UpGrade(Item.ItemType type)
    {
        switch (type)
        {
            case Item.ItemType.SPEEDUP:
                SpeedUP();
                break;
            case Item.ItemType.HPUP:
                HPUP();
                break;
            case Item.ItemType.MISSILESPEEDUP:
                MissileSpeedUP();
                break;
            case Item.ItemType.MISSILEASIZEUP:
                MissileASizeUP();
                break;
            case Item.ItemType.MISSILEACOUNTUP:
                MissileACountUP();
                break;
            case Item.ItemType.MISSILEBTIMEUP:
                MissileBTimeUP();
                break;
        }
    }

    public void SpeedUP()
    {
        if (SpeedUPCount >= SpeedUPMax)
            return;
        SpeedUPCount++;
        upgradetarget.UpgradeSpeed(speedUPState * SpeedUPCount);
        SpeedUpHandler?.Invoke();
    }
    public void HPUP()
    {
        if (HPUpCount >= HPUPMax)
            return;
        HPUpCount++;
        upgradetarget.UpgradeHP(hpUPState * HPUpCount);
        HPUpHandler?.Invoke();
    }
    public void MissileSpeedUP()
    {
        if(missileSpeedUPCount >= missileSpeedUPMax)
            return;
        missileSpeedUPCount++;
        upgradetarget.UpgradeMissileSpeed(missileSpeedUPState * missileSpeedUPCount);
        MSpeedUpHandler?.Invoke();
    }
    public void MissileASizeUP()
    {
        if (missileASizeUPCount >= missileASizeUPMax)
            return;

        missileASizeUPCount++; 
        upgradetarget.UpgradeSizeAMissile(missileASizeUPState * missileASizeUPCount);
        MAsizeUpHandler?.Invoke();
    }
    public void MissileACountUP()
    {
        if (missileACountUPCount >= missileACountUPMax)
            return;

        missileACountUPCount++; 
        upgradetarget.UpgradeCountAMissile(missileACountUPState * missileACountUPCount);
        MAcountUpHandler?.Invoke();
    }
    public void MissileBTimeUP()
    {
        if (missileBTimeUPCount >= missileBTimeUPMax)
            return;

        missileBTimeUPCount++; 
        upgradetarget.UpgradeTimeBMissile(missileBTimeUPState * missileBTimeUPCount);
        MBtimeUpHandler?.Invoke();
    }
}
