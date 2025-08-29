namespace Content.Features.UpgradeSystem.Scripts
{
    public interface IUpgradeable
    {
        public int GetUpgradeTier();
        public void Upgrade();
    }
}