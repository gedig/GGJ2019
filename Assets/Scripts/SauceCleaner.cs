using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauceCleaner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("ENTERED SINK!");
        if (other.tag == "GetsCleanedUp") {
            Destroy(other.gameObject);
        }
    }
}
