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
    private Vector3 playerVelocity = Vector3.zero;
    [SerializeField] private bool isGrounded;
    private bool doJump = false;

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
        // Calculate Player's velocity
        playerVelocity = (transform.position - playerPreviousFramePosition) / Time.fixedDeltaTime;
        playerPreviousFramePosition = transform.position;

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
        Vector3 movementVectorForward = Input.GetAxis("Vertical") * cameraLookTransform.forward * movementSpeed * Time.fixedDeltaTime;

        // Right / left input
        Vector3 movementVectorRight = Input.GetAxis("Horizontal") * cameraLookTransform.right * movementSpeed * Time.fixedDeltaTime;

        // Move
        Vector3 movementVector = Vector3.ClampMagnitude(movementVectorForward + movementVectorRight, movementSpeed * Time.fixedDeltaTime);
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
}
