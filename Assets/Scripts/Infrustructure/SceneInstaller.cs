using Transition;
using UnityEngine;
using Zenject;

namespace Infrustructure
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        public override void InstallBindings()
        {
            Container.Bind<TransisionManager>().FromInstance(new TransisionManager()).AsSingle();
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
        }
    }
}