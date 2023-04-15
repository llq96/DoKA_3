using System.Linq;
using UnityEngine;

namespace VladB.Doka
{
    //TODO Переделать вспомогательный класс Particle из предыдущих проектов
    public class ParticlesManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle_movePoints;

        public void Init()
        {
            MainController.Instance.TouchRaycaster.OnHitInMovementMask += hit => { PlayParticle_MovePoint(hit.point); };
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