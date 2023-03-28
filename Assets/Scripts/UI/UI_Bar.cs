using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VladB.Doka.UI
{
    public class UI_Bar : MonoBehaviour
    {
        [SerializeField] private Image _barImage;
        [SerializeField] private TextMeshProUGUI _tmp_value;
        [SerializeField] private TextMeshProUGUI _tmp_incrementValue;

        public void SetValues(float currentValue, float maxValue, float incrementValue)
        {
            _barImage.fillAmount = maxValue != 0 ? currentValue / maxValue : 0;
            _tmp_value.text = $"{currentValue} / {maxValue}";
            _tmp_incrementValue.text = $"+ {incrementValue}";
        }
    }
}