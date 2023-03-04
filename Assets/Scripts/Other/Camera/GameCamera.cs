using UnityEngine;

namespace VladB.Doka
{
    public class GameCamera : MonoBehaviour
    {
        [SerializeField] private Transform _camMovingTransform;

        [SerializeField] private Camera _camera;
        [SerializeField] private float _baseCameraSpeed = 10f;
        public Camera Camera => _camera;

        public void Init()
        {
        }

        public void Move(Vector3 vec)
        {
            _camMovingTransform.localPosition += vec * (Time.deltaTime * _baseCameraSpeed);
        }
    }
}