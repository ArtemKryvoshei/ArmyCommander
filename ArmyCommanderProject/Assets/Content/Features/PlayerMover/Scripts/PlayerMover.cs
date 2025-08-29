using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.PlayerMover.Scripts
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
    
        private Transform cameraTransform;
        private Vector2 _currentDirection;
        private IEventBus _eventBus;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<JoystickMoveEvent>(ReadJoystickMove);
            _eventBus.Subscribe<JoystickReleasedEvent>(ReadJoystickRelease);

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            _rigidbody = GetComponent<Rigidbody>();
        }

        private void ReadJoystickRelease(JoystickReleasedEvent obj)
        {
            _currentDirection = Vector2.zero;
        }

        private void ReadJoystickMove(JoystickMoveEvent obj)
        {
            _currentDirection = obj.Direction;
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<JoystickMoveEvent>(ReadJoystickMove);
            _eventBus.Unsubscribe<JoystickReleasedEvent>(ReadJoystickRelease);
        }

        private void FixedUpdate() 
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
                move *= moveSpeed * Time.fixedDeltaTime;

                // Двигаем Rigidbody
                _rigidbody.MovePosition(_rigidbody.position + move);

                // Поворот игрока в сторону движения
                _rigidbody.MoveRotation(Quaternion.LookRotation(move.normalized));
            }
        }
    }
}