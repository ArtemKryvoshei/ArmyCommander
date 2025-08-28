using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.PrefabFactory
{
    public class PrefabFactory : IPrefabFactory
    {
        //Предполагаю что в будущем будут добавлены адресблс, а пока просто спавн в конце кардра

        public async UniTask<GameObject> CreateAsync(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var instance = Object.Instantiate(prefab, position, rotation);
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return instance;
        }

        public async UniTask<GameObject> CreateAsync(GameObject prefab, Transform parent)
        {
            var instance = Object.Instantiate(prefab, parent);
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return instance;
        }

        public async UniTask<GameObject> CreateAsync(GameObject prefab)
        {
            var instance = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return instance;
        }
        
        public async UniTask<T> CreateAsync<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            var instance = Object.Instantiate(prefab, position, rotation);
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return instance;
        }

        public async UniTask<T> CreateAsync<T>(T prefab, Transform parent) where T : MonoBehaviour
        {
            var instance = Object.Instantiate(prefab, parent);
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return instance;
        }

        public async UniTask<T> CreateAsync<T>(T prefab) where T : MonoBehaviour
        {
            var instance = Object.Instantiate(prefab);
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return instance;
        }
    }
}