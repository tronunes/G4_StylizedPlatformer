using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform frogMesh;
    [SerializeField] private Transform cameraLookTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 playerVelocity = Vector3.zero; // Serialized for debugging
    [SerializeField] private bool isGrounded;
    [SerializeField] private GrapplingTongueLauncher grapplingTongueLauncher;
    public bool inputLocked = false;

    private CharacterController characterController;
    private bool isZoomed = false;
    private Vector3 playerPreviousFramePosition;
    private bool chargingJump = false;
    private Vector3 externalVelocity = Vector3.zero;

    [Header("Movement values")]
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float terminalVelocity = -5f; // Has to be a negative value
    [SerializeField] private float gravity = -10f; // Has to be a negative value
    private float verticalVelocity;
    private Vector3 knockbackVelocity;
    private float wallJumpHorizontalVelocity;
    private Vector3 slingshotVelocity;

    [Header("Jumping values")]
    [SerializeField] private float gravityMultiplierPostApex = 5f;
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float maxChargeJumpHeight = 1f;

    private float chargeJumpTimer = 0f;
    private float jumpInputDecayTimer = 0f;
    private float jumpVelocity;
    private float chargeJumpVelocity;


    [Header("Sliding")]
    [SerializeField] private float slidingFriction; // Higher number means the slide is faster
    [SerializeField] private float slidingLength; // How many units the slide moves the character forward
    private bool slidingInput;
    private bool slidingState;
    private float slideStartVelocity;
    private float slidingVelocity;
    private float slidingCooldown;

    [Header("Wall Cling and Wall Jump")]
    [SerializeField] private bool clingingState;
    [SerializeField] private float maxAngle; // Determines how large the angle between the character's facing direction and the face of the wall can be
    [SerializeField] private float wallDetectionLength;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float wallClingSlideSpeed; // The speed at which the player slides down the wall
    [SerializeField] private bool wallJumpState;
    [SerializeField] private float wallJumpDrag; // Has to be a negative value
    [SerializeField] private float wallJumpLength;
    [SerializeField] private float wallJumpGravity; // Has to be a negative value
    [SerializeField] private float maxWallJumpHeight;
    private float wallJumpHorizontalStartVelocity;
    private GameObject previouslyClungWall;
    private RaycastHit frontWallHit; // Stored information from the previous raycast that hit a wall
    private float wallJumpVelocity;

    [Header("Knockback")]
    public bool knockbackState;
    float horizontalKnockbackDrag = 15f;

    [Header("Reeling slingshot")]
    public bool slingshotState = false;
    float horizontalSlingshotDrag = 15f;


    void Start()
    {
        // SlidingFriction and wallJumpDrag multiplied by ten to keep the friction numbers small (just looks nicer)
        slidingFriction *= 10;
        wallJumpDrag *= 10;

        characterController = gameObject.GetComponent<CharacterController>();
        playerPreviousFramePosition = transform.position;

        // Calculate jump velocity for fixed jump height, first for charged jump, then uncharged
        chargeJumpVelocity = Mathf.Sqrt(-2.0f * gravity * maxChargeJumpHeight);
        jumpVelocity = Mathf.Sqrt(-2.0f * gravity * maxJumpHeight);

        // Calculate slide start velocity
        slideStartVelocity = Mathf.Sqrt(-2.0f * -slidingFriction * slidingLength);
        slidingCooldown = 1f;

        // Calculate walljump velocity for fixed jump height
        wallJumpVelocity = Mathf.Sqrt(-2.0f * wallJumpGravity * maxWallJumpHeight);

        // Calculate force for horizontal movement during wall jump
        wallJumpHorizontalStartVelocity = Mathf.Sqrt(-2.0f * wallJumpDrag * wallJumpLength);
    }

    void Update()
    {
        if (inputLocked)
        {
            chargingJump = false;
            slidingInput = false;
            return;
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            chargingJump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            chargingJump = false;
        }

        // Slide
        if (Input.GetButtonDown("Slide"))
        {
            slidingInput = true;
        }
        else if (Input.GetButtonUp("Slide"))
        {
            slidingInput = false;
        }

        float inputVerticalAxisValue = Input.GetAxis("Vertical");
        float inputHorizontalAxisValue = Input.GetAxis("Horizontal");

        // Calculate Player's velocity
        // NOTE: I don't know why this doesn't cause error when Time.timeScale = 0
        playerVelocity = (transform.position - playerPreviousFramePosition) / Time.deltaTime;
        playerPreviousFramePosition = transform.position;


        // KNOCKBACK
        // =========

        // Perform player knockback for this frame if the player is in the knockback state, is in the air, and isn't travelling directly upwards at a velocity below 9
        // The latter half is there to make sure the player regains control partway through a lava caused knockback
        if (knockbackState && !isGrounded && (Mathf.Abs(knockbackVelocity.x) != 0f && Mathf.Abs(knockbackVelocity.z) != 0f) ^ knockbackVelocity.y >= 9f)
        {
            // Bring knockbackVelocity x and z values closer to zero, and bring y down continuously, keep x and z at zero if they already are zeros, to avoid errors
            knockbackVelocity.x = knockbackVelocity.x == 0f ? 0f : knockbackVelocity.x - Mathf.Sign(knockbackVelocity.x) * horizontalKnockbackDrag * Time.deltaTime;
            knockbackVelocity.z = knockbackVelocity.z == 0f ? 0f : knockbackVelocity.z - Mathf.Sign(knockbackVelocity.z) * horizontalKnockbackDrag * Time.deltaTime;
            knockbackVelocity.y -= 30f * Time.deltaTime;

            // Add knockback only if the Player isn't still
            if (playerVelocity != Vector3.zero)
            {
                AddExternalVelocity(knockbackVelocity * Time.deltaTime);
            }

            // Make sure the player can't slide during a knockback
            slidingInput = false;
            slidingVelocity = 0f;
            slidingState = false;
        } 
        // Exit knockbackState
        else if (knockbackState)
        {
            // Keep the player's vertical velocity to avoid a sudden stop in the air
            verticalVelocity = knockbackVelocity.y;

            knockbackState = false;
        }


        // INPUT LOCK
        // ==========

        // Prevent the Frog receiving input from the Player
        if (inputLocked)
        {
            inputVerticalAxisValue = 0f;
            inputHorizontalAxisValue = 0f;

            // Cancel jumping
            chargeJumpTimer = 0f;
            jumpInputDecayTimer = 0f;

            // Cancel sliding
            slidingVelocity = 0f;
            slidingState = false;
            slidingInput = false;
        }

        // REELING SLINGSHOT
        // =================

        if (slingshotState && !isGrounded && (Mathf.Abs(slingshotVelocity.x) != 0f && Mathf.Abs(slingshotVelocity.z) != 0f) ^ slingshotVelocity.y >= 9f)
        {
            // Bring slingshotVelocity x and z values closer to zero, and bring y down continuously, keep x and z at zero if they already are zeros, to avoid errors
            slingshotVelocity.x = slingshotVelocity.x == 0f ? 0f : slingshotVelocity.x - Mathf.Sign(slingshotVelocity.x) * horizontalSlingshotDrag * Time.deltaTime;
            slingshotVelocity.z = slingshotVelocity.z == 0f ? 0f : slingshotVelocity.z - Mathf.Sign(slingshotVelocity.z) * horizontalSlingshotDrag * Time.deltaTime;
            slingshotVelocity.y -= 30f * Time.deltaTime;

            // Add slingshotVelocity only if the Player isn't still
            if (playerVelocity != Vector3.zero)
            {
                AddExternalVelocity(slingshotVelocity * Time.deltaTime);
            }

            // Make sure the player can't slide during a slingshot
            slidingInput = false;
            slidingVelocity = 0f;
            slidingState = false;
        } 
        // Exit slingshotState
        else if (slingshotState)
        {
            // Keep the player's vertical velocity to avoid a sudden stop in the air
            verticalVelocity = slingshotVelocity.y;

            slingshotState = false;
        }


        // WALL CLING
        // ==========

        // If the player isn't on the ground, check if there's a wall in front of them
        if (!isGrounded && verticalVelocity <= 0f)
        {
            WallClingCheck();
        }
        else
        {
            if (clingingState)
            {
                grapplingTongueLauncher.inputLocked = false;
            }
            clingingState = false;
            previouslyClungWall = null;
        }


        // SLIDING
        // =======

        // Set the character's slidingState to true, and set its height and velocity
        if (slidingInput && !slidingState && externalVelocity == Vector3.zero && !knockbackState)
        {
            slidingState = true;
            slidingVelocity = slideStartVelocity;
            ChangeCharacterHeight(0.7f);
        }
        else if (!slidingInput && slidingState) // Set the character's slidingState to false, reset its height and start the cooldown
        {
            slidingState = false;
            slidingCooldown = 0.1f;
            ChangeCharacterHeight(1.5f);
        }

        // Progress cooldown if grounded
        if (!slidingState && slidingCooldown > 0f && isGrounded)
        {
            slidingCooldown -= Time.deltaTime;
        }

        // Handle sliding on the ground
        if (isGrounded && slidingState && slidingVelocity >= 0f && slidingCooldown <= 0f && !knockbackState)
        {
            AddExternalVelocity(frogMesh.forward * slidingVelocity * Time.deltaTime);
            
            slidingVelocity -= slidingFriction * Time.deltaTime;
        }
        // Handle sliding in the air, end slide at velocity 5 instead of 0, in order to stop an abrupt pause in the air after a slide reaches its end
        else if (slidingState && slidingVelocity >= 5f && slidingCooldown <= 0f && !clingingState && !knockbackState) 
        {
            verticalVelocity = verticalVelocity > 0f ? 0f : verticalVelocity + gravity * Time.deltaTime;

            AddExternalVelocity((frogMesh.forward * slidingVelocity * Time.deltaTime) + (Vector3.up * verticalVelocity * Time.deltaTime));

            slidingVelocity -= slidingFriction * Time.deltaTime;
        }
        // When not sliding / After a slide is done
        else
        {
            slidingVelocity = 0f;
            slidingState = false;
            slidingInput = false;
            if (characterController.height == 0.7f) // Make sure that the character's height is right when not sliding
            {
                ChangeCharacterHeight(1.5f);
            }
        }


        // JUMP, CHARGE JUMP AND WALL JUMP
        // ===============================

        // If the player lets go of the jump button 0.2 or more seconds before hitting the ground, clear the jump command, else store the command for when the player lands 
        if (jumpInputDecayTimer >= 0.2f)
        {
            chargeJumpTimer = 0f;
            jumpInputDecayTimer = 0f;
        }

        // Progress jumpInputDecayTimer
        if (!chargingJump && chargeJumpTimer > 0f)
        {
            jumpInputDecayTimer += Time.deltaTime;
        } else if (chargingJump)
        {
            // Reset jumpInputDecayTimer if player presses down jump button again
            jumpInputDecayTimer = 0f;
        }

        // Add to chargeJumpTimer if the jump button is held down
        if (chargingJump && chargeJumpTimer <= 0.5f)
        {
            chargeJumpTimer += Time.deltaTime;
        }

        Vector3 movementVector = Vector3.zero;
        Vector3 movementVectorForward = Vector3.zero;
        Vector3 movementVectorRight = Vector3.zero;

        // If the player has pressed down and released the jump button during a slide on the ground, end slide and return to normal non-externalVelocity move case
        if (!chargingJump && chargeJumpTimer > 0 && isGrounded && slidingState)
        {
            externalVelocity = Vector3.zero;
            slidingVelocity = 0f;
            slidingState = false;
            slidingInput = false;
            ChangeCharacterHeight(1.5f);
        }

        // If the player is currently in the middle of a wall jump, calculate the externalVelocity 
        if (wallJumpState)
        {
            // Cut off the externalVelocity jump before either velocity reaches 0, to avoid an abrupt stop
            if (verticalVelocity < 1f || wallJumpHorizontalVelocity < 1f)
            {
                wallJumpState = false;
                wallJumpHorizontalVelocity = 0f;
            }
            else
            {
                // Detract from both velocities over time
                wallJumpHorizontalVelocity += wallJumpDrag * Time.deltaTime;
                verticalVelocity += wallJumpGravity * Time.deltaTime;

                AddExternalVelocity((frogMesh.forward * wallJumpHorizontalVelocity * Time.deltaTime) + (Vector3.up * verticalVelocity * Time.deltaTime));
            }
        }


        // MOVEMENT
        // ========

        // Case: external velocity given -> don't calculate velocity from movement or gravity
        if (externalVelocity != Vector3.zero)
        {
            movementVector = externalVelocity * Time.deltaTime;

            // Reset the external velocity (it should only affect this frame)
            externalVelocity = Vector3.zero;
        }
        // Case: No external velocities given
        else
        {
            // If the player has reached the apex of their jump, add a multiplier to the gravity.
            // Also apply constant force downward when grounded, in case of faulty positive isGrounded
            if (clingingState)
            {
                // If the player is clinging to a wall, drag them downwards at a slower pace
                verticalVelocity += wallClingSlideSpeed * Time.deltaTime;
            }
            else
            {
                verticalVelocity = isGrounded ?
                    -5f :
                    verticalVelocity + gravity * (playerVelocity.y < 0f ? gravityMultiplierPostApex : 1) * Time.deltaTime;
            }

            // Jump if the jump button is let go of and there is any amount of charge
            if (!chargingJump && chargeJumpTimer > 0 && (isGrounded || clingingState))
            {
                if (clingingState)
                {
                    // Set velocities for the wall jump
                    verticalVelocity = wallJumpVelocity;
                    wallJumpHorizontalVelocity = wallJumpHorizontalStartVelocity;

                    // Turn the player character away from the wall they're jumping from
                    Vector3 newPos = frogMesh.position + frontWallHit.normal;
                    frogMesh.LookAt(new Vector3(newPos.x, frogMesh.transform.position.y, newPos.z));

                    AddExternalVelocity((frogMesh.forward * wallJumpHorizontalVelocity * Time.deltaTime) + (Vector3.up * verticalVelocity * Time.deltaTime));

                    wallJumpState = true;
                    clingingState = false;
                    grapplingTongueLauncher.inputLocked = false;
                    chargeJumpTimer = 0f;

                    // Animate Jump
                    animator.SetTrigger("Jump");
                }
                else
                {
                    // Depending on charge amount, decide which jump to do
                    verticalVelocity = chargeJumpTimer >= 0.5f ? chargeJumpVelocity : jumpVelocity;

                    // Animate Jump
                    animator.SetTrigger("Jump");

                    chargeJumpTimer = 0f;
                }
            }

            // Keep character constrained in terminal velocity
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity = terminalVelocity;
            }

            // Calculate player's vertical movement into a Vector3
            Vector3 verticalMovementVector = Vector3.up * verticalVelocity * Time.deltaTime;

            // Forward / backward input
            movementVectorForward = inputVerticalAxisValue * cameraLookTransform.forward * movementSpeed * Time.deltaTime;

            // Right / left input
            movementVectorRight = inputHorizontalAxisValue * cameraLookTransform.right * movementSpeed * Time.deltaTime;

            // Construct the movementVector from inputs
            movementVector = Vector3.ClampMagnitude(movementVectorForward + movementVectorRight, movementSpeed * Time.deltaTime);

            // Adjust velocity to slope
            movementVector = AdjustVelocityToSlope(movementVector);

            // Lock player's movement to be towards or away from the wall they are clinging to
            if (clingingState)
            {
                movementVector = LockMovementWhileClinging(movementVector);
            }

            // Add vertical velocity to movementVector
            movementVector += verticalMovementVector;
        }

        // Move
        characterController.Move(movementVector);

        // Animate running
        animator.SetBool("Running", (movementVectorForward + movementVectorRight).magnitude > 0f);


        // MESH ROTATION
        // =============

        // Force the frogMesh to look at the clung-to wall during a cling
        if (clingingState)
        {
            // Turn the player character away from the wall they're jumping from
            Vector3 newPos = frogMesh.position + -frontWallHit.normal;
            frogMesh.LookAt(new Vector3(newPos.x, frogMesh.transform.position.y, newPos.z));
        }
        else
        {
            // Case: zoomed -> use camera's rotation for the character's mesh
            if (isZoomed)
            {
                frogMesh.rotation = cameraLookTransform.rotation;
            }
            // Case: NOT zoomed -> Rotate the character's mesh towards movement input's direction
            else if (!wallJumpState)
            {
                frogMesh.LookAt(frogMesh.position + movementVectorForward + movementVectorRight);
            }
        }
    }

    public void SetGroundedState(bool newState)
    {
        isGrounded = newState;
    }

    // To match the platform's velocity and rotation
    public void ParentToPlatform(Transform newParent)
    {
        transform.SetParent(newParent, true);
    }

    public void ClearParent()
    {
        transform.SetParent(null, true);
    }

    public void SetIsZoomed(bool newZoomedState)
    {
        isZoomed = newZoomedState;
    }

    public void AddExternalVelocity(Vector3 additionalExternalVelocity)
    {
        // To make pausing work:
        // Divide by deltaTime here, and when calling this function multiply the external speed by deltaTime
        // NOTE: I don't know why this doesn't cause error when Time.timeScale = 0
        externalVelocity += additionalExternalVelocity / Time.deltaTime;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public Vector3 GetPlayerVelocity()
    {
        return playerVelocity;
    }

    // If the player is going up or down a slope, adjust the horizontal movement to be along the slope's normal
    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
        {
            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Vector3 adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0 || (isGrounded && adjustedVelocity.y > 0))
            {
                return adjustedVelocity;
            }
        }
        return velocity;
    }

    public void ResetPlayerMovement()
    {
        ClearParent();

        playerPreviousFramePosition = transform.position;
        playerVelocity = Vector3.zero;
        externalVelocity = Vector3.zero;
        verticalVelocity = 0f;
        isGrounded = false;

        chargingJump = false;
        chargeJumpTimer = 0f;
        jumpInputDecayTimer = 0f;

        slidingVelocity = 0f;
        slidingState = false;
        slidingInput = false;

        frogMesh.localRotation = Quaternion.identity;
    }

    private void ChangeCharacterHeight(float newHeight)
    {
        characterController.height = newHeight;
        characterController.center = Vector3.up * (characterController.height / 2f);
    }

    // Check if there's a clingable wall in front of the player character
    private void WallClingCheck()
    {
        if(Physics.SphereCast(frogMesh.position, sphereCastRadius, frogMesh.forward, out frontWallHit, wallDetectionLength))
        {
            // If the player is already clinging to a wall, keep clinging to it
            if (clingingState)
            {
                clingingState = true;
            }
            else
            {
                // Check that the player is facing the wall, and that the wall has not been clung to before
                if (Vector3.Angle(frogMesh.forward, -frontWallHit.normal) <= maxAngle && previouslyClungWall != frontWallHit.collider.gameObject && frontWallHit.collider.gameObject.CompareTag("ClingableWall"))
                {
                    clingingState = true;

                    // Stop player from shooting tongue out during a wall cling
                    grapplingTongueLauncher.ResetTongueLauncher();
                    grapplingTongueLauncher.inputLocked = true;

                    // Set previouslyClungWall so you can't cling to the same wall twice in a row before hitting the ground again
                    previouslyClungWall = frontWallHit.collider.gameObject;

                    // Teleport the player character to the point where the spherecast hit the wall
                    AddExternalVelocity(frontWallHit.point - transform.position);

                    // Stop the player's ascent or descent
                    verticalVelocity = 0f;
                }
                else
                {
                    clingingState = false;
                    grapplingTongueLauncher.inputLocked = false;
                }
            }
        } 
        else
        {
            clingingState = false;
            grapplingTongueLauncher.inputLocked = false;
        }
    }

    // Get player's movement's velocity in the direction of the clingable wall's normal, and use that instead of the original velocity
    private Vector3 LockMovementWhileClinging(Vector3 velocity)
    {
        Vector3 adjustedVelocity = ( Vector3.Dot(frontWallHit.normal, velocity) / (frontWallHit.normal.magnitude * frontWallHit.normal.magnitude) ) * frontWallHit.normal;

        return adjustedVelocity;
    }

    // Take in knockback vector from damage source and start knockback state
    public void Knockback(Vector3 knockbackVector)
    {
        knockbackVelocity = knockbackVector;

        AddExternalVelocity(knockbackVelocity * Time.deltaTime);

        knockbackState = true;

        SetGroundedState(false);

        verticalVelocity = 0f;
    }

    public void Slingshot(Vector3 slingshotVector)
    {
        slingshotVelocity = slingshotVector;

        AddExternalVelocity(slingshotVelocity * Time.deltaTime);

        slingshotState = true;

        verticalVelocity = 0f;
    }
}
