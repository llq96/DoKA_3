using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VladB.EffectsSystem;
using VladB.Utility;
using VladB.Doka;

namespace VladB.EffectsSystem {
    public class EffectOnReciever : MonoBehaviour {
        [HideInInspector]public EffectData_Base data;

        public virtual void Init() {
            data.Init();
        }

        public virtual void DoUpdate() {

        }

        #region Debug
        [ContextMenu ("DebugLogData")]
        public void DebugLogData() {
            Debug.Log(GetDebugLogData());
        }

        public virtual string GetDebugLogData() {
            return data?.GetDebugLog();
        }
        #endregion
    }
}