using UnityEngine;
using Zenject;

namespace Player.Input.Installers
{
    [CreateAssetMenu(fileName = "InputInstaller", menuName = "Installers/InputInstaller")]
    public class InputInstaller : ScriptableObjectInstaller<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StandaloneInput>().AsSingle();
        }
    }
}