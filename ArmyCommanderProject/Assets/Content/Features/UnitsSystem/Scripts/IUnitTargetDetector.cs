using System;
using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    public interface IUnitTargetDetector
    {
        void ScanForEnemies();
        event Action<Transform> OnEnemyDetected;
    }
}