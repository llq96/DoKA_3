using UnityEngine;

namespace VladB.Doka.FogOfWar
{
    public class FOW_LightPoint : MonoBehaviour
    {
        [SerializeField] private float _radius = 1;
        public float Radius => _radius;

        public void Init()
        {
        }
    }
}