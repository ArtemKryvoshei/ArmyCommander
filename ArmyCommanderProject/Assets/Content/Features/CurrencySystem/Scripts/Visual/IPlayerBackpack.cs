namespace Content.Features.CurrencySystem.Scripts.Visual
{
    public interface IPlayerBackpack
    {
        public void AddTile(ResourceTile tile);
        public bool TryGiveTile(CurrencyType type, out ResourceTile tile);
    }
}