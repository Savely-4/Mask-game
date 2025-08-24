using UnityEngine;

namespace Runtime.Entities.Player
{
    public interface IPlayerMovementInput
    {
        void SetMovementInput(Vector2 value);

        void SetJumpPressed();
        void SetJumpReleased();

        // Other things like sprint
    }
}