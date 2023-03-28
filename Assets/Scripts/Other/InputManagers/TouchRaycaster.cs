using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VladB.Doka
{
    public class TouchRaycaster : MonoBehaviour
    {
        [SerializeField] private Camera _raycastCamera;

        [SerializeField] private LayerMask _raycastMask;
        [SerializeField] private LayerMask _movementMask;
        [SerializeField] private LayerMask _unitMask;

        public Action<RaycastHit> OnHitInMovementMask;
        public Action<RaycastHit, Unit> OnHitInUnit;

        public void Init()
        {
        }

        private static bool IsRaycastInUI()
        {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition },
                results);
            return results.Count > 0;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;
            if (IsRaycastInUI()) return;

            var mousePos = Input.mousePosition;
            var ray = _raycastCamera.ScreenPointToRay(mousePos);
            var hits = Physics.RaycastAll(ray, float.MaxValue, _raycastMask, QueryTriggerInteraction.Collide);
            hits = hits.OrderBy(x => x.distance).ToArray();

            if (Input.GetMouseButtonDown(0))
            {
                foreach (var hit in hits)
                {
                    var layer = hit.collider.gameObject.layer;

                    if (IsInLayerMask(layer, _unitMask))
                    {
                        var unit = hit.collider.GetComponentInParent<Unit>();
                        if (unit == null) Debug.LogError("Unit == null");
                        OnHitInUnit?.Invoke(hit, unit);
                        break;
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                foreach (var hit in hits)
                {
                    var layer = hit.collider.gameObject.layer;

                    if (IsInLayerMask(layer, _movementMask))
                    {
                        OnHitInMovementMask?.Invoke(hit);
                        break;
                    }

                    //TODO Attack
                    // if (IsInLayerMask(layer, _unitMask))
                    // {
                    //     var unit = hit.collider.GetComponentInParent<Unit>();
                    //     if (unit == null) Debug.LogError("Unit == null");
                    //     OnHitInUnit?.Invoke(hit, unit);
                    //     break;
                    // }
                }
            }
        }

        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
    }
}