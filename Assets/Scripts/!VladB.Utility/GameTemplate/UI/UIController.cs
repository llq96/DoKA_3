using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VladB.Utility;

namespace VladB.Utility
{
    public class UIController : MonoBehaviour
    {
        public List<IUIWindow> WindowsList;

        #region IController Realization

        public virtual void Init()
        {
            WindowsList = GetComponentsInChildren<IUIWindow>(true).ToList();
            WindowsList.Act(w => w.Init());
            WindowsList.Act(w => w.Close());
        }

        #endregion

        #region Open/Close Window

        public virtual void OpenWindow<T>(bool isCloseOther = true) where T : IUIWindow
        {
            WindowsList = WindowsList.Where(w => w != null).ToList();

            if (isCloseOther)
            {
                WindowsList.Where(w => w.IsOpened && !(w is T)).Act(w => w.Close());
            }

            WindowsList.Where(w => (w is T)).Act(w => w.Open());
        }

        public virtual void CloseWindow<T>() where T : IUIWindow
        {
            WindowsList.Where(w => w is T).Act(w => w.Close());
        }

        public virtual void OpenWindow(IUIWindow window, bool isCloseOther = true)
        {
            WindowsList = WindowsList.Where(w => w != null).ToList();

            if (isCloseOther)
            {
                WindowsList.Where(w => w.IsOpened && w != window).Act(w => w.Close());
            }

            window?.Open();
        }

        public virtual void CloseWindow(IUIWindow window)
        {
            window?.Close();
        }

        #endregion
    }


    public interface IUIWindow
    {
        bool IsOpened { get; set; }
        void Open();
        void Close();
        void Init();
    }
}