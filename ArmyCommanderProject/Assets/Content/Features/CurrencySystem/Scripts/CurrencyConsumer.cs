using System;
using Content.Features.CurrencySystem.Scripts.Visual;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Features.CurrencySystem.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class CurrencyConsumer : MonoBehaviour, ICurrencyConsumer
    {
        [Header("General Setup")]
        [SerializeField] private LayerMask _consumeableLayerMask;
        [SerializeField] private float consumeSpeed;
        [Header("Specific Setup")]
        [SerializeField] private int consumeAmount;
        [SerializeField] private CurrencyType consumeType;
        public UnityEvent OnRequiredAmountConsumed;
        
        public CurrencyType ConsumeType => consumeType;
        public int ConsumeAmount => consumeAmount;
        public int AlreadyConsumed => alreadyConsumedAmount;
        public event Action OnConsumerChanged;

        private bool consumingResources = false;
        private int alreadyConsumedAmount;
        private float consumeTimer = 0f;
        private IPlayerBackpack currentPlayerBackpack;

        public void Init(CurrencyType type, int amount)
        {
            consumeAmount = amount;
            consumeType = type;
            ResetProgress();
        }

        public void ResetProgress()
        {
            alreadyConsumedAmount = 0;
            consumeTimer = 0;
            OnConsumerChanged?.Invoke();
        }

        private void LateUpdate()
        {
            if (!consumingResources || currentPlayerBackpack == null) return;
            if (alreadyConsumedAmount >= consumeAmount) return;

            consumeTimer += Time.deltaTime;
            if (consumeTimer >= 1f / consumeSpeed)
            {
                consumeTimer = 0f;

                if (currentPlayerBackpack.GiveTile(consumeType))
                {
                    alreadyConsumedAmount++;
                    Debug.Log($"[CurrencyConsumer] Consumed {alreadyConsumedAmount}/{consumeAmount} {consumeType}");
                    OnConsumerChanged?.Invoke();

                    if (alreadyConsumedAmount >= consumeAmount)
                    {
                        Debug.Log($"[CurrencyConsumer] Required amount of {consumeType} consumed!");
                        OnRequiredAmountConsumed?.Invoke();
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //if game object is on _consumeableLayerMask layer, then set consumingResources to true, and get IPlayerBackpack
            if (((1 << other.gameObject.layer) & _consumeableLayerMask) != 0)
            {
                currentPlayerBackpack = other.GetComponent<IPlayerBackpack>();
                if (currentPlayerBackpack != null)
                {
                    consumingResources = true;
                    Debug.Log("[CurrencyConsumer] Player entered, start consuming...");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //if game object is on _consumeableLayerMask layer, then set consumingResources to false, currentPlayerBackpack set to null
            if (((1 << other.gameObject.layer) & _consumeableLayerMask) != 0)
            {
                var backpack = other.GetComponent<IPlayerBackpack>();
                if (backpack == currentPlayerBackpack)
                {
                    consumingResources = false;
                    currentPlayerBackpack = null;
                    Debug.Log("[CurrencyConsumer] Player exited, stop consuming...");
                }
            }
        }
    }
}