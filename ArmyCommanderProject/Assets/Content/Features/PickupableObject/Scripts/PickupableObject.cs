using System;
using System.Linq;
using Content.Features.PlayerRespawner.Scripts;
using Core.EventBus;
using Core.Other;
using Core.PoolSystem;
using Core.ServiceLocatorSystem;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Features.PickupableObject.Scripts
{
    public class PickupableObject : MonoBehaviour
    {
        [SerializeField] private float pickupRadius = 1.5f;
        public UnityEvent OnPickup;
        
        private IEventBus _eventBus;
        private IPlayerRespawner _playerRespawner;
        private Transform _playerTransform;
        private bool _picked;

        private void Start()
        {
            _playerRespawner = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IPlayerRespawner>().ToArray().First();
        }

        private void Update()
        {
            if (_playerTransform == null)
            {
                _playerTransform = _playerRespawner?.PlayerTransform;
            }
            
            if (_picked || _playerTransform == null) return;
            
            float dist = Vector3.Distance(_playerTransform.position, transform.position);
            if (dist <= pickupRadius)
            {
                _picked = true;
                OnPickup?.Invoke();
                Debug.Log("[PickupableObject] Pickup " + gameObject.name);
            }
        }

        public void ResetPickable()
        {
            _picked = false;
        }
    }
}