namespace Content.Features.CurrencySystem.Scripts
{
    public interface ICurrencyManager
    {
        int GetCurrency(CurrencyType type);
        void AddCurrency(CurrencyType type, int amount);
        void SubtractCurrency(CurrencyType type, int amount);
    }
}