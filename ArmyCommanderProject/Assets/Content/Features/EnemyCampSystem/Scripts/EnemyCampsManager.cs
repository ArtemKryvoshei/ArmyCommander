using System.Collections.Generic;
using System.Linq;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.EnemyCampSystem.Scripts
{
    public class EnemyCampsManager : MonoBehaviour
    {
        private List<IEnemyCamp> _campsQueue;
        private int _currentIndex;
        private IEventBus _eventBus;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnChargeCalled>(HandleChargeCalled);
        }

        private void OnDestroy()
        {
            _eventBus?.Unsubscribe<OnChargeCalled>(HandleChargeCalled);
        }

        private void HandleChargeCalled(OnChargeCalled evt)
        {
            var allCamps = FindObjectsOfType<MonoBehaviour>().OfType<IEnemyCamp>();
            _campsQueue = allCamps
                .OrderBy(c => c.Weight)
                .ToList();

            foreach (var camp in _campsQueue)
                camp.OnCampCleared += HandleCampCleared;

            _currentIndex = -1;
            NextCamp();
        }

        private void HandleCampCleared(EnemyCamp clearedCamp)
        {
            if (clearedCamp == CurrentCamp)
                NextCamp();
        }

        private void NextCamp()
        {
            _currentIndex++;
            
            while (_currentIndex < _campsQueue.Count && _campsQueue[_currentIndex].IsCleared)
                _currentIndex++;

            if (_currentIndex < _campsQueue.Count)
            {
                var next = _campsQueue[_currentIndex];
                OnNewCampTargeted camp = new OnNewCampTargeted();
                camp.position = next.CampCapturePoint.position;
                _eventBus.Publish(camp);
            }
            else
            {
                Debug.Log("[CampsManager] All camps cleared — VICTORY!");
            }
        }

        private IEnemyCamp CurrentCamp =>
            (_campsQueue != null && _currentIndex < _campsQueue.Count)
                ? _campsQueue[_currentIndex]
                : null;
    }
}