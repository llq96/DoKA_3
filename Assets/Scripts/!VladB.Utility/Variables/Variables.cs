using System;
using UnityEngine;

namespace VladB.Utility
{
    [Serializable]
    public class VariableInt : Variable<int>
    {
        protected override void SendEvents(int newValue, int oldValue)
        {
            base.SendEvents(newValue, oldValue);
            OnValueChanged_Ev3?.Invoke(newValue, oldValue, newValue - oldValue);
        }
    }


    [Serializable]
    public class VariableFloat : Variable<float>
    {
        protected override void SendEvents(float newValue, float oldValue)
        {
            base.SendEvents(newValue, oldValue);

            OnValueChanged_Ev3?.Invoke(newValue, oldValue, newValue - oldValue);
        }
    }


    [Serializable]
    public class VariableString : Variable<string>
    {
        protected override void SendEvents(string newValue, string oldValue)
        {
            base.SendEvents(newValue, oldValue);

            OnValueChanged_Ev3?.Invoke(newValue, oldValue, String.Empty);
        }
    }


    [Serializable]
    public class VariableVector2 : Variable<Vector2>
    {
    }


    [Serializable]
    public class VariableVector3 : Variable<Vector3>
    {
    }
}