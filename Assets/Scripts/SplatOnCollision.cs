using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatOnCollision : MonoBehaviour
{
    public GameObject spawnObjectOnCollision;

    private void OnCollisionEnter(Collision collision) {
        ContactPoint contact = collision.contacts[0];
        RaycastHit hit;

        float backTrackLength = 1f;
        Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
        if (collision.collider.Raycast(ray, out hit, 2)) {
            GameObject spawned = GameObject.Instantiate(spawnObjectOnCollision);
            spawned.transform.position = contact.point + (ray.direction * -0.01f);
            spawned.transform.forward = ray.direction;
            spawned.transform.Rotate(spawned.transform.forward, Random.value);
            spawned.transform.parent = collision.transform.parent;
        }

        Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 5, true);

        Destroy(this.gameObject);
    }

}
