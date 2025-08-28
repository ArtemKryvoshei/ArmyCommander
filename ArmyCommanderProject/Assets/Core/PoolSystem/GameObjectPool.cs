using System.Collections.Generic;
using Core.PrefabFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.PoolSystem
{
    public class GameObjectPool<T> : IGameObjectPool<T> where T : MonoBehaviour, IPoolableObject
    {
        private readonly IPrefabFactory _factory;
        private readonly T _prefab;
        private readonly Stack<T> _inactive = new Stack<T>();
        private int _activeCount;
        private readonly Transform _poolRoot; // для порядка в иерархии

        public int CountInactive => _inactive.Count;
        public int CountActive => _activeCount;
        public T Prefab => _prefab;
        
        public GameObjectPool(IPrefabFactory factory, T prefab, Transform poolRoot = null)
        {
            _factory = factory;
            _prefab = prefab;
            _poolRoot = poolRoot;
        }
        
        public async UniTask PreloadAsync(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var inst = await _factory.CreateAsync(_prefab);
                PrepareAsInactive(inst);
                _inactive.Push(inst);
            }
        }
        
        public async UniTask<T> GetAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            T inst;
            if (_inactive.Count > 0)
            {
                inst = _inactive.Pop();

                // На случай, если объект был уничтожен извне.
                if (inst == null)
                {
                    inst = await _factory.CreateAsync(_prefab, position, rotation);
                }
                else
                {
                    var tr = inst.transform;
                    tr.SetParent(parent);
                    tr.SetPositionAndRotation(position, rotation);
                    inst.gameObject.SetActive(true);
                }
            }
            else
            {
                inst = await _factory.CreateAsync(_prefab, position, rotation);
            }

            _activeCount++;
            inst.OnSpawned();
            return inst;
        }
        
        public void Return(T instance)
        {
            if (instance == null) return;

            instance.OnDespawned();
            var go = instance.gameObject;
            go.SetActive(false);

            var tr = instance.transform;
            tr.SetParent(_poolRoot);

            _inactive.Push(instance);
            _activeCount = Mathf.Max(0, _activeCount - 1);
        }
        
        public void Clear(bool destroy = false)
        {
            while (_inactive.Count > 0)
            {
                var inst = _inactive.Pop();
                if (inst == null) continue;

                if (destroy) Object.Destroy(inst.gameObject);
                else
                {
                    // На случай, если кто-то удалил родителя
                    var tr = inst.transform;
                    tr.SetParent(_poolRoot);
                    inst.gameObject.SetActive(false);
                }
            }
        }
        
        private void PrepareAsInactive(T inst)
        {
            var tr = inst.transform;
            tr.SetParent(_poolRoot);
            inst.gameObject.SetActive(false);
            inst.OnDespawned(); 
        }
    }
}