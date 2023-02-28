using UnityEngine;

namespace VladB.Utility
{
    [System.Serializable]
    public class VariableIntClamped : VariableInt
    {
        public int minValue;
        public int maxValue = int.MaxValue;

        public override int Value
        {
            get => base.Value;
            set => base.Value = Mathf.Clamp(value, minValue, maxValue);
        }
    }


    [System.Serializable]
    public class VariableFloatClamped : VariableFloat
    {
        public float minValue;
        public float maxValue = float.MaxValue;

        public override float Value
        {
            get => base.Value;
            set => base.Value = Mathf.Clamp(value, minValue, maxValue);
        }
    }
}