using UnityEngine;
using VladB.Doka.FogOfWar;

namespace VladB.Doka
{
    public class MainController : MonoBehaviour
    {
        public static MainController Instance { get; private set; }

        [SerializeField] private UnitsManager _unitsManager;
        public UnitsManager UnitsManager => _unitsManager;

        [SerializeField] private InputManager_PC _inputManager_pc;
        public InputManager_PC InputManager_PC => _inputManager_pc;

        [SerializeField] private TouchRaycaster _touchRaycaster;
        public TouchRaycaster TouchRaycaster => _touchRaycaster;

        [SerializeField] private UIController _uiController;
        public UIController UIController => _uiController;

        [SerializeField] private GameCamera _gameCamera;
        public GameCamera GameCamera => _gameCamera;

        [SerializeField] private FOW_Controller _fowController;
        public FOW_Controller FOWController => _fowController;

        [SerializeField] private ParticlesManager _particlesManager;
        public ParticlesManager ParticlesManager => _particlesManager;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Init();
            StartGame();
        }

        private void Init()
        {
            UnitsManager.Init();
            InputManager_PC.Init();
            TouchRaycaster.Init();
            FOWController.Init();
            ParticlesManager.Init();

            GameCamera.Init();

            UIController.Init();
        }

        private void StartGame()
        {
            UnitsManager.UnitsSelector.SelectUnit(UnitsManager.Player);
        }
    }
}