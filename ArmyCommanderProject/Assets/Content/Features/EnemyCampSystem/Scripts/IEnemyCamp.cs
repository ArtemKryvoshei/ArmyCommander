using System;
using UnityEngine;

namespace Content.Features.EnemyCampSystem.Scripts
{
    public interface IEnemyCamp
    {
        public bool IsCleared { get; }
        public int Weight { get; }
        public Transform CampCapturePoint { get; }
        public event Action<EnemyCamp> OnCampCleared;
    }
}