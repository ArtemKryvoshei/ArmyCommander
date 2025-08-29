using System;
using UnityEngine;
using System.Collections.Generic;
using Content.Features.UnitsSystem.Scripts;
using Content.Features.UpgradeSystem.Scripts;
using Core.EventBus;
using Core.Other;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using Cysharp.Threading.Tasks;

namespace Content.Features.TeamBuildSystem.Scripts
{
    [System.Serializable]
    public class UnitsSlotsUpgrade
    {
        public int upgradeTier;
        public int totalSlotsCount;
    }

    public class TeamSlotsHolder : MonoBehaviour, ITeamSlotsHolder, IUpgradeable
    {
        [SerializeField] private UnitType holderUnitsType;
        [Header("Grid Settings")] 
        [SerializeField] private int columns = 3;
        [SerializeField] private float horizontalSpacing = 1.5f;
        [SerializeField] private float verticalSpacing = 1.5f;
        [SerializeField] private Vector3 startOffset = Vector3.zero;
        [SerializeField] private GameObject slotPrefab;

        [Header("Upgrade Info")] 
        [SerializeField] private UnitsSlotsUpgrade[] upgrades;

        private List<Transform> _slots = new();
        private List<bool> _occupied = new();

        private int _upgradeTier = -1;
        
        private IPrefabFactory _prefabFactory;
        private IEventBus _eventBus;
        public int TotalSlots => _slots.Count;
        public int OccupiedSlots
        {
            get
            {
                int count = 0;
                foreach (var occ in _occupied)
                    if (occ) count++;
                return count;
            }
        }
        public int FreeSlots => TotalSlots - OccupiedSlots;
        public UnitType ProvidedType => holderUnitsType;
        public Transform[] GetAllSlots() => _slots.ToArray();
        
        private void Awake()
        {
            _prefabFactory = ServiceLocator.Get<IPrefabFactory>();
            _eventBus = ServiceLocator.Get<IEventBus>();
            _eventBus.Subscribe<OnChargeCalled>(_ => ClearAllSlots());
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnChargeCalled>(_ => ClearAllSlots());
        }

        private async void Start()
        {
            Upgrade();
        }

        private async UniTask GenerateSlotsAsync(int totalSlots)
        {
            for (int i = 0; i < totalSlots; i++)
            {
                GameObject slotObj = await _prefabFactory.CreateAsync(slotPrefab, transform);
                _slots.Add(slotObj.transform);
                _occupied.Add(false);
            }

            RebuildGrid();
        }
        
        public async UniTask IncreaseSlotsAsync(int additionalSlots)
        {
            for (int i = 0; i < additionalSlots; i++)
            {
                GameObject slotObj = await _prefabFactory.CreateAsync(slotPrefab, transform);
                _slots.Add(slotObj.transform);
                _occupied.Add(false);
            }

            RebuildGrid();
        }
        
        private void RebuildGrid()
        {
            int total = _slots.Count;
            int rows = Mathf.CeilToInt((float)total / columns);

            for (int i = 0; i < total; i++)
            {
                int r = i / columns;
                int c = i % columns;

                Vector3 pos = startOffset + new Vector3(c * horizontalSpacing, 0, r * verticalSpacing);
                _slots[i].localPosition = pos;
                _slots[i].localRotation = Quaternion.identity;
            }
        }
        
        public bool HasFreeSlot()
        {
            foreach (var occ in _occupied)
            {
                if (!occ) return true;
            }
            return false;
        }

        public Transform OccupyFreeSlot()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (!_occupied[i])
                {
                    _occupied[i] = true;
                    return _slots[i];
                }
            }
            return null;
        }

        public void ReleaseSlot(Transform slot)
        {
            int index = _slots.IndexOf(slot);
            if (index >= 0)
            {
                _occupied[index] = false;
            }
        }

        public void ClearAllSlots()
        {
            for (int i = 0; i < _occupied.Count; i++)
                _occupied[i] = false;
        }

        public int GetUpgradeTier()
        {
            return _upgradeTier;
        }

        public async void Upgrade()
        {
            _upgradeTier++;
            int requiredSlots = FindTierSlotsCount(_upgradeTier);
            int currentSlots = _slots.Count;
            await GenerateSlotsAsync(requiredSlots - currentSlots);
        }

        private int FindTierSlotsCount(int tier)
        {
            foreach (var tierInfo in upgrades)
            {
                if (tierInfo.upgradeTier == tier)
                {
                    return tierInfo.totalSlotsCount;
                }
            }
            return upgrades[0].totalSlotsCount;
        }
    }
}