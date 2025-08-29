using System;
using Core.Other;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Features.CurrencySystem.Scripts.Visual
{
    public class CurrencyConsumerView : MonoBehaviour
    {
        [SerializeField] private Image imageToFill;
        [SerializeField] private TMP_Text textProgress;
        [SerializeField] private GameObject consumerScriptHolder;
        
        private ICurrencyConsumer consumer;

        private void Start()
        {
            consumer = consumerScriptHolder.GetComponent<ICurrencyConsumer>();
            if (consumer != null)
            {
                UpdateConsumerVisual();
                consumer.OnConsumerChanged += UpdateConsumerVisual;
            }
        }

        private void OnDestroy()
        {
            if (consumer != null)
            {
                consumer.OnConsumerChanged -= UpdateConsumerVisual;
            }
        }

        private void UpdateConsumerVisual()
        {
            if (consumer == null) return;
            float progress = (float)consumer.AlreadyConsumed / consumer.ConsumeAmount;
            imageToFill.fillAmount = progress;
            textProgress.text = consumer.AlreadyConsumed + ConstantsHolder.CONSUME_TXT_SEPARATOR + consumer.ConsumeAmount;
        }
    }
}