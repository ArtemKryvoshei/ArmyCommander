using Content.Features.UnitsSystem.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.TeamBuildSystem.Scripts
{
    public interface ITeamSlotsHolder : IUnitTypeProvider
    {
        int TotalSlots { get; }
        int OccupiedSlots { get; }
        int FreeSlots { get; }

        Transform[] GetAllSlots();
        bool HasFreeSlot();
        Transform OccupyFreeSlot();
        void ClearAllSlots();
        void ReleaseSlot(Transform slot);
        public UniTask IncreaseSlotsAsync(int additionalSlots);
    }
}