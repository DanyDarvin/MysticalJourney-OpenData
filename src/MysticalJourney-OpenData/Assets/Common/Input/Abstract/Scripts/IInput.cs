using UnityEngine;

namespace Common.Input.Abstract
{
    public interface IInput
    {
        Vector2 MoveAxis { get; }
        Vector2 LookAxis { get; }

        bool IsAttackButtonDown();
    }
}