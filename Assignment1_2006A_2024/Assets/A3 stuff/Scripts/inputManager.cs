using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.WalkingActions walking;

    private playerMovementProperties properties;
    private playerLook look;

    void Awake()
    {
        playerInput = new PlayerInput();
        walking = playerInput.Walking;
        properties = GetComponent<playerMovementProperties>();
        look = GetComponent<playerLook>();
        walking.Jump.performed += ctx => properties.Jump();
        walking.Crouch.performed += ctx => properties.Crouch();
        walking.Sprint.performed += ctx => properties.Sprint();
    }
    private void Update()
    {
        properties.movement(walking.Movement.ReadValue<Vector2>());
        look.lookAround(walking.lookAround.ReadValue<Vector2>());
    }
    //void FixedUpdate()
    //{
    //    properties.movement(walking.Movement.ReadValue<Vector2>());
    //}

    //private void LateUpdate()
    //{
    //    look.lookAround(walking.lookAround.ReadValue<Vector2>());
    //}

    private void OnEnable()
    {
        walking.Enable();
    }

    private void OnDisable()
    {
        walking.Disable();
    }
}
