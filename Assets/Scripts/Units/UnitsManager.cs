using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace VladB.Doka
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private UnitsSelector _unitsSelector;
        public UnitsSelector UnitsSelector => _unitsSelector;

        [SerializeField] private Transform _unitsParent;

        private List<Unit> _allUnits;
        public ReadOnlyCollection<Unit> AllUnits => _allUnits.AsReadOnly();

        public Player Player { get; private set; }

        public void Init()
        {
            _allUnits = _unitsParent.GetComponentsInChildren<Unit>(true).ToList();
            _allUnits.ForEach(x => x.Init());
            Player = _allUnits.First(x => x is Player) as Player;

            UnitsSelector.Init();
        }
    }
}