using Runtime.Components;
using Runtime.Configs;
using Runtime.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using CameraService = Runtime.Components.CameraService;

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

    
        private CameraService _cameraService;
        private StaminaService _staminaService;
        private PlayerInputKeyboardService _playerInputKeyboardService;
    
        private Vector2 _moveInput;
        private bool cdDash = false;
        private float xRotation = 0f;
        private bool canDoubleJump;
        private float _currentSlantAngle = 0f;
        
        private void Awake()
        {
            InitComponents();
            _cameraService.SetOldRotationEulerZ(_camera.transform);
        }
        
    #region Init
        private void InitComponents()
        {
            _playerInputKeyboardService = new PlayerInputKeyboardService(_inputKeyboardConfig);
            _cameraService = new CameraService(_cameraConfig);
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
            _cameraService.Bobbing(_moveInput);
            var rotation = _cameraService.Rotate(_camera.transform, _currentSlantAngle);
            _cameraService.UpdateCameraPosition(_camera.transform, this.transform.position + (Vector3.up * _cameraConfig.CameraOffsetY));
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
