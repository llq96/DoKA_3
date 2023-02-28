using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VladB.Utility;
using static VladB.EffectsSystem.Effect_ChangeValue_Float;

namespace VladB.EffectsSystem {
    public class Effect_ChangeValue_Float : EffectOnReciever {
        public EffectData_ChangeValue_Float thisData => data as EffectData_ChangeValue_Float;
        public virtual float ApplyEffectToValue(float prevStepValue) {
            return thisData.changeValueEffectType switch {
                EffectData_ChangeValue_Float_Type.Number => prevStepValue + thisData.changeValue,
                EffectData_ChangeValue_Float_Type.Percentage => prevStepValue * thisData.changeValue,
                _ => prevStepValue,
            };
        }

        public enum EffectData_ChangeValue_Float_Type {
            Number, Percentage
        }

        public override string GetDebugLogData() {
            return thisData?.GetDebugLog();
        }
    }


    [System.Serializable]
    public class EffectData_ChangeValue_Float : EffectData_Base {
        [Header("Change Value Data")]
        public EffectData_ChangeValue_Float_Type changeValueEffectType;
        public float changeValue;

        //public override void CopyDataTo(EffectData_Base newData) {
        //    base.CopyDataTo(newData);
        //    if(newData is EffectData_ChangeValue_Float data) {
        //        data.changeValueEffectType = changeValueEffectType;
        //        data.changeValue = changeValue;
        //    }
        //}

        public override string GetDebugLog() {
            string log = base.GetDebugLog();
            log += $"changeValueEffectType = {changeValueEffectType} \n";
            log += $"changeValue = {changeValue} \n";
            return log;
        }
    }




    #region Extensions
    public static partial class EffectExtensions {
        public static float Calc(this IList<Effect_ChangeValue_Float> iList) {
            float result = 0;
            iList.Act(ef => result = ef.ApplyEffectToValue(result));
            return result;
        }
    }
    #endregion
}
