using Common.Ui.Elements.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Hud
{
    public class ProgressBar : MonoBehaviour, IProgressBar
    {
        [SerializeField] public Image ImageCurrent;
        private float _max;

        public void Initialize(float current, float max)
        {
            _max = max;
            Fill(current / _max);
        }

        public void SetValue(float current) => Fill(current / _max);

        private void Fill(float amount) => ImageCurrent.fillAmount = amount;
    }
}