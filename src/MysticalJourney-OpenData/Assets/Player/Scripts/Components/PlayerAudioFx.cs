using Common.Input.Abstract;
using Common.Player.Abstract.Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Player.Components
{
    public class PlayerAudioFx : MonoBehaviour, IPlayerAudioFx
    {
        [SerializeField] private AudioSource _footstepAudioSource;
        [SerializeField] private AudioClip _footstepAudioClip;
        [SerializeField] private AudioMixerGroup _footstepAudioMixerGroup;

        [SerializeField] private AudioSource _fxAudioSource;
        [SerializeField] private AudioClip _attackFxAudioClip;
        [SerializeField] private AudioClip _screamAttackFxAudioClip;
        [SerializeField] private AudioClip _screamHitFxAudioClip;
        [SerializeField] private AudioMixerGroup _fxAudioMixerGroup;
        private IInput _input;
        private float _footstepStartSourceVolume;
        private float _fxStartSourceVolume;
        private Tweener _fadeOutTweener;

        [Inject]
        public void Construct(IInput input)
        {
            _input = input;
        }

        private void Start()
        {
            InitializeFootstepAudio();
            InitializeFxAudio();
        }

        private void InitializeFxAudio()
        {
            _fxAudioSource.outputAudioMixerGroup = _fxAudioMixerGroup;
            _fxAudioSource.clip = _attackFxAudioClip;
            _fxStartSourceVolume = _fxAudioSource.volume;
        }

        private void InitializeFootstepAudio()
        {
            _footstepAudioSource.outputAudioMixerGroup = _footstepAudioMixerGroup;
            _footstepAudioSource.clip = _footstepAudioClip;
            _footstepStartSourceVolume = _footstepAudioSource.volume;
        }

        private void FixedUpdate()
        {
            FootstepSound();
        }

        public void Attacking()
        {
            _fxAudioSource.clip = _attackFxAudioClip;
            _fxAudioSource.Play();
        }

        public void ScreamAttacking()
        {
            _fxAudioSource.clip = _screamAttackFxAudioClip;
            _fxAudioSource.Play();
        }

        public void Hitting()
        {
            _fxAudioSource.clip = _screamHitFxAudioClip;
            _fxAudioSource.Play();
        }

        private void FootstepSound()
        {
            if (_input.MoveAxis.normalized.magnitude > 0)
            {
                if (_footstepAudioSource.isPlaying is false)
                {
                    _footstepAudioSource.volume = Mathf.Clamp(_footstepStartSourceVolume, 0f, 1f);
                    _footstepAudioSource.Play();
                }
            }
            else
            {
                if (_footstepAudioSource.isPlaying)
                {
                    FadeOutAndStop(_footstepAudioSource);
                }
            }
        }

        private void FadeOutAndStop(AudioSource audioSource)
        {
            _fadeOutTweener = DOVirtual.Float(audioSource.volume, 0f, audioSource.clip.length - audioSource.time,
                    volume => { audioSource.volume = volume; })
                .OnComplete(audioSource.Stop).SetAutoKill(true);
        }

        private void OnDisable()
        {
            _fadeOutTweener.Kill();
        }
    }
}