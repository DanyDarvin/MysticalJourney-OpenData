using Common.Input.Abstract;
using Common.Player.Abstract.Components;
using Common.Stats.Abstract;
using StaticData.Models;
using UnityEngine;
using Zenject;

namespace Player.Components
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerParent : MonoBehaviour, IPlayer
    {
        [field: SerializeField] private PlayerAnimator _animator;
        [field: SerializeField] private PlayerAudioFx _playerAudioFx;
        [SerializeField] private Camera _camera;
        [SerializeField] private CharacterController _characterController;
        private IHealth _health;
        private IArmor _armor;
        private IPlayerAttack _playerAttack;

        [Inject]
        public void Construct(
            [Inject(Id = GameEntityType.Player)] IHealth health,
            [Inject(Id = GameEntityType.Player)] IArmor armor,
            IPlayerAttack playerAttack,
            IInput input
        )
        {
            _playerAttack = playerAttack;
            _armor = armor;
            _health = health;

            _animator.Construct(input, this);
        }

        public Transform Transform() => transform;
        public IPlayerAnimator Animator() => _animator;
        public IPlayerAudioFx AudioFx() => _playerAudioFx;
        public Camera Camera() => _camera;
        public CharacterController CharacterController() => _characterController;
        public IPlayerAttack PlayerAttack() => _playerAttack;
        public IHealth Health() => _health;
        public IArmor Armor() => _armor;

        private void OnValidate()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void TakeDamage(float damage, Vector3 point)
        {
            _playerAudioFx.Hitting();
            DamageImpact(damage);
        }

        private void DamageImpact(float damage)
        {
            if (_armor.IsNotEmpty)
            {
                if (_armor.Current.Value >= damage)
                {
                    _armor.Take(damage);
                }
                else if (_armor.Current.Value < damage)
                {
                    var remainingDamage = damage - _armor.Current.Value;
                    _health.Take(remainingDamage);
                }
            }
            else
            {
                _health.Take(damage);
            }
        }
    }
}