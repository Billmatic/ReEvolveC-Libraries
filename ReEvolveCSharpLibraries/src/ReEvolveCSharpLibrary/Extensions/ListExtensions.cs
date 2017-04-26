using System;
using System.Collections.Generic;

namespace ReEvolveCSharpLibrary.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Quick sort the string list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> QuickSort(this List<string> list)
        {
            Random r = new Random();
            List<string> less = new List<string>();
            List<string> greater = new List<string>();

            if (list.Count <= 1)
            {
                return list;
            }

            int pos = r.Next(list.Count);
            string pivot = list[pos];
            list.RemoveAt(pos);

            foreach (string x in list)
            {
                if (String.Compare( x, pivot) < 0)
                {
                    less.Add(x);
                }
                else
                {
                    greater.Add(x);
                }
            }
            return Concat(QuickSort(less), pivot, QuickSort(greater));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="less"></param>
        /// <param name="pivot"></param>
        /// <param name="greater"></param>
        /// <returns></returns>
        private static List<string> Concat(List<string> less, string pivot, List<string> greater)
        {
            List<string> sorted = new List<string>(less);
            sorted.Add(pivot);

            foreach (string i in greater)
            {
                sorted.Add(i);
            }
            return sorted;
        }

        /// <summary>
        /// Remove duplicate strings in the list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> RemoveDuplicateItems(this List<string> list)
        {
            List<string> distinctList = list.QuickSort();
            string previousItem = "";
            int listIndex = distinctList.Count-1;

            while(listIndex >= 0)
            {
                if (distinctList[listIndex] == previousItem)
                {
                    distinctList.Remove(distinctList[listIndex]);
                }
                else
                {
                    previousItem = distinctList[listIndex];
                }
                listIndex--;
            }
            return distinctList;
        }
    }
}
