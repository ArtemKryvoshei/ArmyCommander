using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.PrefabFactory
{
    public interface IPrefabFactory
    {
        UniTask<GameObject> CreateAsync(GameObject prefab, Vector3 position, Quaternion rotation);
        UniTask<GameObject> CreateAsync(GameObject prefab, Transform parent);
        UniTask<GameObject> CreateAsync(GameObject prefab);
        
        UniTask<T> CreateAsync<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour;
        UniTask<T> CreateAsync<T>(T prefab, Transform parent) where T : MonoBehaviour;
        UniTask<T> CreateAsync<T>(T prefab) where T : MonoBehaviour;
    }
}