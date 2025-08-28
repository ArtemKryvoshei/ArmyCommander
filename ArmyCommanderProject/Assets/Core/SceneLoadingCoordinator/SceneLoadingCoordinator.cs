using Content.Features.LoadingScreenService;
using Core.Other;
using Core.UniTaskSceneLoader;
using Cysharp.Threading.Tasks;

namespace Core.SceneLoadingCoordinator
{
    public class SceneLoadingCoordinator
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingScreenService _loadingScreenService;

        public SceneLoadingCoordinator(ISceneLoader sceneLoader, ILoadingScreenService loadingScreenService)
        {
            _sceneLoader = sceneLoader;
            _loadingScreenService = loadingScreenService;
        }

        public async UniTask LoadSceneWithUI(string sceneName)
        {
            await _loadingScreenService.ShowAsync();
            
            await _sceneLoader.LoadSceneAsync(sceneName);

            await UniTask.Delay(ConstantsHolder.LOAD_SCREEN_ADDITIONAL_WAIT);
            _loadingScreenService.Hide();
        }
    }
}