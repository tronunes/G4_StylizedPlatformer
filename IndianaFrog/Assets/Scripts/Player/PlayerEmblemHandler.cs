using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmblemHandler : MonoBehaviour
{
    private int emblemsCollected = 0;
    public int requiredEmblemCount = 0;


    void Start()
    {
        // If required emblem count is not defined in the inspector
        // -> count the number of emblems in the scene and require collecting them all
        if (requiredEmblemCount == 0)
        {
            requiredEmblemCount = GameObject.FindGameObjectsWithTag("Emblem").Length;
        }
    }

    public void CollectEmblem()
    {
        emblemsCollected++;
        Debug.Log("Emblem collected");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Emblem"))
        {
            CollectEmblem();
        }
    }
}
