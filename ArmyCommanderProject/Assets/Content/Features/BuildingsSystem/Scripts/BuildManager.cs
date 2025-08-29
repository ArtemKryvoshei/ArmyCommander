using System;
using Core.EventBus;
using Core.Other;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Content.Features.BuildingsSystem.Scripts
{
    public class BuildManager : MonoBehaviour, IBuildManager
    {
        [SerializeField] private BuildData[] _availableBuildings;
        [SerializeField] private Transform buildPlace;
        public UnityEvent OnBuildComplete;
        
        private bool prepared = false;
        private IPrefabFactory _prefabFactory;
        private IEventBus _eventBus;

        private void Awake()
        {
            _prefabFactory = ServiceLocator.Get<IPrefabFactory>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnBuildRequest>(TryBuild);
        }

        private void TryBuild(OnBuildRequest obj)
        {
            Build(obj.buildingID);
        }

        public BuildData[] GetAvailableBuilds()
        {
            return _availableBuildings;
        }

        public async  void Build(int id)
        {
            if (!prepared)
            {
                return;
            }

            foreach (var building in _availableBuildings)
            {
                if (building.Id == id && _prefabFactory != null)
                {
                    if (building.Id == id && _prefabFactory != null)
                    {
                        Debug.Log($"[BuildManager] Building {building.UIName}");
                        await _prefabFactory.CreateAsync(building.Prefab, buildPlace);
            
                        OnBuildComplete?.Invoke();
                        prepared = false;
                        return;
                    }
                }
            }
            Debug.LogError("[BuildManager] Cant build!, no building or wrong ID");
        }

        public void ChooseBuilding()
        {
            prepared = true;
            OnChooseBuildingCall buildingMenuShowEvent = new OnChooseBuildingCall();
            buildingMenuShowEvent.buildManager = this;
            _eventBus.Publish(buildingMenuShowEvent);
        }

        //Это на случай если я не успею доделать выбор зданий, просто билдим первое в списке, в дальнейшем можно доделать для выбора
        public void TestBuild()
        {
            Build(0);
        }
    }
}