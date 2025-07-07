using Runtime.Configs;

namespace Runtime.InteractSystem 
{
    public class PlayerInteractor : Interactor
    {
        private readonly PlayerItemInteractorConfig _playerItemInteractorConfig;
        
        public ItemInteractor ItemInteractor { get; private set; }
        public PlayerInteractor(PlayerItemInteractorConfig playerItemInteractorConfig)
        {
            _playerItemInteractorConfig = playerItemInteractorConfig;


            InitializeInteractors();
        }
        
        
        private void InitializeInteractors() 
        {
            ItemInteractor = new PlayerItemInteractor(_playerItemInteractorConfig)
            {

            };
            
            
        }
    }
}

