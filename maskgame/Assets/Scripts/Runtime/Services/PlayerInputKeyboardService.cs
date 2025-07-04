using Runtime.Configs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Services
{
    public class PlayerInputKeyboardService
    {
        private readonly PlayerInputKeyboardConfig _config;
    
        public PlayerInputKeyboardService(PlayerInputKeyboardConfig config)
        {
            _config = config;
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

        public Vector2 GetCameraInput()
        {
            return new Vector2(Input.GetAxis(_config.MouseXAxis), Input.GetAxis(_config.MouseYAxis));
        }
        
        public bool SprintButtonPressed(bool retentionSupport)
        {
            if(retentionSupport)
                return Input.GetKeyDown(_config.SprintKey);
            else
                return Input.GetKey(_config.SprintKey);
        }

        public bool JumpButtonPressedThisFrame()
        {
            return Input.GetKeyDown(_config.JumpKey);
        }

        public bool JumpButtonReleasedThisFrame()
        {
            return Input.GetKeyUp(_config.JumpKey);
        }

        public Vector2 GetMovementInput()
        {
            if (IsMovementPressed())
                return new Vector2(Input.GetAxis(_config.HorizontalAxis), Input.GetAxis(_config.VerticalAxis)).normalized;
            else
                return Vector2.zero;
        }

        public bool IsMovementPressed()
        {
            return Input.GetAxis(_config.HorizontalAxis) != 0 || Input.GetAxis(_config.VerticalAxis) != 0;
        }
    }
}
