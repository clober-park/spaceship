using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    float defaultHP;
    float plusHP;

    public event Action OnDamageHandler;
    public float GetHPPer => HP / MaxHP;

    public bool isDead => HP <= 0;

    public void Init(float hp)
    {
        defaultHP = hp;
        MaxHP = hp;
        HP = hp;
    }
    public void Upgrade(float plus)
    {
        plusHP = plus;
        MaxHP = defaultHP + plusHP;
    }
    public void Heal()
    {
        HP += plusHP;
        if(HP > MaxHP) { HP =  MaxHP; }
    }

    public void Damage(float damage)
    {
        HP -= damage;
        OnDamageHandler?.Invoke();
    }

}
