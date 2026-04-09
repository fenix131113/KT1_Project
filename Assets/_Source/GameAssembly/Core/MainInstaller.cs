using GameAssembly.Core.Data;
using GameAssembly.EnemySystem;
using GameAssembly.Game;
using GameAssembly.PlayerSystem;
using GameAssembly.ReplaySystem;
using GameAssembly.ScoresSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameAssembly.Core
{
    public class MainInstaller : LifetimeScope
    {
        [SerializeField] private LayersDataSO layersDataSO;

        private InputSystem_Actions _playerInput;
        
        protected override void Configure(IContainerBuilder builder)
        {
            #region Core

            builder.RegisterInstance(layersDataSO).As<LayersDataSO>();
            builder.Register<ReplayClock>(Lifetime.Singleton)
                .AsSelf()
                .As<IFixedTickable>();
            builder.Register<DeterministicRng>(Lifetime.Singleton)
                .As<IRng>();
            builder.Register<ReplayController>(Lifetime.Singleton)
                .AsSelf()
                .As<IInitializable>()
                .As<IFixedTickable>();
            builder.Register<ReplayHotkeys>(Lifetime.Singleton)
                .As<IInitializable>()
                .As<ITickable>();

            #endregion
            
            #region Player

            builder.RegisterComponentInHierarchy<Player>();
            builder.RegisterComponentInHierarchy<Player>().As<IPositionGetter>();
            _playerInput = new InputSystem_Actions();
            builder.RegisterInstance(_playerInput);
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .AsSelf()
                .As<IInitializable>()
                .As<IFixedTickable>();

            #endregion
            #region Game
            
            builder.Register<GameRestart>(Lifetime.Singleton);
            builder.Register<Score>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<EnemySpawner>();
            builder.RegisterComponentInHierarchy<ScoresSpawner>();

            #endregion
        }
    }
}
