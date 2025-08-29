using System;
using System.Collections.Generic;
using Core.EventBus;
using Core.Other;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Content.Features.EnemyCampSystem.Scripts
{
    public class EnemyCamp : MonoBehaviour, IEnemyCamp
    {
        [Header("Camp Settings")] 
        [SerializeField] private Transform _pointToCapture;
        [SerializeField] private int weight = 1;
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private BoxCollider spawnZone;

        private readonly List<GameObject> _spawnedEnemies = new();
        private IPrefabFactory _prefabFactory;
        private IEventBus _eventBus;

        public bool IsCleared { get; private set; }
        public int Weight => weight;
        public Transform CampCapturePoint => _pointToCapture;

        public event Action<EnemyCamp> OnCampCleared; 

        private async void Start()
        {
            IsCleared = false;
            _prefabFactory = ServiceLocator.Get<IPrefabFactory>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            if (spawnZone == null)
            {
                Debug.LogError($"[EnemyCamp] {name} has no spawn zone!");
                return;
            }

            _eventBus.Subscribe<UnitDiedEvent>(HandleEnemyDeath);
            await SpawnEnemiesAsync();
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<UnitDiedEvent>(HandleEnemyDeath);
        }

        private async UniTask SpawnEnemiesAsync()
        {
            foreach (var prefab in enemyPrefabs)
            {
                Vector3 pos = GetRandomPointInZone();
                var enemy = await _prefabFactory.CreateAsync(prefab, pos, Quaternion.identity);

                _spawnedEnemies.Add(enemy);
            }
        }

        private void HandleEnemyDeath(UnitDiedEvent obj)
        {
            if (_spawnedEnemies.Contains(obj.Unit.GetUnitGameObject()))
                _spawnedEnemies.Remove(obj.Unit.GetUnitGameObject());

            if (_spawnedEnemies.Count == 0 && !IsCleared)
            {
                IsCleared = true;
                Debug.Log($"[EnemyCamp] {name} cleared!");
                OnCampCleared?.Invoke(this);
            }
        }

        private Vector3 GetRandomPointInZone()
        {
            Bounds b = spawnZone.bounds;

            float x = Random.Range(b.min.x, b.max.x);
            float z = Random.Range(b.min.z, b.max.z);

            return new Vector3(x, spawnZone.transform.position.y, z);
        }
    }
}