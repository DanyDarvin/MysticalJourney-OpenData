using Common.GameLogic.Abstract;
using Common.Npc.Enemy.Abstract.Ai;
using Common.Npc.Enemy.Abstract.Components;
using Common.Player.Abstract.Components;
using Common.Stats.Abstract;
using GameLogic.Components;
using StaticData.Models;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Npc.Enemy.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyParent : MonoBehaviour, IEnemy
    {
        public float MinimalChaseDistance { get; set; }
        public float Damage { get; set; }
        public float AttackCooldown { get; set; }
        public float IdleCooldown { get; set; }
        public double PatrolRemainingDistance { get; set; }
        public float PatrolMoveSpeed { get; set; }
        public float ChaseMoveSpeed { get; set; }

        public EnemyTypeId EnemyTypeId => _enemyTypeId;

        public IEnemyAnimator Animator() => _enemyAnimator;
        public IEnemyAudioFx AudioFx() => _enemyAudioFx;
        public NavMeshAgent Agent() => _navMeshAgent;
        public IAiBehaviour<IEnemy> AiBehaviour() => _aiBehaviour;
        public IHealth Health() => _health;
        public IEnemyDeath Death() => _death;
        public Transform Transform() => transform;
        public IPlayer Player() => _player;

        public ITriggerObserver AggroTriggerObserver() => _aggroTriggerObserver;
        public ITriggerObserver HurtTriggerObserver() => _hurtTriggerObserver;

        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private EnemyAudioFx _enemyAudioFx;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private TriggerObserver _aggroTriggerObserver;
        [SerializeField] private TriggerObserver _hurtTriggerObserver;

        private IHealth _health;
        private IPlayer _player;
        private IAiBehaviour<IEnemy> _aiBehaviour;
        private IWaypoint[] _waypoints;
        private IEnemyDeath _death;
        private EnemyTypeId _enemyTypeId;

        [Inject]
        public void Construct(
            [Inject(Id = GameEntityType.Enemy)] IHealth health,
            IEnemyDeath death,
            IAiBehaviour<IEnemy> aiBehaviour
        )
        {
            _death = death;
            _aiBehaviour = aiBehaviour;
            _health = health;
        }

        public void Initialize(
            EnemyTypeId enemyTypeId,
            IPlayer player,
            IWaypoint[] waypoints
        )
        {
            _enemyTypeId = enemyTypeId;
            _player = player;
            _waypoints = waypoints;
        }

        private void Start()
        {
            _aiBehaviour.Initialize(this, _waypoints);
        }

        public void TakeDamage(float damage, Vector3 point)
        {
            _enemyAudioFx.Hitting();
            _health.Take(damage);
        }

        private void Update()
        {
            _aiBehaviour.Tick();
        }

        private void OnDisable()
        {
            _aiBehaviour.Dispose();
        }
    }
}