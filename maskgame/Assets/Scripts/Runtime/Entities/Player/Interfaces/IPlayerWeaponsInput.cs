namespace Runtime.Entities.Player
{
    public interface IPlayerWeaponsInput
    {
        void SetPrimaryPressed();
        void SetPrimaryReleased();

        void SetSecondaryPressed();
        void SetSecondaryReleased();

        //Switch weapon input
    }
}