using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Content.Global.Scripts
{
    public class BattleEventCaller : MonoBehaviour
    {
        [SerializeField] private float cooldownSeconds = 3f;

        private bool _canTrigger = true;
        private IEventBus _eventBus;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
        }

        public void TriggerBattle()
        {
            if (!_canTrigger) return;

            _canTrigger = false;
            _eventBus.Publish(new OnPrepareAllyUnits());
            StartCooldownAsync().Forget();
            _eventBus.Publish(new OnChargeCalled());
            Debug.Log("[BattleEventCaller] CHARGE!");
        }

        private async UniTaskVoid StartCooldownAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(cooldownSeconds));
            _canTrigger = true;
        }
    }
}