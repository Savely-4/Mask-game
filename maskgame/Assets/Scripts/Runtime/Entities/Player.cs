using Runtime.Components;
using Runtime.Configs;
using Runtime.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movementComponent;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerInputKeyboardConfig _inputKeyboardConfig;
    
        [Header("Camera")]
        [SerializeField] private CameraConfig _cameraConfig;

    
        private CameraService cameraService;
        private PlayerInputKeyboardService _playerInputKeyboardService;
    
        public InputAction mouseLook, jumpAction, sprintAction, dashAction, interact, mousePosAction;
        private Vector2 _moveInput;
        private bool cdDash = false;
        private float xRotation = 0f;
        private bool canDoubleJump;
        private float _currentSlantAngle = 0f;
        private void Awake()
        {
            InitBindings();
            InitComponents();
            cameraService.SetOldRotationEulerZ(_camera.transform);
        }
    
        #region Init methods
        private void InitBindings()
        {
            interact = InputSystem.actions.FindAction("Interact");
            dashAction = InputSystem.actions.FindAction("Dash");
            jumpAction = InputSystem.actions.FindAction("Jump");
            mouseLook = InputSystem.actions.FindAction("Look");
            mousePosAction = InputSystem.actions.FindAction("MousePositions");
            sprintAction = InputSystem.actions.FindAction("Sprint");
        }
    
        private void InitComponents()
        {
            _playerInputKeyboardService = new PlayerInputKeyboardService(_inputKeyboardConfig);
            cameraService = new CameraService(_cameraConfig);
        }
        #endregion

        void Update()
        {
            UpdateCamera();
            Movement();

            if (jumpAction.WasPressedThisFrame())
                Jump();
        
        }

        private void UpdateCamera()
        {
            cameraService.Bobbing(_moveInput);
            var rotation = cameraService.Rotate(_camera.transform, _currentSlantAngle);
            cameraService.UpdateCameraPosition(_camera.transform, this.transform.position + (Vector3.up * _cameraConfig.CameraOffsetY));
            UpdatePlayerRotation(rotation);
        }

        private void UpdatePlayerRotation(Quaternion rotation)
        {
            // Debug.Log(rotation);
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, rotation.eulerAngles.y, this.transform.rotation.z);
        }

        //TODO: Fix reversed input values
        void Movement()
        {
            var moveInput = _playerInputKeyboardService.GetMovementInput();
            _moveInput = new Vector2(moveInput.y, moveInput.x);
            _movementComponent.SetMovementInput(_moveInput);
        }

        void Jump()
        {
            if (_movementComponent.IsGrounded)
            {
                _movementComponent.Jump();
                canDoubleJump = true;

                return;
            }

            if (!canDoubleJump)
                return;

            _movementComponent.Jump();
            canDoubleJump = false;
        }
    }
}
