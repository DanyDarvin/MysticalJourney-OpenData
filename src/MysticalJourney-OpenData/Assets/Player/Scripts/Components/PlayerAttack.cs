using Common.GameLogic.Abstract;
using Common.Input.Abstract;
using Common.Player.Abstract.Components;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;
using Debug = Common.Utils.Debug;

namespace Player.Components
{
    public class PlayerAttack : ITickable, IPlayerAttack
    {
        private ReactiveProperty<float> _currentCooldownAmount = new();
        public IReadOnlyReactiveProperty<float> CooldownAmount => _currentCooldownAmount;

        private IPlayer _player;
        private IInput _inputService;

        private Collider[] _damageRangeHits = new Collider[3];
        private RaycastHit _rayHit;

        private int _layerMask = 1 << LayerMask.NameToLayer("Hittable");

        private float _range;
        private float _verticalRange;
        private float _horizontalRange;
        private float _attackRate;
        private float _attackDelay;
        private float _attackDamage;
        private float _cooldownAmount = 1f;

        private bool _isActive;
        private bool _isAttacking;

        public void Enable() => _isActive = true;
        public void Disable() => _isActive = false;

        public void Initialize(
            IPlayer player,
            float range,
            float verticalRange,
            float horizontalRange,
            float attackRate,
            float attackDelay,
            float attackDamage
        )
        {
            _player = player;
            _range = range;
            _verticalRange = verticalRange;
            _horizontalRange = horizontalRange;
            _attackRate = attackRate;
            _attackDelay = attackDelay;
            _attackDamage = attackDamage;

            _currentCooldownAmount.Value = _cooldownAmount;
        }


        [Inject]
        public void Construct(IInput inputService)
        {
            _inputService = inputService;
        }

        public void Tick()
        {
            if (_inputService.IsAttackButtonDown() && CanAttack())
            {
                Attack();
            }
        }

        private void Attack()
        {
            AttackCooldown();

            _player.Animator().Attacking();

            AttackProcess();
        }

        private void AttackProcess()
        {
            _player.AudioFx().ScreamAttacking();
            DOVirtual.Float(0, _attackDelay, _attackDelay, _ => { }).OnComplete(() =>
            {
                _player.AudioFx().Attacking();

                for (var rangeHit = 0; rangeHit < RangeHits(); rangeHit++)
                {
                    var playerPosition = _player.Camera().transform.position;
                    var direction = _damageRangeHits[rangeHit].transform.position - playerPosition;

                    if (DamageHit(playerPosition, direction))
                    {
                        var impactPoint = _rayHit.point;
                        Debug.DrawPointer(_rayHit.point, 0.5f, 1f);

                        if (_rayHit.transform.TryGetComponent<IDamageable>(out var damageable))
                        {
                            //Debug.Log(direction.sqrMagnitude);
                            TakeDamage(playerPosition, direction, damageable, impactPoint);
                        }
                    }
                }
            }).SetAutoKill();
        }

        private bool DamageHit(Vector3 playerPosition, Vector3 direction)
        {
            return Physics.Raycast(playerPosition, direction, out _rayHit, _range, _layerMask);
        }

        private void TakeDamage(Vector3 playerPosition, Vector3 direction, IDamageable damageable, Vector3 impactPoint)
        {
            UnityEngine.Debug.DrawRay(playerPosition, direction, Color.cyan);
            damageable.TakeDamage(_attackDamage, impactPoint);
        }

        private void AttackCooldown()
        {
            _isAttacking = true;
            DOVirtual.Float(0f, _cooldownAmount, _attackRate, OnCooldownUpdate())
                .OnComplete(() => { _isAttacking = false; })
                .SetAutoKill(true);
        }

        private TweenCallback<float> OnCooldownUpdate()
        {
            return amount => { _currentCooldownAmount.Value = amount; };
        }

        private bool CanAttack() => !_isAttacking && _isActive;

        private int RangeHits() =>
            Physics.OverlapBoxNonAlloc(
                DamageCenterPoint(),
                new Vector3(_horizontalRange, _verticalRange, _range),
                _damageRangeHits,
                _player.Transform().rotation, _layerMask);

        private Vector3 DamageCenterPoint()
        {
            return _player.Transform().position + _player.Transform().forward * (_range * 0.5f);
        }
    }
}