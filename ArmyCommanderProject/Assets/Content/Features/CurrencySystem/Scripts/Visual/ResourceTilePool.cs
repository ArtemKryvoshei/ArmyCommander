using System;
using Core.EventBus;
using Core.Other;
using Core.PoolSystem;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.CurrencySystem.Scripts.Visual
{ 
    public class ResourceTilePool : MonoBehaviour
    {
        [SerializeField] private CurrencyType currencyType;
        [SerializeField] private ResourceTile prefab;
        [SerializeField] private int initialSize = 10;
        [SerializeField] private Transform poolParent;

        private IGameObjectPool<ResourceTile> _pool;
        public CurrencyType CurrencyType => currencyType;
        public IGameObjectPool<ResourceTile> Pool => _pool;
        private IPrefabFactory _prefabFactory;
        private IPoolRegistry _poolRegistry;
        private IEventBus _eventBus;

        private void Awake()
        {
            _prefabFactory = ServiceLocator.Get<IPrefabFactory>();
            _poolRegistry = ServiceLocator.Get<IPoolRegistry>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnMapContentInit>(InitCurrencyTilesPool);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnMapContentInit>(InitCurrencyTilesPool);
        }

        private void InitCurrencyTilesPool(OnMapContentInit obj)
        {
            _pool = new GameObjectPool<ResourceTile>(_prefabFactory, prefab, poolParent ?? transform);
            _pool.PreloadAsync(initialSize);
            _poolRegistry.RegisterPool(_pool);
        }
    }
}