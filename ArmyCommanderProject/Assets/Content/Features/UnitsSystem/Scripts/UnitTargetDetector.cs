using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    [RequireComponent(typeof(IUnit))]
    public class UnitTargetDetector : MonoBehaviour, IUnitTargetDetector
    {
        public event Action<Transform> OnEnemyDetected;

        [SerializeField] private LayerMask enemyLayerMask;

        private IUnit _unit;
        private IEventBus _eventBus;
        private float _detectionRange;
        

        private void Awake()
        {
            _unit = GetComponent<IUnit>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _detectionRange = _unit.GetStats().DetectionRange;
        }
        
        private void Update()
        {
            ScanForEnemies();
        }

        public void ScanForEnemies()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRange, enemyLayerMask);
            Transform nearestEnemy = null;
            float nearestDist = float.MaxValue;

            foreach (var hit in hits)
            {
                var unit = hit.GetComponent<IUnit>();
                if (unit == null || unit.GetUnitTeam() == _unit.GetUnitTeam()) continue;

                float dist = Vector3.Distance(transform.position, unit.GetUnitGameObject().transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestEnemy = unit.GetUnitGameObject().transform;
                }
            }

            if (nearestEnemy != null)
                OnEnemyDetected?.Invoke(nearestEnemy);
        }
    }
}