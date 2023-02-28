using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace VladB.Doka {
    public class EffectsOnSourceList : MonoBehaviour {
        public List<EffectOnSource> effects = new List<EffectOnSource>();

        public virtual void Init() {
            effects = GetComponents<EffectOnSource>().ToList();
        }
    }
}