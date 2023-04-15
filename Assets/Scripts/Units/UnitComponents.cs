using UnityEngine;

namespace VladB.Doka
{
    public class UnitComponents : MonoBehaviour
    {
        [SerializeField] private UnitMover _mover;
        public UnitMover Mover => _mover;

        [SerializeField] private UnitEffectsReceiver _effectsReceiver;
        public UnitEffectsReceiver EffectsReceiver => _effectsReceiver;

        [SerializeField] private UnitStats _stats;
        public UnitStats Stats => _stats;

        [SerializeField] private UnitUI _ui;
        public UnitUI UI => _ui;

        [SerializeField] private UnitVisibility _visibility;
        public UnitVisibility Visibility => _visibility;

        [SerializeField] private UnitModel _model;
        public UnitModel Model => _model;
    }
}