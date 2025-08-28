using System.Collections.Generic;
using Core.PoolSystem;
using UnityEngine;

namespace Core.PrefabFactory
{
    public interface IPoolRegistry
    {
        void RegisterPool<T>(IGameObjectPool<T> pool) where T : MonoBehaviour, IPoolableObject;
        bool TryGetPool<T>(out IGameObjectPool<T> pool) where T : MonoBehaviour, IPoolableObject;
        IEnumerable<IGameObjectPool<T>> GetAllPoolsOfType<T>() where T : MonoBehaviour, IPoolableObject;
    }
}