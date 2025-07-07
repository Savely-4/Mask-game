using Runtime.Components;
using Runtime.Configs;
using Runtime.InteractSystem;//danil
using Runtime.InventorySystem;//danil
using Runtime.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using WeaponSystem;
using CameraService = Runtime.Components.CameraService;

namespace Runtime.Entities
{
    public class Player : MonoBehaviour
    {
        #region fields_Test_Danil
        [SerializeField] private PlayerItemInteractorConfig _playerItemInteractorConfig;
        
        [SerializeField] private InventoryConfig _playerInventoryConfig;
        
        private WeaponCombatSystem _weaponCombatSystem;
        private PlayerInteractor _playerInteractor;
        private Inventory _inventory;
        private PlayerItemHolder _playerItemHolder;
        
        
        #endregion
        [SerializeField] private PlayerMovement _movementComponent;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerInputKeyboardConfig _inputKeyboardConfig;

        [Header("Camera")]
        [SerializeField] private CameraConfig _cameraConfig;
        [Header("Stamina")]
        [SerializeField] private StaminaConfig _staminaConfig;

        [SerializeField] private Weapon _weapon;

        private CameraService _cameraService;
        private StaminaService _staminaService;
        private PlayerInputKeyboardService _input;

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
            _input = new PlayerInputKeyboardService(_inputKeyboardConfig);
            _cameraService = new CameraService(_cameraConfig);
            _staminaService = new StaminaService(_staminaConfig);
            _playerInteractor = new PlayerInteractor(_playerItemInteractorConfig); //danil
            _inventory = new Inventory(_playerInventoryConfig); // danil
            _weaponCombatSystem = new WeaponCombatSystem(); //danil
            _playerItemHolder = new(transform);

            _playerInteractor.ItemInteractor.OnPickupItem += OnPickupItem;
            _playerInteractor.ItemInteractor.OnDropItem += OnDropItem;
        }
        #endregion

        void Update()
        {
            UpdateCamera();
            PerformMovementControl();
            PerformJumpsControl();
            PerformSprintControl();
            PerformInteractorControl();
        }
        
    #region Methods_by_Danil
        private void PerformInteractorControl() 
        {
            _playerInteractor.ItemInteractor.PickupUpdate(transform.position, _input.PickupButtonPressed());
            _playerInteractor.ItemInteractor.DropUpdate(transform.position, _input.DropButtonPressed());
        }
        
        
        private void OnPickupItem(IPickableItem pickableItem) 
        {
            if (_inventory.TryAddItemInSlot(pickableItem.ItemData)) 
            {
                _playerItemHolder.Equip(pickableItem);
            }
            
            //_weaponCombatSystem.SetNewWeapon(pickableItem as Weapon);
        }
        
        private void OnDropItem(IPickableItem pickableItem) 
        {
            //_inventory.TryRemoveItemSlot(pickableItem);
            //_weaponCombatSystem.SetNewWeapon(null);
        }
        
    #endregion
        private void UpdateCamera()
        {
            _cameraService.Bobbing(_moveInput);
            var rotation = _cameraService.Rotate(_camera.transform, _currentSlantAngle, _input.GetCameraInput());
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
            var moveInput = _input.GetMovementInput();
            _moveInput = new Vector2(moveInput.y, moveInput.x);
            _movementComponent.SetMovementInput(_moveInput);
        }

        void PerformSprintControl()
        {
            if (_input.SprintButtonPressed(false) && _moveInput.sqrMagnitude != 0)
            {
                _movementComponent.ToggleSprint(true);
                return;
            }
            _movementComponent.ToggleSprint(false);
        }

        void PerformJumpsControl()
        {
            if (_input.JumpButtonPressedThisFrame())
                _movementComponent.PerformJump();

            if (_input.JumpButtonReleasedThisFrame())
                _movementComponent.StopPerformJump();
        }
    }
}
