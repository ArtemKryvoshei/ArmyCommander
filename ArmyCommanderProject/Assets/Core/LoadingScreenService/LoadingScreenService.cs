using Core.PrefabFactory;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Content.Features.LoadingScreenService
{
    public class LoadingScreenService : ILoadingScreenService
    {
        private readonly IPrefabFactory _factory;
        private GameObject _prefabToSpawn;
        private GameObject _instance;

        public LoadingScreenService(IPrefabFactory factory, GameObject prefab)
        {
            _factory = factory;
            _prefabToSpawn = prefab;
        }

        public async UniTask ShowAsync()
        {
            if (_instance == null)
            {
                _instance = await _factory.CreateAsync(_prefabToSpawn);
                GameObject.DontDestroyOnLoad(_instance);
            }

            _instance.SetActive(true);
        }

        public void Hide()
        {
            if (_instance != null)
            {
                _instance.SetActive(false);
            }
        }
    }
}