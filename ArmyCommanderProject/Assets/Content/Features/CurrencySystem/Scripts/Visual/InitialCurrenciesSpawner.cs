using System.Collections.Generic;
using Core.EventBus;
using Core.Other;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.CurrencySystem.Scripts.Visual
{
    [System.Serializable]
    public struct ResourceSpawnData
    {
        public CurrencyType currencyType;
        public int amount;
    }
    
    public class InitialCurrenciesSpawner : MonoBehaviour
    {
        [SerializeField] private List<ResourceSpawnData> initialResources;
        [SerializeField] private float stackOffsetY = 0.26f;
        
        private Transform startCurrencySpawnPoint;
        private IPoolRegistry _poolRegistry;
        private IEventBus _eventBus;

        private void Awake()
        {
            _poolRegistry = ServiceLocator.Get<IPoolRegistry>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnMapContentInit>(InitSpawnPoint);
            _eventBus.Subscribe<OnPlayerSpawned>(_ => CallResourceSpawn());
        }

        private void InitSpawnPoint(OnMapContentInit obj)
        {
            startCurrencySpawnPoint = obj.activeMap.GetStartCurrencySpawnPoint();
        }

        private async void CallResourceSpawn()
        {
            await SpawnInitialResources();
        }

        private async UniTask SpawnInitialResources()
        {
            Vector3 basePosition = startCurrencySpawnPoint.position;
            int spawnedCount = 0;

            foreach (var res in initialResources)
            {
                for (int i = 0; i < res.amount; i++)
                {
                    ResourceTile tile = await GetTileFromPool(res.currencyType);
                    if (tile == null)
                    {
                        Debug.LogWarning($"[InitialResourcesSpawner] Pool not found {res.currencyType}");
                        continue;
                    }
                    
                    tile.transform.position = basePosition + new Vector3(0, spawnedCount * stackOffsetY, 0);
                    tile.transform.rotation = Quaternion.identity;

                    spawnedCount++;
                }
            }
        }

        private async UniTask<ResourceTile> GetTileFromPool(CurrencyType type)
        {
            foreach (var pool in _poolRegistry.GetAllPoolsOfType<ResourceTile>())
            {
                if (pool.Prefab.CurrencyType == type)
                {
                    return await pool.GetAsync(Vector3.zero, Quaternion.identity);
                }
            }
            return null;
        }
    }
}