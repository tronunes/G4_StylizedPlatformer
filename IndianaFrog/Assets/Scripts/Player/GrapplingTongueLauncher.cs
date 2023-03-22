using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTongueLauncher : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform tongueStart; // Where the tongue starts
    [SerializeField] private Transform tongueMid; // The middle, stretchy part of the tongue
    [SerializeField] private GameObject tongueEndPrefab; // Prefab reference to the end part of the tongue which latches onto walls
    private GameObject tongueEnd = null; // The actual end part of the tongue. Created when shot.
    private float shootForce = 35f;
    private float reelingSpeed = 10f;
    private bool isReeling; // Are we reeling the Frog towards TongueEnd

    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        HideTongue();
    }

    void Update()
    {
        // Case: Shoot Tongue
        if (!tongueEnd && Input.GetButtonDown("Fire1"))
        {
            ShootTongue();
        }
        // Case: cancel reeling
        else if (tongueEnd && Input.GetButtonDown("Fire1"))
        {
            StopReeling();
        }

        // Reeling the Frog in
        if (isReeling)
        {
            playerMovement.AddExternalVelocity((tongueEnd.transform.position - transform.position).normalized * reelingSpeed);
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
        tongueEnd.GetComponent<TongueEnd>().SetLauncher(this);
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

            // Scale the Tongue's middle part to fill the gap between the Frog and the Tongue end part
            tongueMid.localScale = new Vector3(
                1f,
                1f,
                (tongueEnd.transform.position - tongueStart.position).magnitude * 5f
            );
        }
        else
        {
            HideTongue();
        }
    }

    public void StartReeling()
    {
        isReeling = true;
    }

    public void StopReeling()
    {
        isReeling = false;
        Destroy(tongueEnd);
    }
}
