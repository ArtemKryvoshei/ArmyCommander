using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.PlayerMover.Scripts
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        
        private Transform cameraTransform;
        private Vector2 _currentDirection;
        private IEventBus _eventBus;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<JoystickMoveEvent>(evt => _currentDirection = evt.Direction);
            _eventBus.Subscribe<JoystickReleasedEvent>(_ => _currentDirection = Vector2.zero);

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (_currentDirection != Vector2.zero)
            {
                Vector3 camForward = cameraTransform.forward;
                Vector3 camRight = cameraTransform.right;

                camForward.y = 0f;
                camRight.y = 0f;

                camForward.Normalize();
                camRight.Normalize();
                
                Vector3 move = camRight * _currentDirection.x + camForward * _currentDirection.y;
                move *= moveSpeed * Time.deltaTime;

                transform.position += move;
                
                transform.forward = move.normalized;
            }
        }
    }
}