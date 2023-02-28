using UnityEngine;
using UnityEngine.AI;

namespace VladB.Doka
{
    public class UnitMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        public float Speed
        {
            get => _agent.speed;
            set => _agent.speed = value;
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.destination = destination;
        }
    }
}