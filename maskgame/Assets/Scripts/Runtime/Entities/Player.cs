using Runtime.Components;
using Runtime.Configs;
using Runtime.InventorySystem;
using Runtime.Services;
using UnityEngine;
using Runtime.Entities.WeaponSystem;
using Runtime.Services.InteractSystem;
using CameraService = Runtime.Components.CameraService;
using Runtime.Services.CombatSystem;

namespace Runtime.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movementComponent;
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerInputKeyboardConfig _inputKeyboardConfig;
        [SerializeField] private PlayerItemInteractorConfig _playerItemInteractorConfig;
        [SerializeField] private InventoryConfig _playerInventoryConfig;
        [SerializeField] private WeaponCombatPresenterConfig _weaponCombatPresenterConfig;
        
        [Header("Camera")]
        [SerializeField] private CameraConfig _cameraConfig;
        [Header("Stamina")]
        [SerializeField] private StaminaConfig _staminaConfig;

        [SerializeField] private IPickableItem _currentItemInHands;

        private CameraService _cameraService;
        private StaminaService _staminaService;
        private PlayerInputKeyboardService _input;
        private WeaponCombatPresenter _weaponCombatPresenter;
        private PlayerInteractor _playerInteractor;
        private Inventory _inventory;
        private PlayerItemHolder _playerItemHolder;
        
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
            var animator = GetComponent<Animator>();    
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        
            _input = new PlayerInputKeyboardService(_inputKeyboardConfig);
            _cameraService = new CameraService(_cameraConfig);
            _staminaService = new StaminaService(_staminaConfig);
            
            _playerInteractor = new PlayerInteractor(_playerItemInteractorConfig);
            _inventory = new Inventory(_playerInventoryConfig);
            _weaponCombatPresenter = new WeaponCombatPresenter(_weaponCombatPresenterConfig, animator);
            _playerItemHolder = new PlayerItemHolder(transform);

            _playerInteractor.ItemInteractor.OnPickupItem += _inventory.AddItemInSlot;
            _playerInteractor.ItemInteractor.OnTryDropItem += TryDropItem; 

            _inventory.OnChangeCurrentSelectedSlot += OnChangeCurrentSelectedSlot;
            
            
            // Ремуваем из оружия (даже если там нету оружия, то ничего не пройзойдет) и выбрасываем из руки текущее оружие
            /*/_inventory.OnDroppedItemInSlot += () =>
            {
                _weaponCombatPresenter.SetNewWeapon(null);
                _playerItemHolder.DropView();
            };/*/
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
        
        private bool TryDropItem() 
        {
            if (_inventory.TryRemoveItemInCurrentSlot()) 
            {
                _playerItemHolder.DropView();
            }
            
            return false;
        }
        private void OnChangeCurrentSelectedSlot(int slotIndex) 
        {
            IPickableItem pickableItem = _inventory.GetItemInSlotAt(slotIndex);
   
            Weapon weapon = pickableItem as Weapon;
            Rigidbody rigidbodyItem = pickableItem as Rigidbody;
            
            _weaponCombatPresenter.SetNewWeapon(weapon);
            _playerItemHolder.PickupView(rigidbodyItem);
        }
        
        
        private void PerformInteractorControl() 
        {
            Vector3 pos = _camera.ScreenPointToRay(Input.mousePosition).origin;
            Vector3 dir = _camera.ScreenPointToRay(Input.mousePosition).direction;
        
            _playerInteractor.ItemInteractor.PickupUpdate(pos, dir, _input.PickupButtonPressed());
            _playerInteractor.ItemInteractor.DropUpdate(_input.DropButtonPressed());
        }
        
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
