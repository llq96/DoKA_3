using UniRx;
using UnityEngine;


namespace VladB.Utility
{
    public abstract class VariableUI<T> : MonoBehaviour
    {
        [SerializeField] protected string Prefix;
        [SerializeField] protected string Postfix;

        protected IReadOnlyReactiveProperty<T> ReactiveProperty;

        private CompositeDisposable _disposables = new();

        public virtual void SetReactiveProperty(IReadOnlyReactiveProperty<T> reactiveProperty)
        {
            _disposables.Dispose();
            ReactiveProperty = reactiveProperty;
            ReactiveProperty.Subscribe(_ => UpdateVariableUI()).AddTo(_disposables);
            UpdateVariableUI();
        }

        public abstract void UpdateVariableUI();

        #region OnEnable/OnDisable/Update Functions

        private void OnEnable()
        {
            OnEnableMethod();
        }

        protected virtual void OnEnableMethod()
        {
            ReactiveProperty.Subscribe(_ => UpdateVariableUI()).AddTo(_disposables);
            UpdateVariableUI();
        }


        private void OnDisable()
        {
            OnDisableMethod();
        }

        protected virtual void OnDisableMethod()
        {
            _disposables.Dispose();
        }

        private void Update()
        {
            UpdateMethod();
        }

        protected virtual void UpdateMethod()
        {
        }

        #endregion
    }
}