using System;
using Core.EventBus;
using Core.Other;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.PlayerRespawner.Scripts
{
    public class SimplePlayerRespawner : MonoBehaviour, IPlayerRespawner
    {
        [SerializeField] private GameObject _playerPrefab;
        private IPrefabFactory _factory;
        private IEventBus _eventBus;
        private Transform _spawnPoint;
        private GameObject _currentPlayer;
        
        public Transform PlayerTransform => _currentPlayer != null ? _currentPlayer.transform : null;

        private void Awake()
        {
            ServiceLocator.Register<IPlayerRespawner>(this);
            _eventBus = ServiceLocator.Get<IEventBus>();
            _factory = ServiceLocator.Get<IPrefabFactory>();
            _eventBus.Subscribe<OnMapContentInit>(InitRespawner);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnMapContentInit>(InitRespawner);
        }

        private void InitRespawner(OnMapContentInit obj)
        {
            if (obj.activeMap != null)
            {
                _spawnPoint = obj.activeMap.GetPlayerStartPoint();
                RespawnPlayer();
            }
        }

        public async void RespawnPlayer()
        {
            await RespawnAsync();
        }
        
        public async UniTask RespawnAsync()
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.transform.position = _spawnPoint.position;
                _currentPlayer.transform.rotation = _spawnPoint.rotation;
                Debug.Log("[PlayerRespawner] Player repositioned to spawn point.");
                _eventBus.Publish(new OnPlayerRespawn());
            }
            else
            {
                // Создаём нового игрока
                _currentPlayer = await _factory.CreateAsync(_playerPrefab, _spawnPoint.position, _spawnPoint.rotation);
                _eventBus.Publish(new OnPlayerSpawned());
                Debug.Log("[PlayerRespawner] Player created at spawn point.");
            }
           
        }
    }
}