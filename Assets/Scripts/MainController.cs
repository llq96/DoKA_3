using UnityEngine;
using VladB.Doka.FogOfWar;

namespace VladB.Doka
{
    public class MainController : MonoBehaviour
    {
        public static MainController Instance;

        public UnitsManager UnitsManager;
        public InputManager_PC InputManager_PC;
        public TouchRaycaster TouchRaycaster;
        public UIController UIController;
        public GameCamera GameCamera;
        public FOW_Controller FOWController;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            UnitsManager.Init();
            InputManager_PC.Init();
            TouchRaycaster.Init();
            FOWController.Init();

            GameCamera.Init();

            UIController.Init();
        }
    }
}