using System;
using UnityEngine;
using VladB.Utility;

namespace VladB.Doka
{
    public class UnitStats : MonoBehaviour
    {
        private Unit _unit;

        //TODO Вынести в отдельный класс всё про hp
        [SerializeField] private VariableFloatClamped _hp;
        public float Hp_Current => _hp.Value;
        public float Hp_Max => _hp.maxValue;
        public float Hp_Clamped01 => _hp.Value / _hp.maxValue;

        public Action OnValueChanged_Hp;

        public void Init(Unit unit)
        {
            _unit = unit;

            _hp.OnValueChanged += CheckHp;

            _hp.OnValueChanged += () => OnValueChanged_Hp?.Invoke();

            _hp.Value = _hp.maxValue;
        }

        public void CheckHp()
        {
            if (_hp.Value <= 0)
            {
                Debug.Log("Death");
            }

            // if(person.isAlive  && hp.value <= 0f) {
            //     person.KillByZeroHP();
            // }
        }

        public void SetMaxHp(float maxHp)
        {
            var clamped = Hp_Clamped01;
            _hp.maxValue = maxHp;
            _hp.Value = clamped * maxHp;
        }

        public void DamageUnit(float value)
        {
            _hp.Value -= value;
        }

        [ContextMenu(nameof(TestDamage))]
        private void TestDamage()
        {
            DamageUnit(123);
        }
    }
}