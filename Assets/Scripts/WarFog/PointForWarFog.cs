using UnityEngine;

namespace VladB.Doka
{
    public class PointForWarFog : MonoBehaviour
    {
        [SerializeField]
        private  float _radius = 1;
        public float Radius => _radius;

        public void Init()
        {
        }
    }
}