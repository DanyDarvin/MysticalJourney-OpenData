namespace Common.Ui.Elements.Abstract
{
    public interface IProgressBar
    {
        void Initialize(float current, float max);
        void SetValue(float current);
    }
}