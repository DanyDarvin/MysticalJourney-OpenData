using UnityEngine;
using Zenject;

namespace Common.Input.Abstract
{
    public interface IMouseLook : ITickable
    {
        Vector2 MouseDirection();
        void Look(Vector2 mouseDirection);
        void Initialize(Transform target, Transform camera);
        void Enable();
        void Disable();
    }
}