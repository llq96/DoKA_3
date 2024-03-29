using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace VladB.Doka
{
    public class UnitUI : MonoBehaviour
    {
        private Unit _unit;

        [Header("UI")] [SerializeField] private GameObject _hpUI;
        [SerializeField] private Image _hpBar;
        [SerializeField] private TextMeshProUGUI _tmp_hp;

        private CompositeDisposable _disposables = new();

        public void Init(Unit unit)
        {
            _unit = unit;

            _unit.Stats.Hp.Subscribe(_ => UpdateUI()).AddTo(_disposables);

            UpdateUI();
        }

        private void Update()
        {
            // transform.LookAt(MainController.Instance.GameCamera.transform.position);
            transform.rotation = Quaternion.identity;
        }

        public void UpdateUI()
        {
            _hpBar.fillAmount = _unit.Stats.Hp_Clamped01;
            _tmp_hp.text = $"{_unit.Stats.Hp_Current} / {_unit.Stats.Hp_Max}";
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}