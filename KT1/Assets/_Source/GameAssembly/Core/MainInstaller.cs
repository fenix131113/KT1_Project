using GameAssembly.EnemySystem;
using GameAssembly.Game;
using GameAssembly.PlayerSystem;
using GameAssembly.ScoresSystem;
using VContainer;
using VContainer.Unity;

namespace GameAssembly.Core
{
    public class MainInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            #region Player

            builder.RegisterComponentInHierarchy<Player>();
            builder.Register<InputSystem_Actions>(Lifetime.Singleton);
            builder.Register<PlayerInput>(Lifetime.Singleton)
                .AsSelf()
                .As<IInitializable>()
                .As<ITickable>();

            #endregion
            #region Game
            
            builder.Register<GameRestart>(Lifetime.Singleton);
            builder.Register<Scores>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<EnemySpawner>();
            builder.RegisterComponentInHierarchy<ScoresSpawner>();

            #endregion
        }
    }
}
