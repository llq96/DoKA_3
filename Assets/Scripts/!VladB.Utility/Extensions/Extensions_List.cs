using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CommentTypo

namespace VladB.Utility
{
    public static partial class Extensions
    {
        #region Act

        /// <summary>
        /// Выполняет указанное действие со всеми элементами списка/массива/любого другого IList
        /// </summary>
        /// <typeparam name="T">Тип элементов содержащихся в IList</typeparam>
        /// <param name="iList">Список, массив или любой другой IList</param>
        /// <param name="action">Делегат, принимающий объект типа T и int(счётчик)</param>
        public static void Act<T>(this IList<T> iList, Action<T, int> action)
        {
            for (int i = 0; i < iList.Count; i++)
            {
                action?.Invoke(iList[i], i);
            }
        }

        /// <summary>
        /// Выполняет указанное действие со всеми элементами списка/массива/любого другого IList, аналогичен дефолтному ForEach(x=>{blablabla})
        /// </summary>
        /// <typeparam name="T">Тип элементов содержащихся в IList</typeparam>
        /// <param name="iList">Список, массив или любой другой IList</param>
        /// <param name="action">Делегат, принимающий объект типа T</param>
        public static void Act<T>(this IList<T> iList, Action<T> action)
        {
            for (int i = 0; i < iList.Count; i++)
            {
                action?.Invoke(iList[i]);
            }
        }

        /// <summary>
        /// Выполняет указанное действие со всеми элементами любого IEnumerable
        /// </summary>
        /// <typeparam name="T">Тип элементов содержащихся в IEnumerable</typeparam>
        /// <param name="iEnumerable">Любой IEnumerable</param>
        /// <param name="action">Делегат, принимающий объект типа T и int(счётчик)</param>
        public static void Act<T>(this IEnumerable<T> iEnumerable, Action<T, int> action)
        {
            int i = 0;
            foreach (var item in iEnumerable)
            {
                action?.Invoke(item, i);
                i++;
            }
        }

        /// <summary>
        /// Выполняет указанное действие со всеми элементами любого IEnumerable
        /// </summary>
        /// <typeparam name="T">Тип элементов содержащихся в IEnumerable</typeparam>
        /// <param name="iEnumerable">Любой IEnumerable</param>
        /// <param name="action">Делегат, принимающий объект типа T</param>
        public static void Act<T>(this IEnumerable<T> iEnumerable, Action<T> action)
        {
            foreach (var item in iEnumerable)
            {
                action?.Invoke(item);
            }
        }

        #endregion

        #region Random

        public static List<T> GetSortedByRandom<T>(this IEnumerable<T> iEnumerable)
        {
            return iEnumerable.OrderBy((item) => UnityEngine.Random.Range(0f, 1f)).ToList();
        }

        public static T GetRandom<T>(this IList<T> iList)
        {
            return (iList.Count > 0) ? iList[UnityEngine.Random.Range(0, iList.Count)] : default;
        }

        #endregion
    }
}