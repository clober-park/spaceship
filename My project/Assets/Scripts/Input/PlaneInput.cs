using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlaneButtons
{
    Fire1 = 0,
    Fire2
}

public struct PlaneInput : INetworkInput
{
    public Vector3 ViewVec;
    public NetworkButtons Buttons;
}
