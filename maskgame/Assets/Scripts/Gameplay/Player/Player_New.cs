using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections;

public class Player_New : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    [SerializeField] private Camera _camera;

    public InputAction mouseLook, movementAction, jumpAction, sprintAction, dashAction, interact, mousePosAction;
    public Transform CameraHolder;

    private Vector2 moveInput;
    private Dictionary<string, bool> cooldowns = new();
    private float stamina;
    private const float staminaMax = 100f;
    private bool cdDash = false;
    private bool canDoubleJump;
    private float xRotation = 0f;
    private Vector2 look;
    private bool notAllInteract = false;


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
        CameraHolder = _camera.transform;
    }

    void Update()
    {
        Movement();

        if (jumpAction.WasPressedThisFrame() && !notAllInteract)
            Jump();

        LookAround();
    }


    //TODO: Fix reversed input values
    void Movement()
    {
        if (movementAction.IsPressed() && !cdDash && !notAllInteract)
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

    void LookAround(float mouseSens = 0.1f)
    {
        if (notAllInteract) return;
        look = mouseLook.ReadValue<Vector2>() * mouseSens;
        xRotation -= look.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        CameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * look.x);
    }

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

    IEnumerator NonEnteract(int layer, float secondsToWait = 10)
    {
        notAllInteract = true;
        yield return new WaitForSeconds(secondsToWait);
        notAllInteract = false;
    }

    void TakeItem()
    {
        if (interact.IsPressed())
        {
            Vector2 mousePosition = mousePosAction.ReadValue<Vector2>();
            if (Physics.Raycast(mousePosition, Camera.main.transform.forward, out RaycastHit hitItem, 20) && hitItem.collider.tag == "Item")
            {
                Destroy(hitItem.collider.gameObject);
            }
        }
    }

    #region cdUnirsality
    bool IsOnCooldown(string action)
    {
        return cooldowns.ContainsKey(action) && cooldowns[action];
    }
    async void StartCooldown(string action, float duration)
    {
        if (IsOnCooldown(action)) return;

        cooldowns[action] = true;
        await Task.Delay(TimeSpan.FromSeconds(duration));
        cooldowns[action] = false;
    }

    #endregion
}