namespace Content.Features.UnitsSystem.Scripts
{
    public interface IUnitHealth
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        
        public void TakeDamage(float amount);
        public void Heal(float amount);
        public void ResetHealth();
    }
}