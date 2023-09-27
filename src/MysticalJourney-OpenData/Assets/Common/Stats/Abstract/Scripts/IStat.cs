using UniRx;

namespace Common.Stats.Abstract
{
    public interface IStat
    {
        IReadOnlyReactiveProperty<float> Current { get; }
        float Max { get; }
        bool IsNotEmpty { get; }
        void Initialize(float current, float max);
        void Take(float amount);
    }
}