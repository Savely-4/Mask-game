using System;
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

        private InputActions.PlayerActions PlayerActions => _inputActions.Player;


        void Awake()
        {
            _movementInput = GetComponent<IPlayerMovementInput>();
            _cameraInput = GetComponent<IPlayerCameraInput>();

            _inputActions = new InputActions();
        }


        void OnEnable()
        {
            PlayerActions.Move.performed += OnMovementPerformed;
            PlayerActions.Jump.performed += OnJumpPerformed;
            PlayerActions.Jump.canceled += OnJumpReleased;

            PlayerActions.Look.performed += OnLookPerformed;

            PlayerActions.Enable();
        }

        void OnDisable()
        {
            PlayerActions.Move.performed -= OnMovementPerformed;
            PlayerActions.Jump.performed -= OnJumpPerformed;
            PlayerActions.Jump.canceled -= OnJumpReleased;

            PlayerActions.Look.performed -= OnLookPerformed;

            PlayerActions.Disable();
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