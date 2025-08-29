using System;
using System.Collections.Generic;
using System.Linq;
using Content.Features.TeamBuildSystem.Scripts;
using Core.EventBus;
using Core.Other;
using Core.PoolSystem;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    public class PlayerUnitsSpawner : MonoBehaviour, IPlayerUnitSpawner
    {
        [SerializeField] private UnitType spawnType;
        [SerializeField] private UnitBase prefab;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private float spawnTime = 2f;

        public UnitType SpawnType => spawnType;
        public float SpawnTime => spawnTime;

        private IPrefabFactory _prefabFactory;
        private IEventBus _eventBus;
        private GameObjectPool<UnitBase> _pool;
        private List<UnitBase> unitsToPrepare;

        private void Awake()
        {
            unitsToPrepare = new List<UnitBase>();
            _prefabFactory = ServiceLocator.Get<IPrefabFactory>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnPrepareAllyUnits>(PrepareThisBuildingUnits);
            _pool = new GameObjectPool<UnitBase>(_prefabFactory, prefab, transform);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnPrepareAllyUnits>(PrepareThisBuildingUnits);
        }

        private void PrepareThisBuildingUnits(OnPrepareAllyUnits obj)
        {
            foreach (var uni in unitsToPrepare)
            {
                uni.GetComponent<IUnitMover>().PrepareForCharge();
            }
            unitsToPrepare.Clear();
        }

        private void Start()
        {
            StartSpawn();
        }

        public void StartSpawn()
        {
            TrySpawnAsync().Forget();
        }

        private async UniTaskVoid TrySpawnAsync()
        {
            while (true)
            {
                var holders = FindObjectsOfType<MonoBehaviour>().OfType<ITeamSlotsHolder>();
                var suitableHolder = holders.FirstOrDefault(h => h.ProvidedType == spawnType && h.HasFreeSlot());

                if (suitableHolder != null)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(spawnTime));
                    if (suitableHolder.HasFreeSlot())
                    {
                        Transform slot = suitableHolder.OccupyFreeSlot();
                        var unitObj = await _prefabFactory.CreateAsync(prefab, spawnPos.position, Quaternion.identity);
                        unitObj.GetComponent<IUnitMover>()?.SetMoveTarget(slot.position);
                        unitsToPrepare.Add(unitObj);
                        Debug.Log($"[UnitSpawner] Spawned {spawnType} at slot {slot.name}");
                    }
                }

                await UniTask.Delay(TimeSpan.FromSeconds(1f));
            }
        }
    }

}