using UnityEngine;

namespace VladB.Doka
{
    public class UnitModel : MonoBehaviour
    {
        private Unit _unit;

        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;

        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        public void Init(Unit unit)
        {
            _unit = unit;
        }

        // public void PlayAnimation_Run()
        // {
        //     Debug.Log("Run");
        //     _animator.SetTrigger(Run);
        // }
        //
        // public void PlayAnimation_Idle()
        // {
        //     _animator.SetTrigger(Idle);
        // }

        public void Set_MoveSpeed(float value)
        {
            _animator.SetFloat(MoveSpeed, value);
        }
    }
}