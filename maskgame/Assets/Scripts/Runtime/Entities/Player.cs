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
        [Header("Stamina")]
        [SerializeField] private StaminaConfig _staminaConfig;

    
        private CameraComponent _cameraComponent;
        private StaminaService _staminaService;
        private PlayerInputKeyboardService _playerInputKeyboardService;
    
        public InputAction mouseLook, sprintAction, dashAction, interact, mousePosAction;
        private Vector2 _moveInput;
        private bool cdDash = false;
        private float xRotation = 0f;
        private bool canDoubleJump;
        private float _currentSlantAngle = 0f;
        
        private void Awake()
        {
            InitBindings();
            InitComponents();
            _cameraComponent.SetOldRotationEulerZ(_camera.transform);
        }
    
        #region Init methods
        private void InitBindings()
        {
            interact = InputSystem.actions.FindAction("Interact");
            dashAction = InputSystem.actions.FindAction("Dash");
            mouseLook = InputSystem.actions.FindAction("Look");
            mousePosAction = InputSystem.actions.FindAction("MousePositions");
            sprintAction = InputSystem.actions.FindAction("Sprint");
        }
    
        private void InitComponents()
        {
            _playerInputKeyboardService = new PlayerInputKeyboardService(_inputKeyboardConfig);
            _cameraComponent = new CameraComponent(_cameraConfig);
            _staminaService = new StaminaService(_staminaConfig);
        }
        #endregion

        void Update()
        {
            UpdateCamera();
            PerformMovementControl();
            PerformJumpsControl();
            PerformSprintControl();
        }

        private void UpdateCamera()
        {
            _cameraComponent.Bobbing(_moveInput);
            var rotation = _cameraComponent.Rotate(_camera.transform, _currentSlantAngle);
            _cameraComponent.UpdateCameraPosition(_camera.transform, this.transform.position + (Vector3.up * _cameraConfig.CameraOffsetY));
            UpdatePlayerRotation(rotation);
        }

        private void UpdatePlayerRotation(Quaternion rotation)
        {
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, rotation.eulerAngles.y, this.transform.rotation.z);
        }

        //TODO: Fix reversed input values
        void PerformMovementControl()
        {
            var moveInput = _playerInputKeyboardService.GetMovementInput();
            _moveInput = new Vector2(moveInput.y, moveInput.x);
            _movementComponent.SetMovementInput(_moveInput);
        }
        
        void PerformSprintControl()
        {
            //Otherwise, if not standing still and sprint pressed - sprint
            if (_playerInputKeyboardService.SprintButtonPressed(false) && _moveInput.sqrMagnitude != 0)
            {
                _movementComponent.ToggleSprint();
                return;
            }
            _movementComponent.UnToggleSprint();
        }

        void PerformJumpsControl()
        {
            _movementComponent.ResetJumps();

            if (_playerInputKeyboardService.JumpButtonPressedThisFrame())
                _movementComponent.PerformJump();

            if (_playerInputKeyboardService.JumpButtonReleasedThisFrame())
                _movementComponent.StopPerformJump();
        }
    }
}
