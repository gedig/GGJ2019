using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaghettiGrow : MonoBehaviour
{
    public int NumSpaghettis = 0;

    public GameObject spaghetti;

    private Vector3 initialScale;
    private float currentScaleMod = 1.0f;
    private float sizeModPerSpaghetti = 0.5f;
    private const float MAX_SCALE_MOD = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = spaghetti.transform.localScale;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTERED!");
        if (other.tag == "Spaghetti") {
            Destroy(other.gameObject);
            if (NumSpaghettis == 0) {
                spaghetti.SetActive(true);
            }
            else {
                currentScaleMod += sizeModPerSpaghetti;
                currentScaleMod = Mathf.Max(MAX_SCALE_MOD, currentScaleMod);
                spaghetti.transform.localScale = initialScale * currentScaleMod;
            }

            NumSpaghettis++;
        }
    }
}
