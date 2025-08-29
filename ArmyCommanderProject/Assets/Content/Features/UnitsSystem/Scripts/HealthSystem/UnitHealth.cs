using System;
using Content.Features.UnitsSystem.Scripts;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.CurrencySystem.Scripts.Visual
{
    [RequireComponent(typeof(IUnit))]
    public class UnitHealth : MonoBehaviour, IUnitHealth
    {
        private float _maxHealth;
        private float _currentHealth;
        private IEventBus _eventBus;
        private IUnit _unit;
    
        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0;
    
        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _unit = GetComponent<IUnit>();
            _maxHealth = _unit.GetStats().MaxHealth;
            ResetHealth();
        }

        private void OnDisable()
        {
            ResetHealth();
        }

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
    
            _currentHealth = Mathf.Max(0, _currentHealth - amount);
            PublishHealthChanged();
    
            if (_currentHealth <= 0)
                Die();
        }
    
        public void Heal(float amount)
        {
            if (IsDead) return;
    
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            PublishHealthChanged();
        }
    
        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
            PublishHealthChanged();
        }
    
        private void Die()
        {
            _eventBus.Publish(new UnitDiedEvent(_unit));
        }
    
        private void PublishHealthChanged()
        {
            _eventBus.Publish(new HealthChangedEvent(_unit, _currentHealth, _maxHealth));
        }
    }
}

