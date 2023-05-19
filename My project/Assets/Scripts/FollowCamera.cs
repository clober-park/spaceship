using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : NetworkBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] float speed;
    Transform myTransfom;
    public override void FixedUpdateNetwork()
    {
        CameraMove();
    }

    void CameraMove()
    {
        if (Target == null)
            return;
        Vector3 DirVec = Target.transform.position - myTransfom.position;
        DirVec.y = 0;
        myTransfom.position += DirVec.normalized * speed * Runner.DeltaTime;
    }

    public void Init(GameObject target)
    {
        myTransfom = this.transform;
        this.Target = target;
        myTransfom.position = Target.transform.position;
    }

    public void Init(Plane plane)
    {
        myTransfom = this.transform;
        this.Target = plane.gameObject;
        myTransfom.position = new Vector3(Target.transform.position.x, 10f, Target.transform.position.z);
        speed = plane.Speed - 0.1f;
    }

    public void ChangeMove(float speed)
    {
        this.speed = speed - 0.1f;
    }
}
