using UnityEngine;
using VladB.Utility;

namespace VladB.EffectsSystem
{
    [System.Serializable]
    public class EffectData_Base
    {
        [Header("Priority")] [Range(0, 255)] public int priority;

        [Header("Unique?")]
        //Совсем уникальные, не может висеть двух эффектов этого класса не при каких обстоятельствах
        public bool isFullUnique;

        // Может висеть несколько однотипных эффектов, но только 1 от этого источника
        public bool isUniqueBySource;

        // Может висеть несколько однотипных эффектов, но именно этот только 1
        public bool isUniqueByGUID = true;
        public string guid = "";

        [Header("Time Settings")] public bool isInfiniteDuration = true;
        public float baseDuration;

        [Header("Other")] public bool isNeedUpdate;


        [Header("Changable:")] public float currentDuration;
        public IEffectSource source;

        public EffectData_Base()
        {
            if (guid.IsNullOrEmpty())
            {
                guid = Extensions.GetRandomGUID();
            }
        }

        public virtual void Init()
        {
            if (!isInfiniteDuration)
            {
                currentDuration = baseDuration;
            }
        }

        public virtual void CopyDataTo(EffectOnReceiver newEffect)
        {
            newEffect.SetData(MemberwiseClone());
        }

        public virtual string GetDebugLog()
        {
            string log = "";
            log += $"source = {source} \n";
            log += $"priority = {priority} \n";
            log += $"isFullUnique = {isFullUnique} \n";
            log += $"isUniqueBySource = {isUniqueBySource} \n";
            log += $"isUniqueByGUID = {isUniqueByGUID} \n";
            log += $"guid = {guid} \n";
            return log;
        }
    }


    public interface IEffectSource
    {
    }
}