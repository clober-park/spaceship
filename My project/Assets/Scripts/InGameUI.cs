using Doozy.Runtime.Reactor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class InGameUI : NetworkBehaviour
{
    [SerializeField]Image hpbarfill;
    [SerializeField] TextMeshProUGUI timeUI;
    [SerializeField] GameManager gm;
    [SerializeField] Dictionary<PlayerRef, Upgrade> upgrades;
    [SerializeField] Dictionary<PlayerRef, Health> HPs;

    [Header("UpgradeCounter")]
    [SerializeField] UpgradeCounter UCspeed;
    [SerializeField] UpgradeCounter UChp;
    [SerializeField] UpgradeCounter UCMspeed;
    [SerializeField] UpgradeCounter UCMAsize;
    [SerializeField] UpgradeCounter UCMAcount;
    [SerializeField] UpgradeCounter UCMBtime;
    string timetext = "Time : ";

    float recordtime;

    void SetUI(PlayerRef player)
    {
        upgrades[player].SpeedUpHandler += UCspeed.PlusCounter;
        upgrades[player].HPUpHandler += UChp.PlusCounter;
        upgrades[player].MSpeedUpHandler += UCMspeed.PlusCounter;
        upgrades[player].MAsizeUpHandler += UCMAsize.PlusCounter;
        upgrades[player].MAcountUpHandler += UCMAcount.PlusCounter;
        upgrades[player].MBtimeUpHandler += UCMBtime.PlusCounter;
        HPs[player].OnDamageHandler += UpdateHPBar;
    }

    public override void FixedUpdateNetwork()
    {
        UpdateTime();
    }

    public float GetTime()
    {
        return recordtime;
    }

    void UpdateHPBar()
    {
        hpbarfill.fillAmount = gm.Players[Object.InputAuthority].myPlane.HP.GetHPPer;
    }

    void UpdateTime()
    {
        timeUI.text = timetext + string.Format("{0:0.#}", gm.gameTime);
    }

}
