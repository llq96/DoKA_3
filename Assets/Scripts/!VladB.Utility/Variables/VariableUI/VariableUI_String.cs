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

        public override void UpdateVariableUI()
        {
            if (TMP && ReactiveProperty != null)
            {
                TMP.text = $"{Prefix}{ReactiveProperty.Value}{Postfix}";
            }
        }
    }
}