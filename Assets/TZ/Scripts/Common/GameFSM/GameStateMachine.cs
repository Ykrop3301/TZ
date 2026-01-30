using Common.AssetsProvider;
using Common.Curtain;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Common.GameFSM
{
    public class GameStateMachine : IGameStateMachine
    {
        public Dictionary<System.Type, IGameState> _states;
        private IGameState _currentState;

        public GameStateMachine(ICurtain curtain, IAssetsProvider assetsProvider)
        {
            _states = new Dictionary<System.Type, IGameState>()
            {
                { typeof(BootstrapState), new BootstrapState(this) },
                { typeof(MenuState), new MenuState(curtain, assetsProvider) },
                { typeof(GameplayState), new GameplayState(curtain, assetsProvider) },
            };
        }

        public async UniTask Enter<T>() where T : IGameState
        {
            if (_currentState != null)
                await _currentState.OnExit();

            _currentState = _states[typeof(T)];

            _currentState.Enter().Forget();
        }
    }
}
