using Common.Player.Abstract.Factories;
using Common.Stats.Abstract;
using Player.Components;
using Player.Factories;
using Player.Input;
using Player.Stats;
using StaticData.Models;
using UnityEngine;
using Zenject;

namespace Player.Installers
{
    [CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Installers/PlayerInstaller")]
    public class PlayerInstaller : ScriptableObjectInstaller<PlayerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IHealth>().WithId(GameEntityType.Player).To<PlayerHealth>().AsSingle().Lazy();
            Container.Bind<IArmor>().WithId(GameEntityType.Player).To<PlayerArmor>().AsSingle().Lazy();

            Container.BindInterfacesAndSelfTo<PlayerDeath>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerAttack>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerMove>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerMouseLook>().AsSingle().NonLazy();

            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle().Lazy();
        }
    }
}