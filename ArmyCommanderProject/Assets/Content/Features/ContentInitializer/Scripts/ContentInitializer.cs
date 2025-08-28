using System;
using Content.Features.CurrencySystem.Scripts;
using Content.Features.InputListener.Scripts;
using Content.Features.LoadingScreenService;
using Core.AudioManager;
using Core.EventBus;
using Core.Other;
using Core.PrefabFactory;
using Core.SceneLoadingCoordinator;
using Core.ServiceLocatorSystem;
using Core.UniTaskSceneLoader;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Content.Features.ContentInitializer.Scripts
{
    public class ContentInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject audioManagerPrefab;
        [SerializeField] private GameObject loadScreenPrefab;
        [SerializeField] private GameObject ingameUIPackPrefab;
        [SerializeField] private string gameplaySceneName;
        private IPrefabFactory _prefabFactory;
        private IEventBus _eventBus;
        private IAudioManager _audioManager;
        private ILoadingScreenService _loadingScreenService;
        private ISceneLoader _sceneLoader;
        private ICurrencyManager _currencyManager;
        private SceneLoadingCoordinator _sceneLoadingCoordinator;
        private IPoolRegistry _poolRegistry;

        private async void Awake()
        {
            _prefabFactory = new PrefabFactory();
            _eventBus = new EventBus();
            _sceneLoader = new SceneLoader();
            _poolRegistry = new PoolRegistry();
            _loadingScreenService = new LoadingScreenService.LoadingScreenService(_prefabFactory, loadScreenPrefab);
            _sceneLoadingCoordinator = new SceneLoadingCoordinator(_sceneLoader, _loadingScreenService);
            _currencyManager = new CurrencyManager(_eventBus);

            GameObject audioSpawnedPrefab = await _prefabFactory.CreateAsync(audioManagerPrefab);
            _audioManager = audioSpawnedPrefab.GetComponent<IAudioManager>();
            
            ServiceLocator.Register<IPrefabFactory>(_prefabFactory);
            ServiceLocator.Register<IPoolRegistry>(_poolRegistry);
            ServiceLocator.Register<IEventBus>(_eventBus);
            ServiceLocator.Register<ILoadingScreenService>(_loadingScreenService);
            ServiceLocator.Register<ISceneLoader>(_sceneLoader);
            ServiceLocator.Register<SceneLoadingCoordinator>(_sceneLoadingCoordinator);
            ServiceLocator.Register<ICurrencyManager>(_currencyManager);
            
            GameObject ingameUIPackSpawnedPrefab = await _prefabFactory.CreateAsync(ingameUIPackPrefab);
            
            
            if (_audioManager != null)
            {
                ServiceLocator.Register<IAudioManager>(_audioManager);
            }
            else
            {
                Debug.LogError("[ContentInitializer] No audio manager found.");
            }

            _eventBus.Publish(new ComponentsInitializeEnd());
            await _sceneLoadingCoordinator.LoadSceneWithUI(gameplaySceneName);
        }
    }
}