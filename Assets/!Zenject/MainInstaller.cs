using UnityEngine;
using VladB.Doka.FogOfWar;
using Zenject;

namespace VladB.Doka
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private UnitsManager _unitsManager;
        [SerializeField] private InputManager_PC _inputManager_pc;
        [SerializeField] private TouchRaycaster _touchRaycaster;
        [SerializeField] private UIController _uiController;
        [SerializeField] private GameCamera _gameCamera;
        [SerializeField] private FOW_Controller _fowController;
        [SerializeField] private ParticlesManager _particlesManager;


        public override void InstallBindings()
        {
            Container.Bind<UnitsManager>().FromInstance(_unitsManager).AsSingle().NonLazy();
            // Container.Bind<InputManager_PC>().FromInstance(_inputManager_pc).AsSingle().NonLazy();
            Container.Bind<TouchRaycaster>().FromInstance(_touchRaycaster).AsSingle().NonLazy();
            Container.Bind<UIController>().FromInstance(_uiController).AsSingle().NonLazy();
            Container.Bind<GameCamera>().FromInstance(_gameCamera).AsSingle().NonLazy();
            Container.Bind<FOW_Controller>().FromInstance(_fowController).AsSingle().NonLazy();
            Container.Bind<ParticlesManager>().FromInstance(_particlesManager).AsSingle().NonLazy();
        }

        public override void Start()
        {
            Init();
            StartGame();
        }

        private void Init()
        {
            _unitsManager.Init();
            _inputManager_pc.Init();
            _touchRaycaster.Init();
            _fowController.Init();
            _particlesManager.Init();

            _gameCamera.Init();

            _uiController.Init();
        }

        private void StartGame()
        {
            _unitsManager.UnitsSelector.SelectUnit(_unitsManager.Player);
        }
    }
}