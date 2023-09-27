using Common.Input.Abstract;
using Common.Player.Abstract.Components;
using UnityEngine;

namespace Player.Components
{
    public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
    {
        [SerializeField] private Animator _animator;

        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int MoveHash = Animator.StringToHash("Walking");
        private static readonly int AttackHash = Animator.StringToHash("Attacking");

        private IPlayer _player;
        private IInput _inputService;
        private int _currentStateHash;

        public void Construct(IInput inputService, IPlayer player)
        {
            _player = player;
            _inputService = inputService;
        }

        public void Update()
        {
            Walking();
        }

        public void Attacking()
        {
            _animator.SetTrigger(AttackHash);
        }

        private void Walking()
        {
            _animator.SetFloat(MoveHash, _inputService.MoveAxis.magnitude);
        }
    }
}