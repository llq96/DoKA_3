using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VladB.Utility;

namespace VladB.EffectsSystem
{
    public class EffectsReceiver : MonoBehaviour
    {
        [Header("Effects list")] public List<EffectOnReceiver> _effects = new();

        public Action OnAddAnyEffect;
        public Action<EffectOnReceiver> OnAddThisEffect;
        public Action<Type> OnAddAnyEffectWithType;

        public Action OnRemoveAnyEffect;
        public Action<EffectOnReceiver> OnRemoveThisEffect;
        public Action<Type> OnRemoveAnyEffectWithType;

        #region AddEffect

        public void AddEffect<TEffect, TData>(TData effectDataSource)
            where TEffect : EffectOnReceiver<TData>, new()
            where TData : EffectData_Base
        {
            if (effectDataSource.isFullUnique)
            {
                if (_effects.Any(ef => ef is TEffect))
                {
                    Debug.Log("Effects Already Exist On Receiver");
                    return;
                }
            }

            if (effectDataSource.isUniqueBySource)
            {
                if (_effects.Any(ef => ef is TEffect && ef.BaseData.source == effectDataSource.source))
                {
                    Debug.Log("Effects From This Source Already Exist On Receiver");
                    return;
                }
            }

            if (effectDataSource.isUniqueByGUID)
            {
                if (_effects.Any(ef => ef is TEffect && ef.BaseData.guid == effectDataSource.guid))
                {
                    Debug.Log("Effects WithThisGUID Already Exist On Receiver");
                    return;
                }
            }

            var newEffect = gameObject.AddComponent<TEffect>();
            // var newEffect = new TEffect();

            effectDataSource.CopyDataTo(newEffect);
            InitEffect(newEffect);
            _effects.Add(newEffect);

            OnAddAnyEffect?.Invoke();
            OnAddThisEffect?.Invoke(newEffect);
            OnAddAnyEffectWithType?.Invoke(newEffect.GetType());
        }

        #endregion

        protected virtual void InitEffect(EffectOnReceiver effect)
        {
            effect.Init();
        }

        #region RemoveEffects

        public void RemoveEffects_ByDataGUID(EffectData_Base data)
        {
            RemoveEffects_ByGUID(data.guid);
        }

        public void RemoveEffects_ByGUID(string guid)
        {
            _effects.Where(ef => ef.BaseData.guid == guid).ToList().Act(RemoveEffect);
        }

        public void RemoveEffects_BySource(IEffectSource source)
        {
            _effects.Where(ef => ef.BaseData.source == source).ToList().Act(RemoveEffect);
        }

        public void RemoveEffect(EffectOnReceiver effect)
        {
            if (effect != null)
            {
                if (_effects.Contains(effect))
                {
                    _effects.Remove(effect);

                    OnRemoveAnyEffect?.Invoke();
                    OnRemoveThisEffect?.Invoke(effect);
                    OnRemoveAnyEffectWithType?.Invoke(effect.GetType());

                    Destroy(effect);
                }
                else
                {
                    Debug.LogError("Not Exist Effect");
                }
            }
            else
            {
                Debug.LogError("effect == null");
            }
        }

        #endregion


        #region IsContains...

        public bool IsContainsEffect_WithGUID(string guid)
        {
            return _effects.Any(ef => ef.BaseData.guid == guid);
        }
        //TODO...

        public bool IsContainsEffect_WithType<T>() where T : EffectOnReceiver
        {
            return _effects.Any(e => e is T);
        }

        #endregion

        public List<T> GetEffectsWithType<T>() where T : EffectOnReceiver
        {
            return _effects.Where(e => e is T).OfType<T>().OrderBy(e => e.BaseData.priority).ToList();
        }


        private void Update()
        {
            _effects.Where(ef => !ef.BaseData.isInfiniteDuration).ToList().Act(ef =>
            {
                ef.BaseData.currentDuration -= Time.deltaTime;
                if (ef.BaseData.currentDuration <= 0f)
                {
                    RemoveEffect(ef);
                }
            });

            _effects.Where(ef => ef.BaseData.isNeedUpdate).Act(ef => ef.DoUpdate());
        }
    }
}