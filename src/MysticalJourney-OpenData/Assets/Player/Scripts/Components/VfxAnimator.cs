using UnityEngine;

namespace Player.Components
{
    public class VfxAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _leftHandVFXPivot;
        [SerializeField] private Transform _rightHandVFXPivot;
        [SerializeField] private Transform _idleVfx;
        [SerializeField] private ParticleSystem _attackVfx;
        [SerializeField] private float _speed;
        [SerializeField] private bool _isRightHand;

        private void Update()
        {
            var step = _speed * Time.deltaTime;
            _idleVfx.position = _isRightHand
                ? Vector3.MoveTowards(_idleVfx.position, _rightHandVFXPivot.position, step)
                : Vector3.MoveTowards(_idleVfx.position, _leftHandVFXPivot.position, step);
        }

        public void OnLeft() => _isRightHand = false;
        public void OnRight() => _isRightHand = true;
        public void Attack() => _attackVfx.Play(true);
        private void EnableIdleVfx() => _idleVfx.gameObject.SetActive(true);
        private void DisableIdleVfx() => _idleVfx.gameObject.SetActive(false);
    }
}