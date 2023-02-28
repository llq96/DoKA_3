using UnityEngine;

namespace VladB.Doka
{
    public class InputManager_PC : MonoBehaviour
    {
        // [SerializeField] private Transform testTarget;

        private Player _player;

        public void Init()
        {
            _player = MainController.Instance.UnitsManager.Player;
            MainController.Instance.TouchRaycaster.OnHitInMovementMask += OnHitInMovementMask;
        }

        private void OnHitInMovementMask(RaycastHit hit)
        {
            _player.Mover.MoveTo(hit.point);
        }

        // private void Update()
        // {
        //     _player.Mover.MoveTo(testTarget.position);
        // }
    }
}