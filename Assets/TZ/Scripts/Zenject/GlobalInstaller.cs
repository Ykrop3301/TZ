using Common.AssetsProvider;
using Common.Curtain;
using Common.GameFSM;
using UnityEngine;

namespace Zenject
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private Curtain _curtain;
        public override void InstallBindings() 
        {
            Container.BindInterfacesAndSelfTo<AddressablesAssetsProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<Curtain>().FromInstance(_curtain).AsSingle();
          
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        }
    }
}