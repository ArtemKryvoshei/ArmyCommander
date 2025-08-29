using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    public class UnitAttacker : MonoBehaviour, IUnitAttacker
    {
        [SerializeField] private LayerMask enemyLayerMask;
        public event Action OnAttackStateEnter;
        public event Action OnAttackStateExit;
        
        
        private IUnit _unit;
        private Transform _currentTarget;
        private IEventBus _eventBus;
        private float _attackRange;
        private int _attackDamage;
        private float _attackInterval;
        private float _attackTimer;

        private bool _isAttacking = false;

        private void Awake()
        {
            _unit = GetComponent<IUnit>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            var stats = _unit.GetStats();
            _attackRange = stats.AttackRange;
            _attackDamage = stats.AttackDamage;
            _attackInterval = stats.AttackInterval;
            _attackTimer = _attackInterval;
        }

        private void Update()
        {
            TryAttack();
        }
        
        private void OnDisable()
        {
            _attackTimer = _attackInterval;
            SetTarget(null);
            ExitAttackState();
        }

        public void SetTarget(Transform target)
        {
            _currentTarget = target;
        }

        public void TryAttack()
        {
            if (_currentTarget == null)
            {
                ExitAttackState();
                return;
            }

            float distance = Vector3.Distance(transform.position, _currentTarget.position);
            var health = _currentTarget.GetComponent<IUnitHealth>();
            if (distance <= _attackRange)
            {
                EnterAttackState();
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0f)
                {
                    _attackTimer = _attackInterval;
                    if (health != null)
                    {
                        health.TakeDamage(_attackDamage);
                    }
                }
            }
            else
            {
                _currentTarget = null;
                ExitAttackState();
            }
        }

        private void EnterAttackState()
        {
            if (_isAttacking) return;
            _isAttacking = true;
            OnAttackStateEnter?.Invoke();
        }

        private void ExitAttackState()
        {
            if (!_isAttacking) return;
            _isAttacking = false;
            OnAttackStateExit?.Invoke();
        }
    }
}