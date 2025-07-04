using Runtime.Configs;

namespace Runtime.Services
{
    public class StaminaService
    {
        private float _currentStamina;
        private StaminaConfig _staminaConfig;

        public float Stamina => _currentStamina;

        public StaminaService(StaminaConfig config)
        {
            _staminaConfig = config;
        }

        public void ReduceStamina()
        {
            
        }

        public void RegenerateStamina()
        {
            
        }
    }
}