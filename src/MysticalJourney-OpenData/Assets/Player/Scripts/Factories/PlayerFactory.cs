using System.Threading;
using Common.Assets.Addressable;
using Common.Input.Abstract;
using Common.Player.Abstract.Components;
using Common.Player.Abstract.Factories;
using Common.Player.Abstract.Hud;
using Cysharp.Threading.Tasks;
using StaticData;
using UnityEngine;
using Zenject;

namespace Player.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Player => _player;

        private DiContainer _container;
        private IAssets _assets;
        private IStaticData _staticData;
        private IPlayer _player;
        private IHud _hud;

        [Inject]
        public PlayerFactory(
            DiContainer container,
            IAssets assets,
            IStaticData staticData
        )
        {
            _staticData = staticData;
            _assets = assets;
            _container = container;
        }

        public async UniTask<IHud> InitializeHud(IPlayer player, CancellationToken cancellationToken)
        {
            _hud = await CreateHud(cancellationToken);
            _hud.Initialize(player.Health(), player.Armor(), player.PlayerAttack());

            return _hud;
        }

        public async UniTask<IPlayer> InitializePlayer(string sceneName, CancellationToken cancellationToken)
        {
            _player = await CreatePlayer(cancellationToken);

            var playerStaticData = _staticData.ForPlayer();
            var levelStaticData = _staticData.ForLevel(sceneName);

            _player.Health().Initialize(playerStaticData.HP, playerStaticData.HP);
            _player.Armor().Initialize(playerStaticData.Armor, playerStaticData.Armor);

            _player.PlayerAttack().Initialize(_player,
                playerStaticData.AttackRange,
                playerStaticData.VerticalRange,
                playerStaticData.HorizontalRange,
                playerStaticData.AttackRate,
                playerStaticData.AttackDelay,
                playerStaticData.AttackDamage
            );
            _player.CharacterController().enabled = false;
            _player.Transform().position = levelStaticData.InitialHeroPosition;
            _player.Transform().rotation = Quaternion.identity;

            _container.Resolve<IMouseLook>().Initialize(_player.Transform(), _player.Camera().transform);
            _container.Resolve<IMove>().Initialize(_player);
            _container.Resolve<IPlayerDeath>().Initialize();

            _player.CharacterController().enabled = true;
            return _player;
        }

        public void DestroyPlayer() => Object.DestroyImmediate(_player.Transform().gameObject);

        public void DestroyHud() => Object.DestroyImmediate(_hud.Transform().gameObject);

        private async UniTask<IPlayer> CreatePlayer(CancellationToken cancellationToken) =>
            _container.InstantiatePrefabForComponent<IPlayer>(await LoadPlayer(cancellationToken));

        private async UniTask<IHud> CreateHud(CancellationToken cancellationToken) =>
            _container.InstantiatePrefabForComponent<IHud>(await LoadHud(cancellationToken));

        private async UniTask<GameObject> LoadPlayer(CancellationToken cancellationToken) =>
            await _assets.Load<GameObject>(_staticData.ForPlayer().PlayerPrefabReference, cancellationToken);

        private async UniTask<GameObject> LoadHud(CancellationToken cancellationToken) =>
            await _assets.Load<GameObject>(_staticData.ForPlayer().HudPrefabReference, cancellationToken);
    }
}