using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerMovement playerMovement;
    public PlayerController controls;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        inputManager.HandleAllMovement();
    }

    private void FixedUpdate()
    {
        playerMovement.HandleAllMovement();
    }
    // === Lectura de Inputs ===
    public bool InteractPressed()
    {
        return controls.Gameplay.Interaction.WasPressedThisFrame();
    }

    public bool InventoryPressed()
    {
        return controls.Gameplay.Inventory.WasPressedThisFrame();
    }
}
