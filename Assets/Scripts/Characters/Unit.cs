using UnityEngine;

namespace VladB.Doka
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitMover _mover;

        public UnitMover Mover => _mover;
    }
}