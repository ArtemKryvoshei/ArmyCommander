using System;
using Core.EventBus;
using Core.Other;
using Core.ServiceLocatorSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Features.BuildingsSystem.Scripts
{
    public class BuildingUIElementView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text name;
        [SerializeField] private Button buildButton;

        private BuildData _myData;
        private IEventBus _eventBus;

        private void Awake()
        {
            _eventBus = ServiceLocator.Get<IEventBus>();
        }

        public void Initialize(BuildData building)
        {
            _myData = building;

            icon.sprite = _myData.UIIcon;
            name.text = _myData.UIName;

            buildButton.onClick.RemoveAllListeners();
            buildButton.onClick.AddListener(TryBuildThis);
        }

        public void TryBuildThis()
        {
            if (_myData != null && _eventBus != null)
            {
                OnBuildRequest eventToSend = new OnBuildRequest();
                eventToSend.buildingID = _myData.Id;
                _eventBus.Publish(eventToSend);
            }
            else
            {
                Debug.LogError("[BuildingUIElementView] Can't call build, something wrong");
            }
        }
    }
}