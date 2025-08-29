using System;
using System.Collections.Generic;
using System.Linq;
using Core.PrefabFactory;
using Core.ServiceLocatorSystem;
using UnityEngine;

namespace Content.Features.CurrencySystem.Scripts.Visual
{
    public class PlayerBackpack : MonoBehaviour, IPlayerBackpack
    {
        [SerializeField] private Transform stackRoot;  
        [SerializeField] private float stackOffsetY = 0.5f;
        
        private readonly List<ResourceTile> _carriedTiles = new();
        private IPoolRegistry _poolRegistry;
        private ICurrencyManager _currencyManager;

        private void Awake()
        {
            _currencyManager = ServiceLocator.Get<ICurrencyManager>();
            _poolRegistry = ServiceLocator.Get<IPoolRegistry>();
            ServiceLocator.Register<IPlayerBackpack>(this);
        }
        
        public void AddTile(ResourceTile tile)
        {
            _carriedTiles.Add(tile);
            
            tile.transform.SetParent(stackRoot);
            tile.transform.localPosition = new Vector3(0, _carriedTiles.Count * stackOffsetY, 0);
            tile.transform.localRotation = Quaternion.identity;
        }

        public bool GiveTile(CurrencyType type)
        {
            var tile = _carriedTiles.FirstOrDefault(t => t.CurrencyType == type);
            if (tile == null) return false;

            _carriedTiles.Remove(tile);
            ReStackTiles();
            
            _currencyManager.SubtractCurrency(type, 1);
            
            foreach (var pool in _poolRegistry.GetAllPoolsOfType<ResourceTile>())
            {
                if (pool.Prefab.CurrencyType == type)
                {
                    pool.Return(tile);
                    return true;
                }
            }

            return false;
        }

        public bool GiveTile(CurrencyType type, int amount)
        {
            if (_carriedTiles.Count(t => t.CurrencyType == type) < amount)
                return false;

            for (int i = 0; i < amount; i++)
            {
                GiveTile(type);
            }
            return true;
        }

        public void ClearAll()
        {
            foreach (var tile in _carriedTiles)
            {
                foreach (var pool in _poolRegistry.GetAllPoolsOfType<ResourceTile>())
                {
                    if (pool.Prefab.CurrencyType == tile.CurrencyType)
                    {
                        pool.Return(tile);
                        break;
                    }
                }
            }
            _carriedTiles.Clear();
        }
        
        private void ReStackTiles()
        {
            for (int i = 0; i < _carriedTiles.Count; i++)
            {
                var tile = _carriedTiles[i];
                tile.transform.localPosition = new Vector3(0, (i + 1) * stackOffsetY, 0);
            }
        }
    }
}