using UniRx;
using UnityEngine;

namespace VladB.Doka.UI
{
    public class UI_DownPanel : MonoBehaviour
    {
        private Unit _unit;
        private static UnitsSelector UnitsSelector => MainController.Instance.UnitsManager.UnitsSelector;

        [SerializeField] private UI_Bar _bar_hp;
        [SerializeField] private UI_Bar _bar_mana;

        private CompositeDisposable _disposables = new();

        public void Init()
        {
            UnitsSelector.OnChangedSelectedUnit += OnChangedSelectedUnit;
        }

        private void OnChangedSelectedUnit()
        {
            SetUnit(UnitsSelector.FirstSelectedUnit);
        }

        private void SetUnit(Unit unit)
        {
            //TODO Scipt for Stat
            _disposables.Dispose();

            if (unit == null) return;

            _unit = unit;
            _unit.Stats.Hp.Subscribe((_) => UpdateUI());
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Debug.Log($"Update Down Panel: {_unit}");
            _bar_hp.SetValues(_unit.Stats.Hp_Current, _unit.Stats.Hp_Max, 0); //TODO regen info
            _bar_mana.SetValues(123, 456, 7); //TODO
        }
    }
}