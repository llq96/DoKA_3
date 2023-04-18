using UniRx;
using UnityEngine;

namespace VladB.Doka
{
    public class UnitStats : MonoBehaviour
    {
        private Unit _unit;

        //TODO Вынести в отдельный класс всё про hp
        [SerializeField] private ReactiveProperty<float> _hp;
        [SerializeField] private ReactiveProperty<float> _maxHp;

        public IReadOnlyReactiveProperty<float> Hp => _hp;
        public IReadOnlyReactiveProperty<float> MaxHp => _maxHp;

        public float Hp_Current => _hp.Value;
        public float Hp_Max => _maxHp.Value;
        public float Hp_Clamped01 => _hp.Value / _maxHp.Value;

        public void Init(Unit unit)
        {
            _unit = unit;
            _hp.Subscribe((_) => CheckHp());
            _hp.Value = _maxHp.Value;
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
            _maxHp.Value = maxHp;
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