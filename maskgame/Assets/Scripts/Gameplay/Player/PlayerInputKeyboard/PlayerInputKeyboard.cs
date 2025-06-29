using UnityEngine;

public class PlayerInputKeyboard
{
    private readonly PlayerInputKeyboardConfig _config;
    
    public PlayerInputKeyboard(PlayerInputKeyboardConfig config)
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
}
