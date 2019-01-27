using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIf : MonoBehaviour
{

    public enum DisableCondition
    {
        OTHER_OBJECT_DISABLED
    }

    public DisableCondition[] conditions = new DisableCondition[] {};
    public GameObject[] gameObjects = new GameObject[]{};

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < conditions.Length; ++i) {
            switch (conditions[i])
            {
                case DisableCondition.OTHER_OBJECT_DISABLED:
                    if (!gameObjects[i].activeInHierarchy) {
                        gameObject.SetActive(false);
                    }

                    break;
            }

            if (!isActiveAndEnabled) {
                // We've done the thing, let's get out of here.
                break;
            }
        }
    }
}
