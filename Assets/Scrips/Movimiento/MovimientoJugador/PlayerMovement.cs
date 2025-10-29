using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    
    InputManager inputManager;
    Vector3 movedirection;
    Transform cameraTransform;
    Rigidbody playerRigidbody;
    public float playerSpeed = 7f;
    public float rotationSpeed = 15f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void HandleMovement()
    {
        movedirection = cameraTransform.forward * inputManager.movementInput.y;
        movedirection = movedirection + cameraTransform.right * inputManager.movementInput.x;
        movedirection.y = 0;
        movedirection.Normalize();
        movedirection = movedirection * playerSpeed;
        Vector3 movementVelocity = movedirection;
        playerRigidbody.linearVelocity = movementVelocity;
    }
    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraTransform.forward * inputManager.movementInput.y;
        targetDirection = movedirection + cameraTransform.right * inputManager.movementInput.x;
        targetDirection.y = 0;
        targetDirection.Normalize();
        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
}
