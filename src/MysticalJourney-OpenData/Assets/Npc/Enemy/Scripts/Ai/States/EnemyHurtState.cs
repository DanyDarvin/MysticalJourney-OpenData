using Common.Npc.Enemy.Abstract.Ai;
using Common.Npc.Enemy.Abstract.Components;
using Common.Player;
using Common.Player.Abstract.Components;
using DG.Tweening;
using Npc.Enemy.Components;
using StaticData.Models;
using UnityEngine;
using UnityEngine.AI;
using static StaticData.Constants;

namespace Npc.Enemy.Ai.States
{
    public class EnemyHurtState : IState
    {
        private readonly EnemyAiBehaviour _enemyAiBehaviour;
        private readonly NavMeshAgent _agent;
        private readonly IEnemy _entity;
        private readonly IPlayer _player;

        private Tweener _hurtCooldownTweener;
        private bool _isAttacking;

        public EnemyHurtState(EnemyAiBehaviour enemyAiBehaviour)
        {
            _enemyAiBehaviour = enemyAiBehaviour;
            _entity = _enemyAiBehaviour.Entity();
            _agent = _entity.Agent();
            _player = _entity.Player();
        }

        public void OnEnter()
        {
            _entity.AggroTriggerObserver().TriggerExit += OnAggroTriggerExit;
            _entity.HurtTriggerObserver().TriggerExit += OnHurtTriggerExit;
        }

        public void OnUpdate()
        {
            SetDestinationForAgent();
            if (CanAttack())
            {
                StartAttack();
            }
        }

        public void OnExit()
        {
            _entity.AggroTriggerObserver().TriggerExit -= OnAggroTriggerExit;
            _entity.HurtTriggerObserver().TriggerExit -= OnHurtTriggerExit;
        }

        private void OnAggroTriggerExit(Collider obj)
        {
            if (obj.CompareTag(PlayerTag) is false) return;

            SwitchIdleState();
        }

        private void OnHurtTriggerExit(Collider obj)
        {
            if (obj.CompareTag(PlayerTag) is false) return;

            SwitchChaseState();
        }

        private void StartAttack()
        {
            var lookPosition = _player.Transform().position - _entity.Transform().position;
            lookPosition.y = 0;

            _entity.Transform()
                .DORotateQuaternion(
                    Quaternion.LookRotation(lookPosition, _entity.Transform().up), 0.25f)
                .SetAutoKill(true);

            _entity.Animator().Attacking();
            _entity.AudioFx().Attacking();
            _player.TakeDamage(_entity.Damage, _player.Transform().position);

            AttackCooldown();
        }

        private void AttackCooldown()
        {
            _isAttacking = true;
            _hurtCooldownTweener = DOVirtual.Float(0f, _entity.AttackCooldown, _entity.AttackCooldown, _ => { })
                .OnComplete(() => { _isAttacking = false; })
                .SetAutoKill(true);
        }

        private void SetDestinationForAgent()
        {
            if (_player.Transform() &&
                IsPlayerNotReached() &&
                _enemyAiBehaviour.Entity().Health().IsNotEmpty &&
                _agent.isOnNavMesh)
                _agent.destination = _player.Transform().position;
        }

        private bool IsPlayerNotReached() =>
            Vector3.SqrMagnitude(_player.Transform().position - _agent.transform.position) >=
            _entity.MinimalChaseDistance * _entity.MinimalChaseDistance;

        private void SwitchIdleState() => _enemyAiBehaviour.ChangeState(EnemyStateType.Idle);

        private void SwitchChaseState() => _enemyAiBehaviour.ChangeState(EnemyStateType.Chase);

        private bool CanAttack() => !_isAttacking && _enemyAiBehaviour.Entity().Health().IsNotEmpty &&
                                    _enemyAiBehaviour.Entity().Player().Health().IsNotEmpty;

        public void Dispose()
        {
            _hurtCooldownTweener?.Kill();
        }
    }
}