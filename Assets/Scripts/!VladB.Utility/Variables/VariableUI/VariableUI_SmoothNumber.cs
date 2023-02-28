using System;
using UnityEngine;

namespace VladB.Utility
{
    public abstract class VariableUI_SmoothNumber<T> : VariableUI<T>
    {
        public float minChangeSpeed = 100f;

        protected float smoothVariableValue = 0f;
        protected float variableValueChangeSpeed;


        protected override void UpdateVariableValue(T newValue, T oldValue, T deltaValue)
        {
            base.UpdateVariableValue(newValue, deltaValue, deltaValue);
            variableValueChangeSpeed =
                Mathf.Max(minChangeSpeed, Mathf.Abs(smoothVariableValue - Convert.ToSingle(newValue)));
        }

        public override void UpdateVariableUI()
        {
            if (smoothVariableValue != Convert.ToSingle(variableValue))
            {
                smoothVariableValue = Mathf.MoveTowards(smoothVariableValue, Convert.ToSingle(variableValue),
                    Time.unscaledDeltaTime * variableValueChangeSpeed);
            }
        }

        #region OnEnable/Update Methods

        protected override void OnEnableFunc()
        {
            base.OnEnableFunc();
            smoothVariableValue = Convert.ToSingle(variableValue);
            UpdateVariableUI();
        }

        protected override void UpdateFunc()
        {
            base.UpdateFunc();
            UpdateVariableUI();
        }

        #endregion
    }
}