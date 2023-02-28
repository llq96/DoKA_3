using System;
using UnityEngine;

namespace VladB.Utility
{
    [Serializable]
    public class Variable
    {
    }

    [Serializable]
    public class Variable<T> : Variable
    {
        [SerializeField] protected T defaulValue;

        protected bool IsInited;

        public Action OnValueChanged;

        /// <summary>
        /// NewValue
        /// </summary>
        public Action<T> OnValueChanged_Ev1;

        /// <summary>
        /// NewValue, OldValue
        /// </summary>
        public Action<T, T> OnValueChanged_Ev2;

        /// <summary>
        /// NewValue, OldValue , Delta(If can)
        /// </summary>
        public Action<T, T, T> OnValueChanged_Ev3;

        protected T __value;
        protected T __oldValue;

        public virtual T Value
        {
            get
            {
                CheckInit(true);
                return __value;
            }
            set
            {
                CheckInit(false);

                __oldValue = __value;
                __value = value;
                SendEvents(__value, __oldValue);
            }
        }

        protected virtual void CheckInit(bool isChangeValueToDefault)
        {
            if (!IsInited)
            {
                IsInited = true;
                if (isChangeValueToDefault)
                {
                    Value = defaulValue;
                }
            }
        }

        protected virtual void SendEvents(T newValue, T oldValue)
        {
            OnValueChanged?.Invoke();
            OnValueChanged_Ev1?.Invoke(newValue);
            OnValueChanged_Ev2?.Invoke(newValue, oldValue);
        }
    }
}