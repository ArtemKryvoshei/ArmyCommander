using System;
using System.Collections.Generic;
using Core.PoolSystem;
using UnityEngine;

namespace Core.PrefabFactory
{
    public class PoolRegistry : IPoolRegistry
    {
        private readonly Dictionary<Type, List<object>> _pools = new();
        
        public void RegisterPool<T>(IGameObjectPool<T> pool) where T : MonoBehaviour, IPoolableObject
        {
            var type = typeof(T);
            if (!_pools.ContainsKey(type))
                _pools[type] = new List<object>();

            _pools[type].Add(pool);
        }
        
        public bool TryGetPool<T>(out IGameObjectPool<T> pool) where T : MonoBehaviour, IPoolableObject
        {
            var type = typeof(T);
            if (_pools.TryGetValue(type, out var list) && list.Count > 0)
            {
                // Возьмём первый пул нужного типа
                pool = (IGameObjectPool<T>)list[0];
                return true;
            }

            pool = default;
            return false;
        }
        
        public IEnumerable<IGameObjectPool<T>> GetAllPoolsOfType<T>() where T : MonoBehaviour, IPoolableObject
        {
            var type = typeof(T);
            if (_pools.TryGetValue(type, out var list))
            {
                foreach (var poolObj in list)
                    yield return (IGameObjectPool<T>)poolObj;
            }
        }
    }
}