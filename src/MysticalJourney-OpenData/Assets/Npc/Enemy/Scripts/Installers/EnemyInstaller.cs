using Common.Npc.Enemy.Abstract.Ai;
using Common.Npc.Enemy.Abstract.Components;
using Common.Stats.Abstract;
using Npc.Enemy.Ai;
using Npc.Enemy.Components;
using Npc.Enemy.Factories;
using Npc.Enemy.Spawners;
using Npc.Enemy.Stats;
using StaticData.Models;
using UnityEngine;
using Zenject;

namespace Npc.Enemy.Installers
{
    [CreateAssetMenu(fileName = "EnemyInstaller", menuName = "Installers/EnemyInstaller")]
    public class EnemyInstaller : ScriptableObjectInstaller<EnemyInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IHealth>().WithId(GameEntityType.Enemy).To<EnemyHealth>().AsTransient().Lazy();
            Container.Bind<IEnemyDeath>().To<EnemyDeath>().AsTransient().Lazy();
            Container.Bind<IAiBehaviour<IEnemy>>().To<EnemyAiBehaviour>().AsTransient().Lazy();
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle().NonLazy();
        }
    }
}