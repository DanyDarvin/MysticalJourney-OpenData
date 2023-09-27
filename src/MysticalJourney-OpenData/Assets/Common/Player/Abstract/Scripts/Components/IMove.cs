using UnityEngine;
using Zenject;

namespace Common.Player.Abstract.Components
{
    public interface IMove : ITickable
    {
        Vector3 MovementVector();
        void Move(Vector3 movementVector);
        void Initialize(IPlayer player);
        void Enable();
        void Disable();
    }
}