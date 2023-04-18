using UniRx;
using UnityEngine;


namespace VladB.Utility
{
    public abstract class StringReactivePropertyUI : MonoBehaviour
    {
        [SerializeField] protected string Prefix;
        [SerializeField] protected string Postfix;

        protected IReadOnlyReactiveProperty<string> ReactiveProperty;

        private CompositeDisposable _disposables = new();
        private CompositeDisposable _adapterDisposables = new();

        public virtual void SetReactiveProperty(IReadOnlyReactiveProperty<string> reactiveProperty)
        {
            _disposables.Dispose();
            _disposables = new CompositeDisposable();

            ReactiveProperty = reactiveProperty;
            ReactiveProperty
                .Subscribe(_ => UpdateVariableUI())
                .AddTo(_disposables);
            UpdateVariableUI();
        }

        //Adapter
        public void SetReactiveProperty<T>(IReadOnlyReactiveProperty<T> reactiveProperty)
        {
            _adapterDisposables.Dispose();
            _adapterDisposables = new CompositeDisposable();

            var stringReactiveProperty = new StringReactiveProperty(reactiveProperty.Value.ToString());
            reactiveProperty
                .Subscribe(value => { stringReactiveProperty.Value = value.ToString(); })
                .AddTo(_adapterDisposables);
            SetReactiveProperty(stringReactiveProperty);
        }

        public abstract void UpdateVariableUI();

        private void OnEnable()
        {
            UpdateVariableUI();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            _adapterDisposables.Dispose();
        }
    }
}