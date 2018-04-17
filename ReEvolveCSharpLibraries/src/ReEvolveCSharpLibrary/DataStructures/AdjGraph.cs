using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReEvolveCSharpLibrary.DataStructures
{
    public class AdjGraph<T>
    {
        /// <summary>
        /// Finds if there is a path between A and B using DFS
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool PathBetweenABExist(AdjGraphNode<T> a, AdjGraphNode<T> b, Guid visitId)
        {
            if (a.visitId == visitId)
            {
                return false;
            }

            if (a == b)
            {
                return true;
            }

            a.visitId = visitId;
            Console.WriteLine(a.data);

            foreach (AdjGraphNode<T> n in a.neighbours)
            {
                return PathBetweenABExist(n, b, visitId);
            }

            return false;
        }
    }
}
