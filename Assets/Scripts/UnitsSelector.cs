using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace VladB.Doka
{
    public class UnitsSelector : MonoBehaviour
    {
        public ReadOnlyCollection<Unit> SelectedUnits => _currentUnits.AsReadOnly();
        public Unit FirstSelectedUnit => _currentUnits.FirstOrDefault();

        private List<Unit> _currentUnits = new();

        public Action OnChangedSelectedUnit;

        public void Init()
        {
            MainController.Instance.TouchRaycaster.OnHitInUnit += (_, unit) => SelectUnit(unit);
        }

        public void SelectUnits(IEnumerable<Unit> units)
        {
            _currentUnits.Clear();
            _currentUnits.AddRange(units);

            // Debug.Log($"Current Unit = {FirstSelectedUnit}");

            OnChangedSelectedUnit?.Invoke();
        }

        public void SelectUnit(Unit unit)
        {
            SelectUnits(new[] { unit });
        }
    }
}