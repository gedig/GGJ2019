using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt: MonoBehaviour
{
    //public Vector3 worldUp = Vector3.zero;
    public GameObject target;

    void Start()
    {
        /*if (worldUp == Vector3.zero) {
            worldUp = Vector3.up;
        }
        else {
            worldUp.Normalize();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (target) {
            //transform.LookAt(target.transform, worldUp);
            transform.forward = (target.transform.position - transform.position).normalized;
        }
    }
}
