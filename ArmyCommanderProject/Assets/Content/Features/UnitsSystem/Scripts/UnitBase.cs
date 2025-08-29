using System;
using Core.EventBus;
using Core.Other;
using Core.PoolSystem;
using Core.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Features.UnitsSystem.Scripts
{
    public class UnitBase : MonoBehaviour, IUnit
    {
        [SerializeField] private UnitTeam _unitTeam;
        [SerializeField] private UnitStatsData _unitStatsData;
        public UnityEvent OnReset;
        
        private GameObjectPool<UnitBase> _myPool;
        private IEventBus _eventBus;

        private void Start()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<UnitDiedEvent>(HandleDeath);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<UnitDiedEvent>(HandleDeath);
        }

        private void HandleDeath(UnitDiedEvent obj)
        {
            GameObject diedGO = obj.Unit.GetUnitGameObject();
            if (diedGO != null && diedGO == gameObject)
            {
                if (_myPool != null)
                {
                    _myPool.Return(this);
                    OnReset?.Invoke();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }

        public GameObject GetUnitGameObject()
        {
            return gameObject;
        }

        public UnitStatsData GetStats()
        {
            return _unitStatsData;
        }

        public UnitTeam GetUnitTeam()
        {
            return _unitTeam;
        }

        public void SetUnitPool(GameObjectPool<UnitBase> _pool)
        {
            _myPool = _pool;
        }
    }
}