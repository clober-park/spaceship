using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Refection : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(target.position, transform.position, Color.red);
        Debug.DrawLine(target.position, ReflectVector3(), Color.green);
    }

    Vector3 ReflectVector3 ()
    {
        var n = target.up;
        var d = -Vector3.Dot(n, target.position);

        Matrix4x4 mat = new Matrix4x4();
        mat.m00 = 1 - 2 * n.x * n.x;
        mat.m01 = -2 * n.x * n.y;
        mat.m02 = -2 * n.x * n.z;
        mat.m03 = -2 * n.x * d;

        mat.m10 = -2 * n.x * n.y;
        mat.m11 = 1 -2 * n.y * n.y;
        mat.m12 = -2 * n.y * n.z;
        mat.m13 = -2 * n.y * d;

        mat.m20 = -2 * n.x * n.z;
        mat.m21 = - 2 * n.y * n.z;
        mat.m22 = 1 -2 * n.z * n.z;
        mat.m23 = -2 * n.z * d;

        mat.m30 = 0;
        mat.m31 = 0;
        mat.m32 = 0;
        mat.m33 = 1;

        return (mat * transform.localToWorldMatrix).GetPosition();
    }

    private void OnDrawGizmos()
    {
        
    }
}
