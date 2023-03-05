using UnityEngine;

namespace VladB.Doka
{
    public class UnitVisibility : MonoBehaviour
    {
        private Unit _unit;
        private bool _isVisible;

        public void Init(Unit unit)
        {
            _unit = unit;
            SetVisibility(true);
        }

        public void SetVisibility(bool isVisible)
        {
            //TODO 
            if (isVisible == _isVisible) return;
            _isVisible = isVisible;
            
            var layer = isVisible ? LayerMask.NameToLayer("Units") : LayerMask.NameToLayer("UnVisible"); //TODO 
            SetLayerRecursively(_unit.gameObject, layer);
        }

        public static void SetLayerRecursively(GameObject go, int layer)
        {
            foreach (Transform tr in go.GetComponentsInChildren<Transform>(true))
            {
                tr.gameObject.layer = layer;
            }
        }
    }
}