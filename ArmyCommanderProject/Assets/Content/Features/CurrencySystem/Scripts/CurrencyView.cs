using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using UnityEngine;
using TMPro;


namespace Content.Features.CurrencySystem.Scripts
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private CurrencyType _type;
        
        private IEventBus _eventBus;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        private void OnCurrencyChanged(CurrencyChangedEvent evt)
        {
            if (evt.Type == _type)
            {
                currencyText.text = evt.NewAmount.ToString();
            }
        }

        private void OnDestroy()
        {
            if (_eventBus != null)
                _eventBus.Unsubscribe<CurrencyChangedEvent>(OnCurrencyChanged);
        }
    }
}