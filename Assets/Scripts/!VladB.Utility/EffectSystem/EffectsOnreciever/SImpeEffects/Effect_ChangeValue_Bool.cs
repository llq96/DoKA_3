using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VladB.EffectsSystem;
using VladB.Utility;

namespace VladB.EffectsSystem {
    public class Effect_ChangeValue_Bool : EffectOnReciever {
        public EffectData_ChangeValue_Bool thisData => data as EffectData_ChangeValue_Bool;
        public virtual bool ApplyEffectToValue(bool prevStepValue) {
            return thisData.boolValue;
        }

        public override string GetDebugLogData() {
            return thisData?.GetDebugLog();
        }
    }




    [System.Serializable]
    public class EffectData_ChangeValue_Bool : EffectData_Base {
        [Header("Change Value Data")]
        public bool boolValue;

        //public override void CopyDataTo(EffectData_Base newData) {
        //    base.CopyDataTo(newData);
        //    if(newData is EffectData_ChangeValue_Bool data) {
        //        data.boolValue = boolValue;
        //    }
        //}

        public override string GetDebugLog() {
            string log = base.GetDebugLog();
            log += $"BoolValue = {boolValue} \n";
            return log;
        }
    }




    #region Extensions
    public static partial class EffectExtensions {
        public static bool Calc(this IList<Effect_ChangeValue_Bool> iList) {
            bool result = false;
            iList.Act(ef => result = ef.ApplyEffectToValue(result));
            return result;
        }
    }
    #endregion
}