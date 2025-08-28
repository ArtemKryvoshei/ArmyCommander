using Core.PoolSystem;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.CurrencySystem.Scripts.Visual
{
    public class ResourceTile : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private int amount = 1;
        
        public CurrencyType CurrencyType => currencyType;
        private ICurrencyManager _currencyManager;
        private IPlayerBackpack _backpack;

        private void Awake()
        {
            _currencyManager = ServiceLocator.Get<ICurrencyManager>();
            _backpack = ServiceLocator.Get<IPlayerBackpack>();
        }
        
        public void OnPickedInternal()
        {
            _currencyManager?.AddCurrency(currencyType, amount);
            _backpack?.AddTile(this);
        }
        
        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
    }
}