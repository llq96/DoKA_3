using UnityEngine;

namespace VladB.Doka
{
    public class InputManager_PC : MonoBehaviour
    {
        private Player _player;
        private GameCamera _gameCamera;

        public void Init()
        {
            _player = MainController.Instance.UnitsManager.Player;
            _gameCamera = MainController.Instance.GameCamera;

            MainController.Instance.TouchRaycaster.OnHitInMovementMask += OnHitInMovementMask;
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