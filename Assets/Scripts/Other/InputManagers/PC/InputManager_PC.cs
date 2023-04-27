using UnityEngine;
using Zenject;

namespace VladB.Doka
{
    public class InputManager_PC : MonoBehaviour
    {
        private Player _player;
        [Inject] private UnitsManager _unitsManager;
        [Inject] private GameCamera _gameCamera;
        [Inject] private TouchRaycaster _touchRaycaster;

        public void Init()
        {
            _player = _unitsManager.Player;
            _touchRaycaster.OnHitInMovementMask += OnHitInMovementMask;
        }

        private void OnHitInMovementMask(RaycastHit hit)
        {
            _player.Mover.MoveTo(hit.point);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                _gameCamera.Move(Vector3.forward);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _gameCamera.Move(Vector3.back);
            }

            if (Input.GetKey(KeyCode.A))
            {
                _gameCamera.Move(Vector3.left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _gameCamera.Move(Vector3.right);
            }
        }
    }
}