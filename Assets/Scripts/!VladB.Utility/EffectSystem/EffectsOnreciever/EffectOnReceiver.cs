using System;
using UnityEngine;

namespace VladB.EffectsSystem
{
    [Serializable]
    public class EffectOnReceiver<TData> : EffectOnReceiver where TData : EffectData_Base
    {
        [SerializeField] private TData _data;
        public TData Data => _data;
        public override EffectData_Base BaseData => _data;

        public override void SetData(object obj)
        {
            _data = obj as TData;
        }

        public void SetData(TData data)
        {
            _data = data;
        }
    }

    [Serializable]
    public abstract class EffectOnReceiver : MonoBehaviour
    {
        public abstract EffectData_Base BaseData { get; }

        public abstract void SetData(object data);

        public virtual void Init()
        {
            BaseData.Init();
        }

        public virtual void DoUpdate()
        {
        }

        #region Debug

        [ContextMenu("DebugLogData")]
        public void DebugLogData()
        {
            Debug.Log(GetDebugLogData());
        }

        public virtual string GetDebugLogData()
        {
            return BaseData?.GetDebugLog();
        }

        #endregion
    }
}