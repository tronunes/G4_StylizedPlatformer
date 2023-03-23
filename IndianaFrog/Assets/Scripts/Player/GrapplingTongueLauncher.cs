using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTongueLauncher : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerCameraController cameraController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform tongueStart; // Where the tongue starts
    [SerializeField] private Transform tongueMid; // The middle, stretchy part of the tongue
    [SerializeField] private GameObject tongueEndPrefab; // Prefab reference to the end part of the tongue which latches onto walls
    private GameObject tongueEnd = null; // The actual end part of the tongue. Created when shot.
    private float shootForce = 35f;
    private float reelingSpeed = 10f;
    private float maxTongueLength = 30f;
    private bool isReeling; // Are we reeling the Frog towards TongueEnd
    private bool fire1PressedButNotReleased = false; // A helper boolean to know when Axis-type button is released (i.e. ButtonUp event but for Axis)

    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        cameraController = gameObject.GetComponent<PlayerCameraController>();
        HideTongue();
    }

    void Update()
    {
        // Check if Fire1 "button" is released
        if (fire1PressedButNotReleased && Input.GetAxis("Fire1") <= 0f)
        {
            fire1PressedButNotReleased = false;
        }

        // Case: Shoot Tongue (only when zoomed)
        if (!tongueEnd && cameraController.IsZoomed() && Input.GetAxis("Fire1") > 0f && !fire1PressedButNotReleased)
        {
            ShootTongue();
            fire1PressedButNotReleased = true;
        }
        // Case: cancel reeling (when Tongue is out and Fire1 pressed again)
        else if (tongueEnd && Input.GetAxis("Fire1") > 0f && !fire1PressedButNotReleased)
        {
            StopReeling();
            fire1PressedButNotReleased = true;
        }

        // Case: Reeling the Frog in
        if (isReeling)
        {
            playerMovement.AddExternalVelocity((tongueEnd.transform.position - tongueStart.position).normalized * reelingSpeed);

            // Stop reeling when reaching the end
            if (Vector3.Distance(tongueStart.position, tongueEnd.transform.position) < 0.2f)
            {
                StopReeling();
            }
        }
        // Case: not reeling
        else
        {
            // ...but tongue has been launched
            if (tongueEnd)
            {
                // Stop reeling when reaching maximum Tongue length
                if (Vector3.Distance(tongueStart.position, tongueEnd.transform.position) >= maxTongueLength)
                {
                    StopReeling();
                }
            }
        }
    }

    void LateUpdate()
    {
        FixTongueVisuals();
    }

    void ShootTongue()
    {
        // Destroy previous tongue
        StopReeling();

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
        if (tongueEnd) { Destroy(tongueEnd); }
    }
}
