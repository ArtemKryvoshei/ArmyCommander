using UnityEngine;

namespace Content.Features.UpgradeSystem.Scripts
{
    public class UpgradeCaller : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _upgradeableScripts;
        
        public void CallUpgrade()
        {
            foreach (var scr in _upgradeableScripts)
            {
                IUpgradeable upgradeable = scr as IUpgradeable;
                if (upgradeable != null)
                {
                    upgradeable.Upgrade();
                }
            }
        }
    }
}