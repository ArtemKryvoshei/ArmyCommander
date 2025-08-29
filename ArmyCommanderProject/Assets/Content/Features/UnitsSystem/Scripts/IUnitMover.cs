using UnityEngine;

namespace Content.Features.UnitsSystem.Scripts
{
    public interface IUnitMover
    {
        void SetMainTarget(Vector3 mainPos);
        void SetMoveTarget(Vector3 targetPos);
        void StopMovement();
        void ResumeMovement();
        void PrepareForCharge();
    }
}