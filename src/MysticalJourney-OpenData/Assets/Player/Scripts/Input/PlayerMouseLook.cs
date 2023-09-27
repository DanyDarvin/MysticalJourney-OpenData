using Common.Input.Abstract;
using UnityEngine;
using Zenject;

namespace Player.Input
{
    public class PlayerMouseLook : IMouseLook
    {
        private IInput _inputService;
        private Transform _player;
        private Transform _camera;

        //todo get from data
        private float sensitivity = 1.5f;
        private float smoothing = 1.5f;
        private float downLimit = -4f;
        private float upLimit = 25f;

        private Vector2 _currentLookDirection;
        private Quaternion _cameraInitializeLocalRotation;
        private bool _isActive;

        public void Enable() => _isActive = true;
        public void Disable() => _isActive = false;

        [Inject]
        public void Construct(IInput inputService)
        {
            _inputService = inputService;
        }

        public void Initialize(Transform target, Transform camera)
        {
            _player = target;
            _camera = camera;
            _cameraInitializeLocalRotation = _camera.localRotation;

            CursorOff();
        }

        public void Tick()
        {
            if (_isActive) Look(MouseDirection());
        }

        public Vector2 MouseDirection()
        {
            var mouseDir = _inputService.LookAxis;
            mouseDir = Vector2.Scale(mouseDir, new Vector2(sensitivity, sensitivity));

            var lookDelta = new Vector2();
            lookDelta.x = Mathf.Lerp(lookDelta.x, mouseDir.x, 1.0f / smoothing);
            lookDelta.y = Mathf.Lerp(lookDelta.y, mouseDir.y, 1.0f / smoothing);
            _currentLookDirection += lookDelta;

            _currentLookDirection.y = Mathf.Clamp(_currentLookDirection.y, downLimit, upLimit);
            return _currentLookDirection;
        }

        public void Look(Vector2 mouseDirection)
        {
            _camera.localRotation = _cameraInitializeLocalRotation *
                                    Quaternion.AngleAxis(-mouseDirection.y, Vector3.right);

            _player.localRotation = Quaternion.AngleAxis(mouseDirection.x, _player.up);
        }

        private static void CursorOff()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}