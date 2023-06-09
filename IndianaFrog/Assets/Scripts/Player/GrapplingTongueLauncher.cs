using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrapplingTongueLauncher : MonoBehaviour
{
    [Header("Technical")]
    private PlayerMovement playerMovement;
    private PlayerCameraController cameraController;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform frogMesh;
    [SerializeField] private Transform tongueStart; // Where the tongue starts
    [SerializeField] private Transform tongueMid; // The middle, stretchy part of the tongue
    [SerializeField] private GameObject tongueEndPrefab; // Prefab reference to the end part of the tongue which latches onto walls
    private GameObject tongueEnd = null; // The actual end part of the tongue. Created when shot, destroyed when reeled back in.
    public bool inputLocked = false;
    private float tongueMiddleScaler; // A scaler to scale the middle part of the tongue correctly

    // Stats
    private float shootForce = 35f;
    private float frogReelingSpeedMax = 25f; // Reeling = Frog moves towards the tongue end (which is attached to a wall)
    private float tongueRetractSpeed = 100f; // Rectracting = the tongue moves towards the Frog without moving the Frog
    private float maxTongueLength = 30f;
    [Tooltip("Make sure the value at T-0 is >= 0.1")] public AnimationCurve reelingCurve;
    [Tooltip("Does the Player get slingshot also when fully reeled in?")] public bool slingshotWhenReachingEnd = false;

    // Helpers
    private bool isReelingFrogIn = false; // Are we reeling the Frog towards TongueEnd
    private bool isRetractingTongue = false; // Are we retracting the Tongue towards the Frog (but Frog is not reeling in)
    private float tonguePreviousDistance; // Used to determine when the Tongue has been fully retracted (to prevent over retracting)
    private bool fire1PressedButNotReleased = false; // A helper boolean to know when Axis-type button is released (i.e. ButtonUp event but for Axis)
    private bool canShootTongue = true; // Require touching ground before shooting the Tongue again
    private float reelingInitialDistance; // The initial distance from TongueStart to TongueEnd when reeling begins

    [Header("Events")]
    public UnityEvent event_TongueFullyRetracted = new UnityEvent(); // Triggers when fully retracted

    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        cameraController = gameObject.GetComponent<PlayerCameraController>();
        HideTongue();

        // Calculate how much the tongue middle needs to be scaled
        tongueMiddleScaler = 1f / tongueMid.GetChild(0).GetComponent<Renderer>().localBounds.size.z;
    }

    void FixedUpdate()
    {
        // Prevent the Frog receiving input from the Player
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
            SlingshotPlayer();
            fire1PressedButNotReleased = true;
        }

        // Case: Retracting the Tongue back towards the Frog
        if (tongueEnd && isRetractingTongue)
        {
            tongueEnd.transform.position += (tongueStart.position - tongueEnd.transform.position).normalized * tongueRetractSpeed * Time.fixedDeltaTime;
            float currentTongueDistance = Vector3.Distance(tongueStart.position, tongueEnd.transform.position);

            // Destroy the Tongue when reaching the Frog
            if (currentTongueDistance < 0.2f || currentTongueDistance > tonguePreviousDistance)
            {
                DestroyTongueEnd();

                // Trigger the event
                event_TongueFullyRetracted.Invoke();
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
                // Use reeling animation
                animator.SetBool("Reeling", true);

                Vector3 tongueLengthVector = tongueEnd.transform.position - tongueStart.position;
                Vector3 tongueDirection = tongueLengthVector.normalized;
                float tongueLength = tongueLengthVector.magnitude;
                float reelingForceNormalized = reelingCurve.Evaluate(1f - (tongueLength / reelingInitialDistance));
                float reelingForce = frogReelingSpeedMax * reelingForceNormalized;

                // Add external velocity to the Frog
                Vector3 externalVelocity = tongueDirection * reelingForce * Time.fixedDeltaTime;
                playerMovement.AddExternalVelocity(externalVelocity);

                // Stop reeling when reaching the end (i.e. fully reeled in)
                if (
                    tongueLength < 0.5f ||                                                           // "Frog is close enough"
                    (tongueLength < 1.5f && playerMovement.GetPlayerVelocity().magnitude < 0.1f))    // "Frog isn't moving anymore and relatively close enough"
                {
                    RetractTongue();

                    // Slingshot the Player depending on settings
                    if (slingshotWhenReachingEnd)
                    {
                        SlingshotPlayer();
                    }
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
        tongueEnd = Instantiate(tongueEndPrefab, tongueStart.position + tongueStart.forward * 0.5f, frogMesh.rotation);
        tongueEnd.GetComponent<TongueEnd>().SetLauncher(this);
        Rigidbody tongueRb = tongueEnd.GetComponent<Rigidbody>();
        tongueRb.velocity = Vector3.zero;

        // Shoot the tongue forwards
        tongueRb.AddForce(playerCamera.transform.forward * shootForce, ForceMode.Impulse);

        ShowTongue();

        // Animate the Tongue shooting
        animator.SetTrigger("Spit");
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
                (tongueEnd.transform.position - tongueStart.position).magnitude * tongueMiddleScaler
            );
        }
        else
        {
            tongueStart.localRotation = Quaternion.identity;
            HideTongue();
        }
    }

    public void StartReeling()
    {
        isReelingFrogIn = true;

        reelingInitialDistance = Vector3.Distance(tongueEnd.transform.position, tongueStart.position);

        // Rotate the Frog correctly
        Vector3 lookAtPosition = tongueEnd.transform.position;
        frogMesh.LookAt(new Vector3(lookAtPosition.x, frogMesh.transform.position.y, lookAtPosition.z));

        // Prevent shooting the Tongue again before touching ground
        canShootTongue = false;
    }

    // This is "Start retracting", so only call when retracting starts, not every frame.
    public void RetractTongue()
    {
        // Stop reeling animation
        animator.SetBool("Reeling", false);

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

    private void SlingshotPlayer()
    {
        // Slingshot the Player with the current velocity
        playerMovement.Slingshot(playerMovement.GetPlayerVelocity() * Time.fixedDeltaTime);
    }
}
