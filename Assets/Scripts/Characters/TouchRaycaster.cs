using System;
using UnityEngine;

namespace VladB.Doka
{
    public class TouchRaycaster : MonoBehaviour
    {
        [SerializeField] private Camera _raycastCamera;

        [SerializeField] private LayerMask _raycastMask;
        [SerializeField] private LayerMask _movementMask;
        [SerializeField] private LayerMask _unitMask;

        public Action<RaycastHit> OnHitInMovementMask;
        public Action<RaycastHit> OnHitInUnit;

        public void Init()
        {
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var mousePos = Input.mousePosition;
                var ray = _raycastCamera.ScreenPointToRay(mousePos);
                // Test(ray);
                var hits = Physics.RaycastAll(ray, 1000f, _raycastMask);

                foreach (var hit in hits)
                {
                    var layer = hit.collider.gameObject.layer;

                    if (IsInLayerMask(layer, _movementMask))
                    {
                        OnHitInMovementMask?.Invoke(hit);
                        break;
                    }

                    if (IsInLayerMask(layer, _unitMask))
                    {
                        OnHitInUnit?.Invoke(hit);
                        break;
                    }
                }
            }
        }

        // private async void Test(Ray ray)
        // {
        //     float timer = 1f;
        //     while (timer > 0f)
        //     {
        //         timer -= Time.deltaTime;
        //         Debug.DrawRay(ray.origin, ray.direction * 1000f);
        //         await Task.Yield();
        //     }
        // }

        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
    }
}