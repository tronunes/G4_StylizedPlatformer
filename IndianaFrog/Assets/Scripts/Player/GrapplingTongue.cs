using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTongue : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform tongueStart; // Where the tongue starts
    [SerializeField] private Transform tongueMid; // The middle, stretchy part of the tongue
    [SerializeField] private GameObject tongueEndPrefab; // Prefab reference to the end part of the tongue which latches onto walls
    private GameObject tongueEnd = null; // The actual end part of the tongue. Created when shot.
    private float shootForce = 35f;

    void Start()
    {
        HideTongue();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootTongue();
        }
    }

    void LateUpdate()
    {
        FixTongueVisuals();
    }

    void ShootTongue()
    {
        // Destroy previous tongue
        if (tongueEnd)
        {
            Destroy(tongueEnd);
        }

        // Create a new tongue
        tongueEnd = Instantiate(tongueEndPrefab, tongueStart.position + tongueStart.forward * 0.5f, tongueStart.rotation);
        Rigidbody tongueRb = tongueEnd.GetComponent<Rigidbody>();
        tongueRb.velocity = Vector3.zero;

        // Shoot the tongue forwards
        tongueRb.AddForce(playerCamera.transform.forward * shootForce, ForceMode.Impulse);

        ShowTongue();
    }

    void ShowTongue()
    {
        tongueStart.gameObject.SetActive(true);
    }

    void HideTongue()
    {
        tongueStart.gameObject.SetActive(false);
    }

    void FixTongueVisuals()
    {
        if (tongueEnd)
        {
            tongueStart.LookAt(tongueEnd.transform);

            tongueMid.localScale = new Vector3(
                1f,
                1f,
                (tongueEnd.transform.position - tongueStart.position).magnitude * 5f
            );
        }
    }
}
