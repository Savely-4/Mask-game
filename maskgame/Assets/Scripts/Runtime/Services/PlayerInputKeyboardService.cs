using Runtime.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Services
{
    public class PlayerInputKeyboardService
    {
        private readonly PlayerInputKeyboardConfig _config;
        private InputAction _movementAction;
    
        public PlayerInputKeyboardService(PlayerInputKeyboardConfig config)
        {
            _config = config;
            _movementAction = InputSystem.actions.FindAction("Move");
        }
    
        public bool PrimaryAttackButtonPressed(bool retentionSupport) 
        {
            if (retentionSupport) 
                return Input.GetKeyDown(_config.PrimaryAttackKey);
            
            else 
                return Input.GetKey(_config.PrimaryAttackKey);
        }
    
        public bool AlternateAttackKeyPressed(bool retentionSupport) 
        {
            if (retentionSupport) 
                return Input.GetKeyDown(_config.AlternateAttackKey);
            
            else 
                return Input.GetKey(_config.AlternateAttackKey);
        }

        public Vector2 GetMovementInput()
        {
            if (IsMovementPressed())
                return _movementAction.ReadValue<Vector2>();
            else
                return Vector2.zero;
        }

        public bool IsMovementPressed()
        {
            return _movementAction.IsPressed();
        }
    }
}
