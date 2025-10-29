using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InputManager : MonoBehaviour
{
    PlayerController playerController;
    public Vector2 movementInput;
    private float verticalInput;
    private float horizontalInput;

    private void OnEnable()
    {
        if (playerController == null)
        {
            playerController = new PlayerController();

            playerController.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }
    private void HandleMovementInput()
            {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }
    public void HandleAllMovement()
    {
        HandleMovementInput();
    }
}
