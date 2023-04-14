using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform frogMesh;
    [SerializeField] private Transform cameraLookTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 playerVelocity = Vector3.zero; // Serialized for debugging
    [SerializeField] private bool isGrounded;
    public bool inputLocked = false;
    [SerializeField] private float gravity = -10f;

    private CharacterController characterController;
    private bool isZoomed = false;
    private Vector3 playerPreviousFramePosition;
    private bool chargingJump = false;
    private Vector3 externalVelocity = Vector3.zero;

    [Header("Movement values")]
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float maxJumpHeight = 1f;
    [SerializeField] private float maxChargeJumpHeight = 1f;
    [SerializeField] private float gravityMultiplierPostApex = 5f;
    [SerializeField] private float terminalVelocity = -5f; // Has to be a negative value
    [SerializeField] private float slidingFriction; // Higher number means the slide is faster
    [SerializeField] private float slidingLength; // How many units the slide moves the character forward

    private float chargeJumpTimer = 0f;
    private float jumpInputDecayTimer = 0f;
    private float verticalVelocity;
    private float jumpVelocity;
    private float chargeJumpVelocity;

    private bool slidingInput;
    private bool slidingState;
    private float slideStartVelocity;
    private float slidingVelocity;
    private float slidingCooldown;

    void Start()
    {
        // SlidingFriction multiplied by ten to keep the friction number small (just looks nicer)
        slidingFriction *= 10;

        characterController = gameObject.GetComponent<CharacterController>();
        playerPreviousFramePosition = transform.position;

        // Calculate jump velocity for fixed jump height, first for charged jump, then uncharged
        chargeJumpVelocity = Mathf.Sqrt(-2.0f * gravity * maxChargeJumpHeight);
        jumpVelocity = Mathf.Sqrt(-2.0f * gravity * maxJumpHeight);

        // Calculate slide start velocity
        slideStartVelocity = Mathf.Sqrt(-2.0f * -slidingFriction * slidingLength);
        slidingCooldown = 1f;
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
        // The input needs to be caught outside of FixedUpdate
        if (Input.GetButtonDown("Jump"))
        {
            chargingJump = true;
        } else if (Input.GetButtonUp("Jump"))
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
    }

    void FixedUpdate()
    {
        float inputVerticalAxisValue = Input.GetAxis("Vertical");
        float inputHorizontalAxisValue = Input.GetAxis("Horizontal");

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

        // Set the character's slidingState to true, and set its height and velocity
        if (slidingInput && !slidingState && externalVelocity == Vector3.zero)
        {
            slidingState = true;
            slidingVelocity = slideStartVelocity;
            changeCharacterHeight(0.7f);
        }
        else if (!slidingInput && slidingState) // Set the character's slidingState to false, reset its height and start the cooldown
        {
            slidingState = false;
            slidingCooldown = 0.1f;
            changeCharacterHeight(1.5f);
        }

        // Progress cooldown if grounded
        if (!slidingState && slidingCooldown > 0f && isGrounded)
        {
            slidingCooldown -= Time.fixedDeltaTime;
        }

        // Handle sliding on the ground
        if (isGrounded && slidingState && slidingVelocity >= 0f && slidingCooldown <= 0f)
        {
            AddExternalVelocity(frogMesh.forward * slidingVelocity * Time.fixedDeltaTime);
            
            slidingVelocity -= slidingFriction * Time.fixedDeltaTime;
        }
        // Handle sliding in the air, end slide at velocity 5 instead of 0, in order to stop an abrupt pause in the air after a slide reaches its end
        else if (slidingState && slidingVelocity >= 5f && slidingCooldown <= 0f) 
        {
            verticalVelocity = verticalVelocity > 0f ? 0f : verticalVelocity + gravity * Time.fixedDeltaTime;

            AddExternalVelocity((frogMesh.forward * slidingVelocity * Time.fixedDeltaTime) + (Vector3.up * verticalVelocity * Time.fixedDeltaTime));

            slidingVelocity -= slidingFriction * Time.fixedDeltaTime;
        }
        // When not sliding / After a slide is done
        else
        {
            slidingVelocity = 0f;
            slidingState = false;
            slidingInput = false;
            if (characterController.height == 0.7f) // Make sure that the character's height is right when not sliding
            {
                changeCharacterHeight(1.5f);
            }
            
        }

        // If the player lets go of the jump button 0.2 or more seconds before hitting the ground, clear the jump command, else store the command for when the player lands 
        if (jumpInputDecayTimer >= 0.2f)
        {
            chargeJumpTimer = 0f;
            jumpInputDecayTimer = 0f;
        }

        // Progress jumpInputDecayTimer
        if (!chargingJump && chargeJumpTimer > 0f)
        {
            jumpInputDecayTimer += Time.fixedDeltaTime;
        } else if (chargingJump)
        {
            // Reset jumpInputDecayTimer if player presses down jump button again
            jumpInputDecayTimer = 0f;
        }

        // Add to chargeJumpTimer if the jump button is held down
        if (chargingJump && chargeJumpTimer <= 0.5f)
        {
            chargeJumpTimer += Time.fixedDeltaTime;
        }

        Vector3 movementVector = Vector3.zero;
        Vector3 movementVectorForward = Vector3.zero;
        Vector3 movementVectorRight = Vector3.zero;

        // Calculate Player's velocity
        // NOTE: I don't know why this doesn't cause error when Time.timeScale = 0
        playerVelocity = (transform.position - playerPreviousFramePosition) / Time.fixedDeltaTime;
        playerPreviousFramePosition = transform.position;

        // If the player has pressed down and released the jump button during a slide on the ground, end slide and return to normal non-externalVelocity move case
        if (!chargingJump && chargeJumpTimer > 0 && isGrounded && slidingState)
        {
            externalVelocity = Vector3.zero;
            slidingVelocity = 0f;
            slidingState = false;
            slidingInput = false;
            changeCharacterHeight(1.5f);
        }

            // Case: external velocity given -> don't calculate velocity from movement or gravity
            if (externalVelocity != Vector3.zero)
        {
            movementVector = externalVelocity * Time.fixedDeltaTime;

            // Reset the external velocity (it should only affect this frame)
            externalVelocity = Vector3.zero;
        }
        // Case: No external velocities given
        else
        {
            // If the player has reached the apex of their jump, add a multiplier to the gravity.
            // Also apply constant force downward when grounded, in case of faulty positive isGrounded
            verticalVelocity = isGrounded ?
                -5f :
                verticalVelocity + gravity * (playerVelocity.y < 0f ? gravityMultiplierPostApex : 1) * Time.fixedDeltaTime;

            // Jump if the jump button is let go of and there is any amount of charge
            if (!chargingJump && chargeJumpTimer > 0 && isGrounded)
            {
                // Depending on charge amount, decide which jump to do
                if (chargeJumpTimer >= 0.5f)
                {
                    verticalVelocity = chargeJumpVelocity;

                    // Animate Jump
                    animator.SetTrigger("Jump");
                } else
                {
                    verticalVelocity = jumpVelocity;

                    // Animate Jump
                    animator.SetTrigger("Jump");
                }
                chargeJumpTimer = 0f;
            }

            // Keep character constrained in terminal velocity
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity = terminalVelocity;
            }

            // Calculate player's vertical movement into a Vector3
            Vector3 verticalMovementVector = Vector3.up * verticalVelocity * Time.fixedDeltaTime;

            // Forward / backward input
            movementVectorForward = inputVerticalAxisValue * cameraLookTransform.forward * movementSpeed * Time.fixedDeltaTime;

            // Right / left input
            movementVectorRight = inputHorizontalAxisValue * cameraLookTransform.right * movementSpeed * Time.fixedDeltaTime;

            // Construct the movementVector from inputs
            movementVector = Vector3.ClampMagnitude(movementVectorForward + movementVectorRight, movementSpeed * Time.fixedDeltaTime);

            // Adjust velocity to slope
            movementVector = AdjustVelocityToSlope(movementVector);

            // Add vertical velocity to movementVector
            movementVector += verticalMovementVector;
        }

        // Move
        characterController.Move(movementVector);

        // Animate running
        animator.SetBool("Running", (movementVectorForward + movementVectorRight).magnitude > 0f);

        // Case: zoomed -> use camera's rotation for the character's mesh
        if (isZoomed)
        {
            frogMesh.rotation = cameraLookTransform.rotation;
        }
        // Case: NOT zoomed -> Rotate the character's mesh towards movement input's direction
        else
        {
            frogMesh.LookAt(frogMesh.position + movementVectorForward + movementVectorRight);
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

    private void changeCharacterHeight(float newHeight)
    {
        characterController.height = newHeight;
        characterController.center = Vector3.up * (characterController.height / 2f);
    }
}
