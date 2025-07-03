using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    public InputAction movementAction, jumpAction, sprintAction;

    private Vector2 moveInput;
    private Dictionary<string, bool> cooldowns = new();

    [SerializeField] private float staminaMax = 10f;
    private float stamina;

    [SerializeField] private int maxJumps = 2;
    private int jumpsLeft;


    private void Awake()
    {
        jumpAction = InputSystem.actions.FindAction("Jump");
        movementAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void Start()
    {
        stamina = staminaMax;
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        PerformMovementControl();
        PerformJumpsControl();
        PerformSprintControl();
    }


    //TODO: Fix reversed input values
    void PerformMovementControl()
    {
        moveInput = movementAction.ReadValue<Vector2>();
        moveInput = new Vector2(moveInput.y, moveInput.x);

        if (movementAction.IsPressed())
        {
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

    void PerformSprintControl()
    {
        stamina = Mathf.Clamp(stamina, 0f, staminaMax);

        //Otherwise, if not standing still and sprint pressed - sprint
        if (sprintAction.IsPressed() && moveInput.sqrMagnitude != 0)
        {
            stamina -= Time.deltaTime;

            movement.ToggleSprint(stamina > 0);

            return;
        }

        stamina += Time.deltaTime;
        movement.ToggleSprint(false);
    }

    void PerformJumpsControl()
    {
        if (movement.IsGrounded)
        {
            jumpsLeft = maxJumps;
        }

        if (jumpAction.WasPressedThisFrame() && jumpsLeft > 0)
        {
            movement.Jump();
            jumpsLeft--;
        }

        if (jumpAction.WasReleasedThisFrame())
            movement.StopJump();
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