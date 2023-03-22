using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform frogMesh;
    [SerializeField] private Transform cameraLookTransform;
    private CharacterController characterController;

    private Vector3 playerPreviousFramePosition;
    [SerializeField] private Vector3 playerVelocity = Vector3.zero; // Serialized for debugging
    [SerializeField] private bool isGrounded;
    private bool doJump = false;

    [Header("Movement values")]
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravityMultiplierPreApex = 2f;
    [SerializeField] private float gravityMultiplierPostApex = 5f;
    [SerializeField] private float terminalVelocity = -5f; // Has to be a negative value
    private float verticalVelocity;

    // New, add comments later


    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        playerPreviousFramePosition = transform.position;
    }

    void Update()
    {
        // Jump
        // The input needs to be caught outside of FixedUpdate
        if (Input.GetButtonDown("Jump"))
        {
            doJump = true;
        }
    }

    void FixedUpdate()
    {
        // Calculate Player's velocity
        playerVelocity = (transform.position - playerPreviousFramePosition) / Time.fixedDeltaTime;
        playerPreviousFramePosition = transform.position;

        // If the player has reached the apex of their jump, add a larger multiplier to the gravity
        verticalVelocity = isGrounded ?
            -5f :
            playerVelocity.y + Physics.gravity.y * (playerVelocity.y < 0f ? gravityMultiplierPostApex : gravityMultiplierPreApex) * Time.fixedDeltaTime;

        // Jump
        if (doJump)
        {
            doJump = false;

            if (isGrounded)
            {
                verticalVelocity = jumpForce;
            }
        }

        // Keep character constrained in terminal velocity
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity = terminalVelocity;
        }

        // Calculate player's vertical movement into a Vector3
        Vector3 verticalMovementVector = Vector3.up * verticalVelocity * Time.fixedDeltaTime;

        // Forward / backward input
        Vector3 movementVectorForward = Input.GetAxis("Vertical") * cameraLookTransform.forward * movementSpeed * Time.fixedDeltaTime;

        // Right / left input
        Vector3 movementVectorRight = Input.GetAxis("Horizontal") * cameraLookTransform.right * movementSpeed * Time.fixedDeltaTime;

        // Move
        Vector3 movementVector = Vector3.ClampMagnitude(movementVectorForward + movementVectorRight, movementSpeed * Time.fixedDeltaTime);

        // Adjust velocity to slope
        movementVector = AdjustVelocityToSlope(movementVector);

        // Add Vertical velocity to movementVector
        movementVector += verticalMovementVector;

        characterController.Move(movementVector);

        // Rotate the character's mesh towards movement input's direction
        frogMesh.LookAt(frogMesh.position + movementVectorForward + movementVectorRight);
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

    // If the player is going up or down a slope, adjust the horizontal movement to be along the slope's normal
    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0 || (isGrounded && adjustedVelocity.y > 0))
            {
                return adjustedVelocity;
            }
        }
        return velocity;
    }
}
