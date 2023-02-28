using System.Collections.Generic;
using UnityEngine;
using VladB.Utility;

namespace VladB.EffectsSystem
{
    public class Effect_ChangeValue_Bool : EffectOnReceiver<EffectData_ChangeValue_Bool>
    {
        public virtual bool ApplyEffectToValue(bool prevStepValue)
        {
            return Data.boolValue;
        }

        public override string GetDebugLogData()
        {
            return Data?.GetDebugLog();
        }
    }


    [System.Serializable]
    public class EffectData_ChangeValue_Bool : EffectData_Base
    {
        [Header("Change Value Data")] public bool boolValue;

        public override string GetDebugLog()
        {
            string log = base.GetDebugLog();
            log += $"BoolValue = {boolValue} \n";
            return log;
        }
    }


    #region Extensions

    public static partial class EffectExtensions
    {
        public static bool Calc(this IList<Effect_ChangeValue_Bool> iList)
        {
            bool result = false;
            iList.Act(ef => result = ef.ApplyEffectToValue(result));
            return result;
        }
    }

    #endregion
}