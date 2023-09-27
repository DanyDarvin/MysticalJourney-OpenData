using Common.Npc.Enemy.Abstract.Components;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Npc.Enemy.Components
{
    public class EnemyAnimator : MonoBehaviour, IEnemyAnimator
    {
        [SerializeField] private EnemyParent _enemyParent;
        [SerializeField] private Animator _animator;

        private static readonly int AttackHash = Animator.StringToHash("Attacking");
        private static readonly int SpeedHash = Animator.StringToHash("Walking");
        private static readonly int IsMovingHash = Animator.StringToHash("IsWalking");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DieHash = Animator.StringToHash("Die");

        //todo get from data
        private readonly float _minimalVelocity = 0.1f;
        private readonly float _hitDelay = 2f;
        private Tweener _hitDelayTweener;

        private void OnEnable()
        {
            _enemyParent.Health().Current.Subscribe(_ => { OnHealthChanged(); }).AddTo(this);
            _enemyParent.Death().OnRelease.Subscribe(OnReleased).AddTo(this);
        }

        private void Update()
        {
            if (CanMove())
                Move(_enemyParent.Agent().velocity.magnitude);
            else
                StopMoving();
        }

        private void Move(float speed)
        {
            _animator.SetBool(IsMovingHash, true);
            _animator.SetFloat(SpeedHash, speed);
        }


        private void OnHealthChanged()
        {
            Hit();
            StopAgentMovement();
        }

        private void StopAgentMovement()
        {
            if (CanMove()) _enemyParent.Agent().isStopped = true;

            _hitDelayTweener = DOVirtual
                .Float(0f, _hitDelay, _hitDelay, _ => { })
                .OnComplete(() =>
                {
                    if (_enemyParent.Health().Current.Value > 0) _enemyParent.Agent().isStopped = false;
                })
                .SetAutoKill();
        }

        private bool CanMove() =>
            _enemyParent.Agent().isActiveAndEnabled &&
            _enemyParent.Agent().velocity.magnitude > _minimalVelocity &&
            _enemyParent.Agent().remainingDistance > _enemyParent.Agent().radius;

        private void OnReleased(Unit unit) => Death();
        private void Hit() => _animator.SetTrigger(HitHash);
        private void Death() => _animator.SetTrigger(DieHash);
        private void StopMoving() => _animator.SetBool(IsMovingHash, false);
        public void Attacking() => _animator.SetTrigger(AttackHash);

        private void OnDisable()
        {
            _hitDelayTweener?.Kill();
        }
    }
}