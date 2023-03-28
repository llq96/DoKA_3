using UnityEngine;
using VladB.Doka.UI;

namespace VladB.Doka
{
    public class UIController : VladB.Utility.UIController
    {
        [SerializeField] private UI_DownPanel _downPanel;

        public override void Init()
        {
            base.Init();

            _downPanel.Init();
        }
    }
}