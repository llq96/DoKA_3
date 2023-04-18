using UnityEngine;

namespace VladB.Doka
{
    [RequireComponent(typeof(UnitComponents))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitComponents _components;

        public UnitMover Mover => _components.Mover;
        public UnitEffectsReceiver EffectsReceiver => _components.EffectsReceiver;
        public UnitStats Stats => _components.Stats;
        public UnitUI UI => _components.UI;
        public UnitVisibility Visibility => _components.Visibility;
        public UnitModel Model => _components.Model;

        public virtual void Init()
        {
            EffectsReceiver.Init(this);
            Stats.Init(this);
            UI.Init(this);
            Visibility.Init(this);
            Model.Init(this);
            Mover.Init(this);
        }
    }
}