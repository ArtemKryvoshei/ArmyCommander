using System.Collections.Generic;
using Core.EventBus;
using Core.Other;
using UnityEngine;

namespace Content.Features.CurrencySystem.Scripts
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly IEventBus _eventBus;
        private readonly Dictionary<CurrencyType, int> _currencies = new();

        public CurrencyManager(IEventBus eventBus)
        {
            _eventBus = eventBus;
            foreach (CurrencyType type in System.Enum.GetValues(typeof(CurrencyType)))
            {
                _currencies[type] = 0;
            }
        }

        public int GetCurrency(CurrencyType type)
        {
            return _currencies[type];
        }

        public void AddCurrency(CurrencyType type, int amount)
        {
            _currencies[type] += amount;
            _eventBus.Publish(new CurrencyChangedEvent(type, _currencies[type]));
        }

        public void SubtractCurrency(CurrencyType type, int amount)
        {
            _currencies[type] = Mathf.Max(0, _currencies[type] - amount);
            _eventBus.Publish(new CurrencyChangedEvent(type, _currencies[type]));
        }
    }
}