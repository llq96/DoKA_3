using System.Collections.Generic;
using UnityEngine;

namespace VladB.Doka.FogOfWar
{
    public class FOW_BlockerPoint : MonoBehaviour
    {
        [SerializeField] private float _radius = 1;
        public float Radius => _radius;

        public List<FOW_VisibilityChangingObject> VisibilityChangingObjects;
            
        public void Init()
        {
        }
    }
}