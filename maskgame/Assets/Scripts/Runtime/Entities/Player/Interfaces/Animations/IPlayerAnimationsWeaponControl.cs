namespace Runtime.Entities.Player
{
    /// <summary>
    /// Part of animation controller responsible for weapons
    /// </summary>
    public interface IPlayerAnimationsWeaponControl
    {
        //Got to idle
        //Is occupied
        bool IsIdle { get; }

        void SetPrimaryPressed(bool value);
        void SetSecondaryPressed(bool value);

        //Set combo for attack?
        void SetInt(string name, int value);
    }
}