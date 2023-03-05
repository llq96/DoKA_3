using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace VladB.Doka
{
    public class UnitsManager : MonoBehaviour
    {
        [SerializeField] private Transform _unitsParent;

        private List<Unit> _units;
        public ReadOnlyCollection<Unit> Units => _units.AsReadOnly();

        public Player Player { get; private set; }

        public void Init()
        {
            _units = _unitsParent.GetComponentsInChildren<Unit>(true).ToList();
            _units.ForEach(x => x.Init());
            Player = _units.First(x => x is Player) as Player;
        }
    }
}