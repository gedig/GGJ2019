using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        for (int i = 0; i < other.transform.childCount; ++i) {
            Transform otherChild = other.transform.GetChild(i);
            if (otherChild.tag == "GetsCleanedUp") {
                Destroy(otherChild.gameObject);
            }
        }
    }
}
