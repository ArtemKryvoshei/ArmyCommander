using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(IUnit))]
    public class UnitMover : MonoBehaviour, IUnitMover
    {
        private IUnit _unit;
        private IEventBus _eventBus;
        private Rigidbody _rb;
        private Vector3? _mainTargetPosition;
        private Vector3? _targetPosition;
        private bool _isMoving = true;
        private bool _prepared = false;
        private float moveSpeed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _unit = GetComponent<IUnit>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnNewCampTargeted>(ReactToCharge);
            moveSpeed = _unit.GetStats().MoveSpeed;
        }

        private void FixedUpdate()
        {
            if (!_isMoving)
            {
               return;
            }

            Vector3 moveTarget = _targetPosition ?? _mainTargetPosition ?? transform.position;
            Vector3 dir = moveTarget - transform.position;
            dir.y = 0;

            if (dir.sqrMagnitude < 0.01f) return;

            Vector3 moveStep = dir.normalized * moveSpeed * Time.fixedDeltaTime;
            if (moveStep.sqrMagnitude > dir.sqrMagnitude)
                moveStep = dir;

            _rb.MovePosition(_rb.position + moveStep);
            _rb.MoveRotation(Quaternion.LookRotation(dir.normalized));
        }

        private void OnDisable()
        {
            _mainTargetPosition = null;
            _targetPosition = null;
            _isMoving = false;
            _prepared = false;
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnNewCampTargeted>(ReactToCharge);
        }

        private void ReactToCharge(OnNewCampTargeted obj)
        {
            if (_unit.GetUnitTeam() == UnitTeam.PlayerTeam && _prepared)
            {
                SetMainTarget(obj.position);
                SetMoveTarget(obj.position);
                ResumeMovement();
            }
            else
            {
                SetMainTarget(transform.position);
            }
        }
        
        
        public void SetMainTarget(Vector3 mainPos) => _mainTargetPosition = mainPos;

        public void SetMoveTarget(Vector3 targetPos) => _targetPosition = targetPos;

        public void StopMovement() => _isMoving = false;

        public void ResumeMovement()
        {
            _isMoving = true;

            if (_targetPosition == null || _targetPosition == Vector3.zero)
            {
                _targetPosition = _mainTargetPosition;
            }
        }

        public void PrepareForCharge()
        {
            _prepared = true;
        }
    }
}