using System;
using TMPro;

namespace VladB.Utility
{
    public class VariableUI_String : VariableUI<String>
    {
        private TextMeshProUGUI _tmp;

        public TextMeshProUGUI TMP
        {
            get
            {
                if (_tmp == null) _tmp = GetComponent<TextMeshProUGUI>();
                return _tmp;
            }
        }

        protected override void OnEnableFunc()
        {
            base.OnEnableFunc();
            UpdateVariableUI();
        }

        protected override void UpdateVariableValue(string newValue, string oldValue, string deltaValue)
        {
            base.UpdateVariableValue(newValue, oldValue, deltaValue);

            UpdateVariableUI();
            // Debug.Log($"{variableValue}");
        }

        public override void UpdateVariableUI()
        {
            if (TMP)
            {
                TMP.text = $"{prefix}{variableValue}{postfix}";
            }
        }
    }
}