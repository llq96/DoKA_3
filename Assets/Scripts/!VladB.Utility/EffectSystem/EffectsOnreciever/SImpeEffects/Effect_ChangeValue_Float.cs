using System.Collections.Generic;
using UnityEngine;
using VladB.Utility;
using static VladB.EffectsSystem.Effect_ChangeValue_Float;

namespace VladB.EffectsSystem
{
    public class Effect_ChangeValue_Float : EffectOnReceiver<EffectData_ChangeValue_Float>
    {
        public virtual float ApplyEffectToValue(float prevStepValue)
        {
            return Data.changeValueEffectType switch
            {
                EffectData_ChangeValue_Float_Type.Number => prevStepValue + Data.changeValue,
                EffectData_ChangeValue_Float_Type.Percentage => prevStepValue * Data.changeValue,
                _ => prevStepValue,
            };
        }

        public enum EffectData_ChangeValue_Float_Type
        {
            Number,
            Percentage
        }

        public override string GetDebugLogData()
        {
            return Data?.GetDebugLog();
        }
    }


    [System.Serializable]
    public class EffectData_ChangeValue_Float : EffectData_Base
    {
        [Header("Change Value Data")] public EffectData_ChangeValue_Float_Type changeValueEffectType;
        public float changeValue;

        public override string GetDebugLog()
        {
            string log = base.GetDebugLog();
            log += $"changeValueEffectType = {changeValueEffectType} \n";
            log += $"changeValue = {changeValue} \n";
            return log;
        }
    }


    #region Extensions

    public static partial class EffectExtensions
    {
        public static float Calc(this IList<Effect_ChangeValue_Float> iList)
        {
            float result = 0;
            iList.Act(ef => result = ef.ApplyEffectToValue(result));
            return result;
        }
    }

    #endregion
}