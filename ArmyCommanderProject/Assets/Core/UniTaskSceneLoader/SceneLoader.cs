using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.UniTaskSceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        public async UniTask LoadSceneAsync(string sceneName)
        {
            var loading = SceneManager.LoadSceneAsync(sceneName);
            loading.allowSceneActivation = true;
            await loading.ToUniTask();
        }
    }
}