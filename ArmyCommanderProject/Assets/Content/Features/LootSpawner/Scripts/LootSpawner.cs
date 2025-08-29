using System.Collections.Generic;
using Content.Features.CurrencySystem.Scripts.Visual;
using Content.Features.UnitsSystem.Scripts;
using Core.EventBus;
using Core.Other;
using Core.PoolSystem;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Content.Features.LootSpawner.Scripts
{
    public class LootSpawner : MonoBehaviour
    {
        private IEventBus _eventBus;
        private IPoolRegistry _poolRegistry;
        
        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _poolRegistry = ServiceLocator.Get<IPoolRegistry>();

            _eventBus.Subscribe<UnitDiedEvent>(OnUnitDied);
        }
        
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<UnitDiedEvent>(OnUnitDied);
        }
        
        private void OnUnitDied(UnitDiedEvent evt)
        {
            if (evt.Unit == null) return;

            var stats = evt.Unit.GetStats();
            if (stats == null || stats.currencyRewards == null) return;

            SpawnLoot(evt.Unit, stats.currencyRewards).Forget();
        }
        
        private async UniTaskVoid SpawnLoot(IUnit unit, CurrencyReward[] rewards)
        {
            Vector3 spawnPosition = Vector3.zero;
            
            if (unit is MonoBehaviour mb)
                spawnPosition = mb.transform.position;

            foreach (var reward in rewards)
            {
                if (_poolRegistry.GetAllPoolsOfType<ResourceTile>() is IEnumerable<IGameObjectPool<ResourceTile>> pools)
                {
                    foreach (var pool in pools)
                    {
                        if (pool.Prefab.CurrencyType == reward.currencyType)
                        {
                            for (int i = 0; i < reward.amount; i++)
                            {
                                var tile = await pool.GetAsync(spawnPosition, Quaternion.identity);
                                tile.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}