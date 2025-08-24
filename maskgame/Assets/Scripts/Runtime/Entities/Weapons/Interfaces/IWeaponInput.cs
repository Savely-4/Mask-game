namespace Runtime.Entities.Weapons
{
    public interface IWeaponInput
    {
        //Get animation controller
        //Set Animation controls for animation start


        void OnPrimaryPressed();
        void OnPrimaryReleased();

        void OnSecondaryPressed();
        void OnSecondaryReleased();
    }
}