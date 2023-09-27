using Common.Npc.Enemy.Abstract.Ai;
using Common.Npc.Enemy.Abstract.Components;
using Npc.Enemy.Components;
using StaticData.Models;
using UnityEngine;
using UnityEngine.AI;
using static StaticData.Constants;

namespace Npc.Enemy.Ai.States
{
    public class EnemyChaseState : IState
    {
        private readonly EnemyAiBehaviour _enemyAiBehaviour;
        private readonly Transform _playerTransform;
        private readonly NavMeshAgent _agent;
        private readonly IEnemy _entity;

        public EnemyChaseState(EnemyAiBehaviour enemyAiBehaviour)
        {
            _enemyAiBehaviour = enemyAiBehaviour;
            _entity = _enemyAiBehaviour.Entity();
            _agent = _entity.Agent();
            _playerTransform = _entity.Player().Transform();
        }

        public void OnEnter()
        {
            _entity.Agent().speed = _entity.ChaseMoveSpeed;

            _entity.AggroTriggerObserver().TriggerExit += OnAggroTriggerExit;
            _entity.HurtTriggerObserver().TriggerEnter += OnHurtTriggerEnter;
        }

        public void OnUpdate()
        {
            SetDestinationForAgent();
        }

        public void OnExit()
        {
            _entity.AggroTriggerObserver().TriggerExit -= OnAggroTriggerExit;
            _entity.HurtTriggerObserver().TriggerEnter -= OnHurtTriggerEnter;
        }

        private void OnHurtTriggerEnter(Collider obj)
        {
            if (obj.CompareTag(PlayerTag) is false) return;

            SwitchHurtState();
        }

        private void OnAggroTriggerExit(Collider obj)
        {
            if (obj.CompareTag(PlayerTag) is false) return;

            SwitchIdleState();
        }

        private void SwitchIdleState() => _enemyAiBehaviour.ChangeState(EnemyStateType.Idle);
        private void SwitchHurtState() => _enemyAiBehaviour.ChangeState(EnemyStateType.Hurt);

        private void SetDestinationForAgent()
        {
            if (_playerTransform &&
                IsPlayerNotReached() &&
                _enemyAiBehaviour.Entity().Health().IsNotEmpty &&
                _agent.isOnNavMesh)
                _agent.destination = _playerTransform.position;
        }

        private bool IsPlayerNotReached() =>
            Vector3.SqrMagnitude(_playerTransform.position - _agent.transform.position) >=
            _entity.MinimalChaseDistance * _entity.MinimalChaseDistance;

        public void Dispose()
        {
            //
        }
    }
}