using Common.AssetsProvider;
using Cysharp.Threading.Tasks;

namespace Menu.Gallery
{
    public class GalleryVisualizer : MenuElement
    {
        public override async UniTask Initialize(IAssetsProvider assetsProvider)
        {
            await UniTask.CompletedTask;
        }
    }
}
