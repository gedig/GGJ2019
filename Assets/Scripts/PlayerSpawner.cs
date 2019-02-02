using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject PrefabToSpawn;

    void Awake()
    {
        GameObject maybeExistingPlayer = GameObject.Find("Player");
        if (maybeExistingPlayer == null) {
            Debug.Log("Spawning new player!");
            Instantiate(PrefabToSpawn);
        } else {
            Debug.Log("Spawning new player!");
            Destroy(gameObject);
        }
    }
}
