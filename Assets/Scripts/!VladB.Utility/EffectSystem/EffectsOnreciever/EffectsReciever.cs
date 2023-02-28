using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VladB.Utility;

namespace VladB.EffectsSystem {
    public class EffectsReciever : MonoBehaviour {
        [Header ("Effects list")]
        public List<EffectOnReciever> effects = new List<EffectOnReciever>();

        public Action OnAddAnyEffect;
        public Action<EffectOnReciever> OnAddThisEffect;
        public Action<Type> OnAddAnyEffectWithType;

        public Action OnRemoveAnyEffect;
        public Action<EffectOnReciever> OnRemoveThisEffect;
        public Action<Type> OnRemoveAnyEffectWithType;

        #region AddEffect
        public void AddEffect<T1,T2>(T2 effectDataSource) where T1 : EffectOnReciever where T2 : EffectData_Base {
            if(effectDataSource.isFullUnique) {
                if(effects.Any(ef => ef is T1)) {
                    Debug.Log("Effects Already Exist On Reciever");
                    return;
                }
            }

            if(effectDataSource.isUniqueBySource) {
                if(effects.Any(ef => ef is T1 && ef.data.source == effectDataSource.source)) {
                    Debug.Log("Effects From This Source Already Exist On Reciever");
                    return;
                }
            }

            if(effectDataSource.isUniqueByGUID) {
                if(effects.Any(ef => ef is T1 && ef.data.guid == effectDataSource.guid)) {
                    Debug.Log("Effects WithThisGUID Already Exist On Reciever");
                    return;
                }
            }

            var newEffect = gameObject.AddComponent<T1>() as EffectOnReciever;
            effectDataSource.CopyDataTo(newEffect);
            InitEffect(newEffect);
            effects.Add(newEffect);

            OnAddAnyEffect?.Invoke();
            OnAddThisEffect?.Invoke(newEffect);
            OnAddAnyEffectWithType?.Invoke(newEffect.GetType());
        }
        #endregion

        public virtual void InitEffect(EffectOnReciever effect) {
            effect.Init();
        }

        #region RemoveEffects
        public void RemoveEffects_ByDataGUID(EffectData_Base data) {
            RemoveEffects_ByGUID(data.guid);
        }

        public void RemoveEffects_ByGUID(string guid) {
            effects.Where(ef => ef.data.guid == guid).ToList().Act(ef => RemoveEffect(ef));
        }

        public void RemoveEffects_BySource(IEffectSource source) {
            effects.Where(ef => ef.data.source == source).ToList().Act(ef => RemoveEffect(ef));
        }

        public void RemoveEffect(EffectOnReciever effect) {
            if(effect) {
                if(effects.Contains(effect)) {
                    effects.Remove(effect);

                    OnRemoveAnyEffect?.Invoke();
                    OnRemoveThisEffect?.Invoke(effect);
                    OnRemoveAnyEffectWithType?.Invoke(effect.GetType());

                    Destroy(effect);
                } else {
                    Debug.LogError("Not Exist Effect");
                }
            } else {
                Debug.LogError("effect == null");
            }
        }
        #endregion


        #region IsContains...
        public bool IsContainsEffect_WithGUID(string guid) {
            return effects.Any(ef => ef.data.guid == guid);
        }
        //TODO...

        public bool IsContainsEffect_WithType<T>() where T : EffectOnReciever {
            return effects.Any(e => e is T);
        }
        #endregion

        public List<T> GetEffectsWithType<T>() where T : EffectOnReciever {
            return effects.Where(e => e is T).OfType<T>().OrderBy(e => e.data.priority).ToList();
        }



        void Update() {
            effects.Where(ef => !ef.data.isInfiniteDuration).ToList().Act(ef => {
                ef.data.currentDuration -= Time.deltaTime;
                if(ef.data.currentDuration <= 0f) {
                    RemoveEffect(ef);
                }
            });

            effects.Where(ef => ef.data.isNeedUpdate).Act(ef => ef.DoUpdate());
        }

    }
}