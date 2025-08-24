namespace Runtime.Entities.Player
{
    /// <summary>
    /// Part of animation controller responsible for movement
    /// </summary>
    public interface IPlayerAnimationsMovementControl
    {
        void SetRelativeSpeed(float forward, float Strafe);
        void ToggleAirborne(bool value);
        void TriggerJump();
    }
}