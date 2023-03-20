using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueEnd : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Latch onto the target
        transform.SetParent(collider.transform);
    }
}
