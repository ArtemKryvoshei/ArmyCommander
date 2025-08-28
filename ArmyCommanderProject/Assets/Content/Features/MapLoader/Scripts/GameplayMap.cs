using Content.Features.PlayerRespawner.Scripts;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.MapLoader.Scripts
{
    public class GameplayMap : MonoBehaviour, IGameplayMap
    {
        [SerializeField] private Transform _playerStartPoint;
        [SerializeField] private Transform _startCurrencyPoint;
        private IEventBus _eventBus;
        
        public void LoadAndInitMap()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            if (_eventBus != null)
            {
                OnMapContentInit onMapContentInit = new OnMapContentInit();
                onMapContentInit.activeMap = this;
                _eventBus.Publish(onMapContentInit);
                Debug.Log("[GameplayMap] Initialize spawners and stuff");
            }
            else
            {
                Debug.LogError("[GameplayMap] Event bus not initialized or not found");
            }
        }

        public Transform GetPlayerStartPoint()
        {
            return _playerStartPoint;
        }

        public Transform GetStartCurrencySpawnPoint()
        {
            return _startCurrencyPoint;
        }
    }
}