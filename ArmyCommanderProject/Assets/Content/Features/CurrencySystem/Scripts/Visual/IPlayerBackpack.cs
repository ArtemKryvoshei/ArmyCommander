namespace Content.Features.CurrencySystem.Scripts.Visual
{
    public interface IPlayerBackpack
    {
        public void AddTile(ResourceTile tile);
        public bool GiveTile(CurrencyType type);
        public bool GiveTile(CurrencyType type, int amount);
    }
}