using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform frogMesh;
    [SerializeField] private Transform cameraLookTransform;
    private CharacterController characterController;
    private bool isZoomed = false;

    private Vector3 playerPreviousFramePosition;
    private Vector3 playerVelocity = Vector3.zero;
    [SerializeField] private bool isGrounded;
    private bool doJump = false;
    private Vector3 externalVelocity = Vector3.zero;

    [Header("Movement values")]
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;

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
        Vector3 movementVector = Vector3.zero;
        Vector3 movementVectorForward = Vector3.zero;
        Vector3 movementVectorRight = Vector3.zero;

        // Calculate Player's velocity
        // NOTE: I don't know why this doesn't cause error when Time.timeScale = 0
        playerVelocity = (transform.position - playerPreviousFramePosition) / Time.fixedDeltaTime;
        playerPreviousFramePosition = transform.position;

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
            float verticalVelocity = isGrounded ?
                0f :
                playerVelocity.y + Globals.GRAVITY * Time.fixedDeltaTime;

            // Jump
            if (doJump)
            {
                doJump = false;

                if (isGrounded)
                {
                    verticalVelocity = jumpForce;
                }
            }

            // Add vertical velocity to Player's movement
            characterController.Move(Vector3.up * verticalVelocity * Time.fixedDeltaTime);

            // Forward / backward input
            movementVectorForward = Input.GetAxis("Vertical") * cameraLookTransform.forward * movementSpeed * Time.fixedDeltaTime;

            // Right / left input
            movementVectorRight = Input.GetAxis("Horizontal") * cameraLookTransform.right * movementSpeed * Time.fixedDeltaTime;

            movementVector = Vector3.ClampMagnitude(movementVectorForward + movementVectorRight, movementSpeed * Time.fixedDeltaTime);
        }

        // Move
        characterController.Move(movementVector);

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

    public void ToggleZoom(bool newZoomedState)
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
}
