using System;
using Core.EventBus;
using Core.Other;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.BuildingsSystem.Scripts
{
    public class BuildMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject menuHolder;
        [SerializeField] private GameObject buildingUIElementPrefab;
        [SerializeField] private RectTransform buildingsContentHolder;
        
        private IEventBus _eventBus;
        private IPrefabFactory _prefabFactory;
        private BuildManager currentBuilder;
        private bool menuOpened;

        private void Awake()
        {
            _prefabFactory = ServiceLocator.Get<IPrefabFactory>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnChooseBuildingCall>(ShowMenu);
            _eventBus.Subscribe<OnBuildRequest>(TryHideMenu);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnChooseBuildingCall>(ShowMenu);
            _eventBus.Unsubscribe<OnBuildRequest>(TryHideMenu);
        }

        private void TryHideMenu(OnBuildRequest obj)
        {
            HideMenu();
        }

        private async  void ShowMenu(OnChooseBuildingCall buildEvent)
        {
            if (buildEvent.buildManager != null && menuOpened is false)
            {
                menuOpened = true;
                currentBuilder = buildEvent.buildManager;

                menuHolder.SetActive(true);
                ClearElements();

                foreach (var building in currentBuilder.GetAvailableBuilds())
                {
                    var go = await _prefabFactory.CreateAsync(buildingUIElementPrefab, buildingsContentHolder);
                    var uiElem = go.GetComponent<BuildingUIElementView>();
                    if (uiElem != null)
                    {
                        uiElem.Initialize(building);
                    }
                }
            }
        }

        private void HideMenu()
        {
            menuOpened = false;
            ClearElements();
            menuHolder.SetActive(false);
        }

        private void ClearElements()
        {
            foreach (Transform elem in buildingsContentHolder)
            {
                Destroy(elem.gameObject);
            }
        }
    }
}