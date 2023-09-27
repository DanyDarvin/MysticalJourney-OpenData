using System;
using Common.GameLogic.Abstract;
using UnityEngine;

namespace GameLogic.Components
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour, ITriggerObserver
    {
        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;

        private void OnTriggerEnter(Collider other) => TriggerEnter?.Invoke(other);

        private void OnTriggerExit(Collider other) => TriggerExit?.Invoke(other);
    }
}