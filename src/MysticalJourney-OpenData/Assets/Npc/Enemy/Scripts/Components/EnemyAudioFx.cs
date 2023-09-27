using Common.Npc.Enemy.Abstract.Components;
using UnityEngine;

namespace Npc.Enemy.Components
{
    public class EnemyAudioFx : MonoBehaviour, IEnemyAudioFx
    {
        [SerializeField] private AudioSource _fxAudioSource;
        [SerializeField] private AudioClip _hitFxAudioClip;
        [SerializeField] private AudioClip _attackFxAudioClip;
        [SerializeField] private AudioClip _idleFxAudioClip;

        public void Hitting()
        {
            _fxAudioSource.clip = _hitFxAudioClip;
            _fxAudioSource.Play();
        }

        public void Attacking()
        {
            _fxAudioSource.clip = _attackFxAudioClip;
            _fxAudioSource.Play();
        }

        public void Idling()
        {
            if (_fxAudioSource.isPlaying) return;
            _fxAudioSource.clip = _idleFxAudioClip;
            _fxAudioSource.Play();
        }
    }
}