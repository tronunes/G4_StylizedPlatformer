using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Technical")]
    [SerializeField] private Transform frogMesh;
    [SerializeField] private Transform cameraLookTransform;
    private CharacterController characterController;

    [Header("Movement values")]
    [SerializeField] private float movementSpeed = 6f;

    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Fall movement
        characterController.Move(
            -Vector3.up * characterController.velocity.y +
            (characterController.isGrounded ?
                Vector3.zero :
                Vector3.up * Globals.GRAVITY * Time.deltaTime)
        );

        // Forward / backward input
        Vector3 movementVectorForward = Input.GetAxis("Vertical") * cameraLookTransform.forward * movementSpeed * Time.deltaTime;

        // Right / left input
        Vector3 movementVectorRight = Input.GetAxis("Horizontal") * cameraLookTransform.right * movementSpeed * Time.deltaTime;

        // Up down movement
        Vector3 movementVectorUp = characterController.isGrounded ? Vector3.zero : Vector3.up * Globals.GRAVITY;

        // Move
        Vector3 movementVector = Vector3.ClampMagnitude(movementVectorForward + movementVectorRight, movementSpeed * Time.deltaTime);
        characterController.Move(movementVector);

        // Rotate the character's mesh towards movement input's direction
        frogMesh.LookAt(frogMesh.position + movementVectorForward + movementVectorRight);
    }
}
