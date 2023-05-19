using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TryGetSearchNearTargetTest : MonoBehaviour
{
    public GameObject spawnObject;

    // Start is called before the first frame update
    void Start()
    {
        var detector = new GameObject("detecor");
        detector.transform.position = Vector3.zero;

        GameObject.Instantiate(spawnObject, new Vector3(0, 1, 0), Quaternion.identity).name = "A";
        GameObject.Instantiate(spawnObject, new Vector3(1, 1, 0), Quaternion.identity).name = "B";
        GameObject.Instantiate(spawnObject, new Vector3(2, 1, 1), Quaternion.identity).name = "C";

        spawnObject.SetActive(false);

        Assert.IsTrue(CollisionDetector.SearchSphere(detector, 2.0f, LayerMask.GetMask("Enemy"), out var colliders));
        Assert.AreEqual(2, colliders.Length);
        Assert.IsTrue(CollisionDetector.SearchSphere(detector, 3.0f, LayerMask.GetMask("Enemy"), out colliders));
        Assert.AreEqual(3, colliders.Length);
        Assert.IsFalse(CollisionDetector.SearchSphere(detector, 3.0f, LayerMask.GetMask("Player"), out colliders));
        Assert.IsNull(colliders);

        Assert.IsTrue(CollisionDetector.TryGetSearchNearTarget(detector, 2.0f, "Enemy", out var searchGo));
        Assert.AreEqual("A", searchGo.name);    
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
