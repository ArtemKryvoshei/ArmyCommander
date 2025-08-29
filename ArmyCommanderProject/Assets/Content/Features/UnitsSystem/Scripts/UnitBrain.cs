using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    [RequireComponent(typeof(IUnit))]
    public class UnitBrain : MonoBehaviour
    {
        private IUnit _unit;
        
        private IUnitMover _mover;
        private IUnitTargetDetector _detector;
        private IUnitAttacker _attacker;
        

        private void Awake()
        {
            _unit = GetComponent<IUnit>();
            
            _mover = GetComponent<IUnitMover>();
            _detector = GetComponent<IUnitTargetDetector>();
            _attacker = GetComponent<IUnitAttacker>();

            if (_detector != null)
            {
                _detector.OnEnemyDetected += OnEnemyDetected;
            }

            if (_attacker != null)
            {
                _attacker.OnAttackStateEnter += OnAttackStarted;
                _attacker.OnAttackStateExit += OnAttackStopped;
            }
        }

        private void OnDestroy()
        {
            if (_detector != null)
            {
                _detector.OnEnemyDetected -= OnEnemyDetected;
            }

            if (_attacker != null)
            {
                _attacker.OnAttackStateEnter -= OnAttackStarted;
                _attacker.OnAttackStateExit -= OnAttackStopped;
            }
        }
        
        private void OnEnemyDetected(Transform enemy)
        {
            if (_mover != null)
            {
                _mover.SetMoveTarget(enemy.position);
            }

            if (_attacker != null)
            {
                _attacker.SetTarget(enemy);
            }
        }
        
        private void OnAttackStarted()
        {
            _mover?.StopMovement();
        }
        
        private void OnAttackStopped()
        {
            _mover?.SetMoveTarget(Vector3.zero);
            _mover?.ResumeMovement();
        }
    }
}