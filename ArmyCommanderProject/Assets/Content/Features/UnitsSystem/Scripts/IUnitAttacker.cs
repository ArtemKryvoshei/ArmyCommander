using System;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    public interface IUnitAttacker
    {
        event Action OnAttackStateEnter;
        event Action OnAttackStateExit;

        void TryAttack();
        void SetTarget(Transform enemy);
    }
}