using Common.Stats.Abstract;
using UniRx;

namespace Player.Stats
{
    public class PlayerHealth : IHealth
    {
        float IStat.Max => _max;
        public IReadOnlyReactiveProperty<float> Current => _current;
        public bool IsNotEmpty => _current.Value > 0;

        private readonly ReactiveProperty<float> _current = new();
        private float _max;

        public void Initialize(float current, float max)
        {
            _current.Value = current;
            _max = max;
        }

        public void Take(float amount)
        {
            if (_current.Value <= 0) return;

            _current.Value -= amount;
        }
    }
}