using System;
using System.Collections.Generic;
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

        private void Awake()
        {
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
        
        public bool TryGiveTile(CurrencyType type, out ResourceTile tile)
        {
            tile = _carriedTiles.FindLast(t => t.CurrencyType == type);
            if (tile == null)
                return false;

            _carriedTiles.Remove(tile);

            // вернуть плитку в пул
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
    }
}