using Cysharp.Threading.Tasks;

namespace Content.Features.LoadingScreenService
{
    public interface ILoadingScreenService
    {
        UniTask ShowAsync();
        void Hide();
    }
}