using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.PoolSystem
{
    public interface IGameObjectPool<T> where T : MonoBehaviour, IPoolableObject
    {
        UniTask PreloadAsync(int count);
        
        UniTask<T> GetAsync(Vector3 position, Quaternion rotation, Transform parent = null);
        
        void Return(T instance);
        
        void Clear(bool destroy = false);
        
        int CountInactive { get; }
        
        int CountActive { get; }
        
        T Prefab { get; }
    }
}