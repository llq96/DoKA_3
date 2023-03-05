using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VladB.Doka.FogOfWar
{
    public class FOW_VisibilityChangingObject : MonoBehaviour
    {
        private bool _isVisible;

        [SerializeField] private Material _mat_visible;
        [SerializeField] private Material _mat_notVisible;

        private Renderer _renderer;

        public void Init()
        {
            SetVisibility(true);
        }

        public void SetVisibility(bool isVisible)
        {
            if (_renderer == null) _renderer = GetComponent<Renderer>();
            if (isVisible == _isVisible) return;
            _isVisible = isVisible;

            _renderer.material = isVisible ? _mat_visible : _mat_notVisible;
        }
    }
}