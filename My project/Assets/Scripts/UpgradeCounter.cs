using Doozy.Runtime.Reactor.Animators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCounter : MonoBehaviour
{
    [SerializeField] UIAnimator[] counters;

    int curcount = 0;
    // Start is called before the first frame update
   public void PlusCounter()
    {
        counters[curcount].Play();
        curcount++;
    }
}
