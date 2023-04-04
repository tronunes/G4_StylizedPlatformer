using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingTongueLauncher : MonoBehaviour
{
    [Header("Technical")]
    private PlayerMovement playerMovement;
    private PlayerCameraController cameraController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform tongueStart; // Where the tongue starts
    [SerializeField] private Transform tongueMid; // The middle, stretchy part of the tongue
    [SerializeField] private GameObject tongueEndPrefab; // Prefab reference to the end part of the tongue which latches onto walls
    private GameObject tongueEnd = null; // The actual end part of the tongue. Created when shot, destroyed when reeled back in.
    public bool inputLocked = false;

    // Stats
    private float shootForce = 35f;
    private float frogReelingSpeed = 10f; // Reeling = Frog moves towards the tongue end (which is attached to a wall)
    private float tongueRetractSpeed = 100f; // Rectracting = the tongue moves towards the Frog without moving the Frog
    private float maxTongueLength = 30f;

    // Helpers
    private bool isReelingFrogIn = false; // Are we reeling the Frog towards TongueEnd
    private bool isRetractingTongue = false; // Are we retracting the Tongue towards the Frog (but Frog is not reeling in)
    private float tonguePreviousDistance; // Used to determine when the Tongue has been fully retracted (to prevent over retracting)
    private bool fire1PressedButNotReleased = false; // A helper boolean to know when Axis-type button is released (i.e. ButtonUp event but for Axis)
    private bool canShootTongue = true; // Require touching ground before shooting the Tongue again

    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        cameraController = gameObject.GetComponent<PlayerCameraController>();
        HideTongue();
    }

    void Update()
    {
        if (inputLocked)
        {
            return;
        }

        // Require touching ground before shooting the Tongue again (while Tongue not out)
        if (!tongueEnd && playerMovement.IsGrounded())
        {
            canShootTongue = true;
        }

        // Check if Fire1 "button" is released, i.e. ButtonUp-event (but for axis instead of button)
        if (fire1PressedButNotReleased && Input.GetAxis("Fire1") <= 0f)
        {
            fire1PressedButNotReleased = false;
        }

        // Case: Shoot Tongue (only when zoomed) when Fire1 pressed
        if (canShootTongue && !tongueEnd && cameraController.IsZoomed() && Input.GetAxis("Fire1") > 0f && !fire1PressedButNotReleased)
        {
            ShootTongue();
            fire1PressedButNotReleased = true;
        }
        // Case: Retract tongue when Fire1 pressed again
        else if (tongueEnd && Input.GetAxis("Fire1") > 0f && !fire1PressedButNotReleased)
        {
            RetractTongue();
            fire1PressedButNotReleased = true;
        }

        // Case: Retracting the Tongue back towards the Frog
        if (tongueEnd && isRetractingTongue)
        {
            tongueEnd.transform.position += (tongueStart.position - tongueEnd.transform.position).normalized * tongueRetractSpeed * Time.deltaTime;
            float currentTongueDistance = Vector3.Distance(tongueStart.position, tongueEnd.transform.position);

            // Destroy the Tongue when reaching the Frog
            if (currentTongueDistance < 0.2f || currentTongueDistance > tonguePreviousDistance)
            {
                DestroyTongueEnd();
            }
            else
            {
                tonguePreviousDistance = currentTongueDistance;
            }
        }
        // Case: Tongue not being retracted (but might not even exist at all)
        else
        {
            // Case: Reeling the Frog in
            if (isReelingFrogIn)
            {
                playerMovement.AddExternalVelocity((tongueEnd.transform.position - tongueStart.position).normalized * frogReelingSpeed * Time.deltaTime);

                // Stop reeling when reaching the end (i.e. fully reeled in)
                float frogDistanceToTongueEnd = Vector3.Distance(tongueStart.position, tongueEnd.transform.position);
                if (
                    frogDistanceToTongueEnd < 0.2f ||                                                           // "Frog is close enough"
                    (frogDistanceToTongueEnd < 1.5f && playerMovement.GetPlayerVelocity().magnitude < 0.1f))    // "Frog isn't moving anymore and relatively close enough"
                {
                    RetractTongue();
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
                        RetractTongue();
                    }
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
        DestroyTongueEnd();

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
        isReelingFrogIn = true;

        // Prevent shooting the Tongue again before touching ground
        canShootTongue = false;
    }

    // This is "Start retracting", so only call when retracting starts, not every frame.
    public void RetractTongue()
    {
        if (tongueEnd)
        {
            isRetractingTongue = true;
            tonguePreviousDistance = Vector3.Distance(tongueEnd.transform.position, tongueStart.position);

            // Disable collision detection for the retracting Tongue
            tongueEnd.GetComponent<Rigidbody>().detectCollisions = false;
            tongueEnd.GetComponent<Collider>().enabled = false;
        }
    }

    void DestroyTongueEnd()
    {
        isRetractingTongue = false;
        isReelingFrogIn = false;
        if (tongueEnd) { Destroy(tongueEnd); }
    }

    public void ResetTongueLauncher()
    {
        DestroyTongueEnd();
        HideTongue();
    }
}
