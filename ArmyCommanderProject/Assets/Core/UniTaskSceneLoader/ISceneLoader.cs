using Cysharp.Threading.Tasks;

namespace Core.UniTaskSceneLoader
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName);
    }
}