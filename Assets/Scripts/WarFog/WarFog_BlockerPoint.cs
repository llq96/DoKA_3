using UnityEngine;

namespace VladB.Doka.WarFog
{
    public class WarFog_BlockerPoint : MonoBehaviour
    {
        [SerializeField] private float _radius = 1;
        public float Radius => _radius;

        public void Init()
        {
        }
    }
}