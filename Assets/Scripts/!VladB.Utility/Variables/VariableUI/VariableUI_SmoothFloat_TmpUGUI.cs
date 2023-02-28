using TMPro;

namespace VladB.Utility
{
    public class VariableUI_SmoothFloat_TmpUGUI : VariableUI_SmoothNumber<float>
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

        public bool isForceToInt;

        protected override void OnEnableFunc()
        {
            base.OnEnableFunc();
        }

        public override void UpdateVariableUI()
        {
            base.UpdateVariableUI();
            if (TMP)
            {
                if (!isForceToInt)
                {
                    TMP.text = $"{prefix}{smoothVariableValue}{postfix}";
                }
                else
                {
                    TMP.text = $"{prefix}{(int)smoothVariableValue}{postfix}";
                }
            }
        }
    }
}