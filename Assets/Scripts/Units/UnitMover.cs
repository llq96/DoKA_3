using UnityEngine;
using UnityEngine.AI;

namespace VladB.Doka
{
    public class UnitMover : MonoBehaviour
    {
        private Unit _unit;

        [SerializeField] private NavMeshAgent _agent;
        private Vector3 _lastDestination;
        private float _lastSpeed;


        public float MaxSpeed
        {
            get => _agent.speed;
            set => _agent.speed = value;
        }

        public float CurrentSpeed => _agent.velocity.magnitude;

        public void Init(Unit unit)
        {
            _unit = unit;
        }

        private void Update()
        {
            _lastSpeed = CurrentSpeed;
            _unit.Model.Set_MoveSpeed(_lastSpeed);
        }

        public void MoveTo(Vector3 destination)
        {
            if (_agent.isOnNavMesh)
            {
                if (_agent.isStopped)
                {
                    _agent.isStopped = false;
                }

                _lastDestination = destination;
                _agent.SetDestination(destination);
            }
        }
    }
}