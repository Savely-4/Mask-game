namespace Runtime.Entities.Weapons
{
    /// <summary>
    /// Handle for object that simulates melee strike and can return objects hit by that strike
    /// </summary>
    public interface IMeleeStrikeHandle
    {
        /// <summary>
        /// Perform wide swipe attack
        /// </summary>
        void PerformSwipe(float angleOffset, float angleWidth);

        /// <summary>
        /// Stops current simulated attack
        /// </summary>
        void Stop();
    }
}