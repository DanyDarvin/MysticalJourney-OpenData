using Common.Input.Abstract;
using Common.Player.Abstract.Components;
using StaticData;
using UnityEngine;
using Zenject;

namespace Player.Components
{
    public class PlayerMove : IMove
    {
        private IInput _inputService;
        private IPlayer _player;

        private float movementSpeed = 10f;
        private float movementDamping = 5f;
        private Vector3 _movementVector;
        private bool _isActive;

        public void Enable() => _isActive = true;
        public void Disable() => _isActive = false;

        [Inject]
        public void Construct(IInput inputService)
        {
            _inputService = inputService;
        }

        public void Initialize(IPlayer player)
        {
            _player = player;
        }

        public void Tick()
        {
            if (_isActive) Move(MovementVector());
        }

        public Vector3 MovementVector()
        {
            if (_inputService.MoveAxis.sqrMagnitude > Constants.Epsilon)
            {
                var inputVector = new Vector3(_inputService.MoveAxis.x, 0f, _inputService.MoveAxis.y);
                _movementVector = _player.Transform().TransformDirection(inputVector);
                _movementVector.Normalize();
            }
            else
            {
                _movementVector = Vector3.Lerp(_movementVector, Vector3.zero, movementDamping * Time.deltaTime);
            }

            return _movementVector * movementSpeed + Physics.gravity;
        }

        public void Move(Vector3 movementVector)
        {
            _player.CharacterController().Move(movementVector * Time.deltaTime);
        }
    }
}