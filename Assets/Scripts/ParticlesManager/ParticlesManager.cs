using System.Linq;
using UnityEngine;
using Zenject;

namespace VladB.Doka
{
    //TODO Переделать вспомогательный класс Particle из предыдущих проектов
    public class ParticlesManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle_movePoints;
        [Inject] private TouchRaycaster _touchRaycaster;

        public void Init()
        {
            _touchRaycaster.OnHitInMovementMask += hit => { PlayParticle_MovePoint(hit.point); };
        }

        public void PlayParticle_MovePoint(Vector3 position)
        {
            _particle_movePoints.transform.position = position;
            var particles = _particle_movePoints.GetComponentsInChildren<ParticleSystem>(true).ToList();
            particles.ForEach(x => x.Stop());
            particles.ForEach(x => x.Clear());
            particles.ForEach(x => x.Play());
        }
    }
}