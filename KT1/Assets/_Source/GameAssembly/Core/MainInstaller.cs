using GameAssembly.Core.Data;
using GameAssembly.EnemySystem;
using GameAssembly.Game;
using GameAssembly.PlayerSystem;
using GameAssembly.ScoresSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameAssembly.Core
{
    public class MainInstaller : LifetimeScope
    {
        [SerializeField] private LayersDataSO layersDataSO;
        protected override void Configure(IContainerBuilder builder)
        {
            #region Core

            builder.RegisterInstance(layersDataSO).As<LayersDataSO>();

            #endregion
            
            #region Player

            builder.RegisterComponentInHierarchy<Player>();
            builder.RegisterComponentInHierarchy<Player>().As<IPositionGetter>();
            builder.Register<InputSystem_Actions>(Lifetime.Singleton);
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .AsSelf()
                .As<IInitializable>()
                .As<ITickable>();

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
