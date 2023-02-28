using UnityEngine;

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

            GameCamera.Init();

            UIController.Init();
        }
    }
}