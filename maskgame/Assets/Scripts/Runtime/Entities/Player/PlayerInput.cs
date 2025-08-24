using System;
using Runtime.Entities.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Entities.Player
{
    //Get all input interfaces
    //Connect to player input control scheme
    //Deliver input when changed
    //Provide ability to enable/disable part of controls?
    public class PlayerInput : MonoBehaviour
    {
        private InputActions _inputActions;

        private IPlayerMovementInput _movementInput;
        private IPlayerCameraInput _cameraInput;
        private IPlayerWeaponsInput _weaponInput;

        private InputActions.PlayerActions PlayerActions => _inputActions.Player;


        void Awake()
        {
            _movementInput = GetComponent<IPlayerMovementInput>();
            _cameraInput = GetComponent<IPlayerCameraInput>();
            _weaponInput = GetComponent<IPlayerWeaponsInput>();

            _inputActions = new InputActions();
        }


        void OnEnable()
        {
            PlayerActions.Move.performed += OnMovementPerformed;
            PlayerActions.Look.performed += OnLookPerformed;

            PlayerActions.Jump.performed += OnJumpPerformed;
            PlayerActions.Jump.canceled += OnJumpReleased;

            PlayerActions.Primary.performed += OnPrimaryPerformed;
            PlayerActions.Primary.canceled += OnPrimaryReleased;

            PlayerActions.Enable();
        }

        void OnDisable()
        {
            PlayerActions.Move.performed -= OnMovementPerformed;
            PlayerActions.Look.performed -= OnLookPerformed;

            PlayerActions.Jump.performed -= OnJumpPerformed;
            PlayerActions.Jump.canceled -= OnJumpReleased;

            PlayerActions.Primary.performed += OnPrimaryPerformed;
            PlayerActions.Primary.canceled += OnPrimaryReleased;

            PlayerActions.Disable();
        }



        private void OnPrimaryPerformed(InputAction.CallbackContext context)
        {
            _weaponInput.SetPrimaryPressed();
        }

        private void OnPrimaryReleased(InputAction.CallbackContext context)
        {
            _weaponInput.SetPrimaryReleased();
        }


        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            _cameraInput.SetCameraInput(value);
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            _movementInput.SetMovementInput(value);
        }


        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            _movementInput.SetJumpPressed();
        }

        private void OnJumpReleased(InputAction.CallbackContext context)
        {
            _movementInput.SetJumpReleased();
        }
    }
}