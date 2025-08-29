using Content.Features.PlayerRespawner.Scripts;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.CameraControll.Scripts
{
    public class SimpleCameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new Vector3(0, 10, -10);
        [SerializeField] private float smoothSpeed = 5f;

        private Transform _target;
        private IEventBus _eventBus;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnPlayerSpawned>(OnPlayerSpawned);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnPlayerSpawned>(OnPlayerSpawned);
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 desiredPosition = _target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            transform.position = smoothedPosition;
            transform.LookAt(_target);
        }

        private void OnPlayerSpawned(OnPlayerSpawned evt)
        {
            var respawner = ServiceLocator.Get<IPlayerRespawner>();
            _target = respawner?.PlayerTransform;
        }
    }
}