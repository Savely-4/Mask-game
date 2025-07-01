using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Camera _camera;

    public InputAction mouseLook, movementAction, jumpAction, sprintAction, dashAction, interact, mousePosAction;

    private Vector2 moveInput;
    private float stamina;
    private const float staminaMax = 100f;
    private bool cdDash = false;
    private bool canDoubleJump;
    private float xRotation = 0f;
    
    private void Awake()
    {
        interact = InputSystem.actions.FindAction("Interact");
        dashAction = InputSystem.actions.FindAction("Dash");
        jumpAction = InputSystem.actions.FindAction("Jump");
        mouseLook = InputSystem.actions.FindAction("Look");
        movementAction = InputSystem.actions.FindAction("Move");
        mousePosAction = InputSystem.actions.FindAction("MousePositions");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        stamina = staminaMax;
    }

    void Update()
    {
        Movement();

        if (jumpAction.WasPressedThisFrame())
            Jump();

    }


    //TODO: Fix reversed input values
    void Movement()
    {
        if (movementAction.IsPressed() && !cdDash)
        {
            moveInput = movementAction.ReadValue<Vector2>();
            moveInput = new Vector2(moveInput.y, moveInput.x);
            movement.SetMovementInput(moveInput);
        }
        else
        {
            movement.SetMovementInput(Vector2.zero);
        }
    }

    //void Dash()
    //{
    //    if (IsOnCooldown("Dash") || notAllInteract) return;

    //    if (dashAction.IsPressed() && !cdDash && movementAction.IsPressed())
    //    {
    //        directionalDash = transform.position + new Vector3(moveInput.x,moveInput.y,0) * DistanceDash;
    //        directionalDash.y = transform.position.y;
    //        Vector3 vector = Vector3.MoveTowards(transform.position, directionalDash, SpeedDash * Time.deltaTime);
    //        rb.MovePosition(vector);
    //        cdDash = true;
    //    }
    //    else if(!cdDash && dashAction.IsPressed())
    //    {
    //        directionalDash = transform.position + transform.forward * DistanceDash;
    //        directionalDash.y = transform.position.y;
    //        Vector3 vector = Vector3.MoveTowards(transform.position, directionalDash, SpeedDash * Time.deltaTime);
    //        rb.MovePosition(vector);
    //        cdDash = true;
    //    }
    //    else if (cdDash)
    //    {
    //        StartCooldown("Dash", 4f);
    //        cdDash = false;
    //    }
    //}

    //void Sprint()
    //{
    //    if (notAllInteract) return;

    //    if(stamina <= 0 && !IsOnCooldown("Sprint") && !playerCantSprint)
    //    {
    //        stamina = 0;
    //        playerCantSprint = true;
    //        StartCooldown("Spint", 5f);
    //    }
    //    if (playerCantSprint && stamina >= staminaMax * 0.3f) playerCantSprint = false;

    //    if(sprintAction.IsPressed() && movementAction.IsPressed() && !IsOnCooldown("Sprint") && !playerCantSprint && stamina > 0)
    //    {
    //        stamina -= 20f * Time.deltaTime;

    //        if (stamina < 0) stamina = 0;

    //        if (CurrentSpeed < configMove.SprintSpeed) CurrentSpeed += CoefSpeed * Time.deltaTime;
    //        staminaRegenDelayTimer = 1.5f;
    //    }
    //    else
    //    {
    //        if(staminaRegenDelayTimer > 0f) staminaRegenDelayTimer -= Time.deltaTime;
    //        else if (stamina < staminaMax) stamina += 10f * Time.deltaTime;

    //        if(CurrentSpeed > configMove.Speed) CurrentSpeed -= CoefSpeed * Time.deltaTime;

    //    }
    //    CurrentSpeed = Mathf.Clamp(CurrentSpeed, configMove.Speed, configMove.SprintSpeed);
    //    Mathf.Clamp(stamina, 0f, staminaMax);
    //}

    void Jump()
    {
        if (movement.IsGrounded)
        {
            movement.Jump();
            canDoubleJump = true;

            return;
        }

        if (!canDoubleJump)
            return;

        movement.Jump();
        canDoubleJump = false;
    }
}
