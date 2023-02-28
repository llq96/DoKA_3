using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace VladB.Doka
{
    public class EffectsOnSourceList : MonoBehaviour
    {
        private List<EffectOnSource> _effects = new();

        public virtual void Init()
        {
            _effects = GetComponents<EffectOnSource>().ToList();
        }
    }
}