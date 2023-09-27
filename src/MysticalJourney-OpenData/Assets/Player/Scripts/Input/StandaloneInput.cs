using Common.Input.Abstract;
using UnityEngine;

namespace Player.Input
{
    public class StandaloneInput : IInput
    {
        private const string MoveHorizontal = "Horizontal";
        private const string MoveVertical = "Vertical";
        private const string MouseHorizontal = "Mouse X";
        private const string MouseVertical = "Mouse Y";
        private const string FireButton = "Fire1";

        public Vector2 MoveAxis =>
            new(UnityEngine.Input.GetAxis(MoveHorizontal), UnityEngine.Input.GetAxis(MoveVertical));

        public Vector2 LookAxis => new(UnityEngine.Input.GetAxisRaw(MouseHorizontal),
            UnityEngine.Input.GetAxis(MouseVertical));

        public bool IsAttackButtonDown() => UnityEngine.Input.GetButtonDown(FireButton);
    }
}