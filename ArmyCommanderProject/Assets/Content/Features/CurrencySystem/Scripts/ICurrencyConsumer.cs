using System;

namespace Content.Features.CurrencySystem.Scripts
{
    public interface ICurrencyConsumer
    {
        public CurrencyType ConsumeType { get; }
        public int ConsumeAmount { get; }
        public int AlreadyConsumed { get; }
        
        public event Action OnConsumerChanged;
        
        public void Init(CurrencyType type, int amount);

        public void ResetProgress();
        public void ResetProgressWithDelay(float delay);
    }
}