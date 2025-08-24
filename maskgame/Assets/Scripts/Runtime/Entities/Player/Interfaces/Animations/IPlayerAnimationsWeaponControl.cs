using System;

namespace Runtime.Entities.Player
{
    /// <summary>
    /// Part of animation controller responsible for weapons
    /// </summary>
    public interface IPlayerAnimationsWeaponControl
    {
        /// <summary>
        /// Is Animator ready for next action
        /// </summary>
        bool IsIdle { get; }

        event Action MeleeStartedHitting;
        event Action MeleeStoppedHitting;

        void SetPrimaryPressed(bool value);
        void SetSecondaryPressed(bool value);

        //Set combo for attack?
        void SetInt(string name, int value);
    }
}