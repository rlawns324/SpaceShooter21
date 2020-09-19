using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "BULLET"){
            ContactPoint[] points = other.contacts;
            Vector3 normalVec = points[0].normal;
            Instantiate(sparkEffect, points[0].point, Quaternion.LookRotation(normalVec));
            Destroy(other.gameObject);
        }
    }
}
