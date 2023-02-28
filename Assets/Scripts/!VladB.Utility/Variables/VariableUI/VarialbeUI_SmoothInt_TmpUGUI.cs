using TMPro;

namespace VladB.Utility
{
    public class VarialbeUI_SmoothInt_TmpUGUI : VariableUI_SmoothNumber<int>
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
        }

        public override void UpdateVariableUI()
        {
            base.UpdateVariableUI();
            if (TMP)
            {
                TMP.text = $"{prefix}{(int)smoothVariableValue}{postfix}";
            }
        }
    }
}