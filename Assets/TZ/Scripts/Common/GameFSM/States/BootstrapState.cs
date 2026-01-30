using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Common.GameFSM
{
    public class BootstrapState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;

        public BootstrapState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public async UniTask Enter()
        {
            await SceneManager.LoadSceneAsync("MenuScene").ToUniTask();

            _gameStateMachine.Enter<MenuState>().Forget();
        }

        public async UniTask OnExit()
        {
            await UniTask.Yield();
        }
    }
}
