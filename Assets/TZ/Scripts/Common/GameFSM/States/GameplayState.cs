using Common.AssetsProvider;
using Common.Curtain;
using Cysharp.Threading.Tasks;

namespace Common.GameFSM
{
    public class GameplayState : IGameState
    {
        private readonly ICurtain _curtain;
        private readonly IAssetsProvider _assetsProvider;


        public GameplayState(ICurtain curtain, IAssetsProvider assetsProvider)
        {
            _curtain = curtain;
            _assetsProvider = assetsProvider;
        }
        public async UniTask Enter()
        {
            await _curtain.Hide();
        }


        public async UniTask OnExit()
        {
            await _curtain.Show();
        }
    }
}
