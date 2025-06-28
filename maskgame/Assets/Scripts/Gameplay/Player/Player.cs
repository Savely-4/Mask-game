using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections;

public class Player : MonoBehaviour
{
    //[SerializeField] public float Speed { get; private set; } = 5f;
    //[SerializeField] public float SprintSpeed { get; private set; } = 10f;
    //[SerializeField] public float JumpDistance { get; private set; } = 1.7f;

    [SerializeField] private Camera _camera;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _lerpWalkSpeed;
    [SerializeField] private float _lerpRunSpeed;
    [SerializeField] private float _jumpDistance;


    public InputAction mouseLook, movementAction, jumpAction, sprintAction, dashAction,interact,mousePosAction;
    public float CoefSpeed = 4f;
    public float DistanceDash = 5f;
    public float SpeedDash = 10f;
    public float DistanceRaycastJump = 0.75f;
    public Transform CameraHolder;
    public Transform Hands;
    public bool SwordInHands = false;
    
    private CharacterController _characterController;
    private Vector3 moveDirection;
    private Vector2 moveInput;
    private Dictionary<string, bool> cooldowns = new();
    private float stamina;
    private const float staminaMax = 100f;
    private float staminaRegenDelayTimer = 0f;
    private bool playerCantSprint;
    private Vector3 directionalDash;
    private float timeToCdDash = 4f;
    private bool cdDash = false;
    private bool onGround;
    private Vector3 originRaycastJump;
    private RaycastHit hitGroundOnRaycast;
    private bool canDoubleJump;
    private bool canJump;
    private float xRotation = 0f;
    private Vector2 look;
    private bool notAllInteract = false;

    private float _currentRunSpeed;
    private float _currentWalkSpeed;
    private float _currentSpeed;

    private float _walkTime;
    private float _sprintTime;
    private float _lastRunSpeed;
    private bool _isWasRunning;
    private float _lastWalkSpeed;

    private void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();    
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
        _currentSpeed = _currentWalkSpeed + _currentRunSpeed;
        //if(interact.IsPressed() && SwordInHands)
        //{
        //    currentWeapon = FindFirstObjectByType<Weapon>();
        //    currentWeapon?.Attack();
        //}
        //if(Mouse.current.leftButton.IsPressed()) currentWeapon?.Attack();

        //Sprint();
        //Dash();
        Movement();
        //Jump();
        LookAround();

        if (jumpAction.WasPressedThisFrame() && !notAllInteract)
            canJump = true;
    }

    void Movement()
    {
        if (movementAction.IsPressed() && !cdDash && !notAllInteract)
        {
            moveInput = movementAction.ReadValue<Vector2>();
            
            if(moveInput.magnitude > 0.0f)
            {
                if (_walkTime < 1.0f)
                    _walkTime += Time.deltaTime / _lerpWalkSpeed;
                else
                    _walkTime = 1.0f;

                _lastWalkSpeed = _currentWalkSpeed;
                _currentWalkSpeed = Mathf.Lerp(_currentWalkSpeed, _walkSpeed, _walkTime);
            } 
            else
            {
                if (_walkTime > 0.0f)
                    _walkTime -= Time.deltaTime / _lerpWalkSpeed;
                else
                    _walkTime = 0.0f;

                _currentWalkSpeed = Mathf.Lerp(0f, _lastWalkSpeed, _walkTime);
            }
            
            moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
            //
        }
        else
        {
            if (_walkTime > 0.0f)
                _walkTime -= Time.deltaTime / _lerpWalkSpeed;
            else
                _walkTime = 0.0f;

            _currentWalkSpeed = Mathf.Lerp(0f, _lastWalkSpeed, _walkTime);
        }

        _characterController.Move(_currentSpeed * Time.deltaTime * moveDirection.normalized + (this.transform.up * -9.81f));

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
        if(notAllInteract) return;
        look = mouseLook.ReadValue<Vector2>() * mouseSens;
        xRotation -= look.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        CameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * look.x);
    }
    //void Jump()
    //{
    //    originRaycastJump = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * 0.5f), transform.position.z);
    //    onGround = Physics.Raycast(originRaycastJump, Vector3.down, out hitGroundOnRaycast, DistanceRaycastJump);
    //    if (onGround) canDoubleJump = true;

    //    if (!canJump) return;

    //    if (onGround) rb.AddForce(0, configMove.JumpDistance, 0, ForceMode.Impulse);

    //    else if (canDoubleJump)
    //    {
    //        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    //        rb.AddForce(0, configMove.JumpDistance, 0, ForceMode.Impulse);
    //        canDoubleJump = false;
    //    }
    //    canJump = false;
    //}

    IEnumerator NonEnteract(int layer, float secondsToWait = 10)
    {
        notAllInteract = true;
        yield return new WaitForSeconds(secondsToWait);
        notAllInteract = false;
    }

    void TakeItem()
    {
        if(interact.IsPressed())
        {
            Vector2 mousePosition = mousePosAction.ReadValue<Vector2>();
            if(Physics.Raycast(mousePosition,Camera.main.transform.forward,out RaycastHit hitItem ,20) && hitItem.collider.tag == "Item")
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