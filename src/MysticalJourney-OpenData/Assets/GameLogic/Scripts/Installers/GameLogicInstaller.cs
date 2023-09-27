using UnityEngine;
using Zenject;

namespace GameLogic.Installers
{
    [CreateAssetMenu(fileName = "GameLogicInstaller", menuName = "Installers/GameLogicInstaller")]
    public class GameLogicInstaller : ScriptableObjectInstaller<GameLogicInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameLoop>().AsSingle().NonLazy();
        }
    }
}