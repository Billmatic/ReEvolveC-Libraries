using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using ReEvolveCSharpLibrary.Extensions;
using ReEvolveCRMLibrary.Extensions;
using ReEvolveCRMLibrary.Helpers;
using Microsoft.Xrm.Sdk.Query;
using ReEvolveCSharpLibrary.DataStructures;

namespace LibraryTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //342, 206, 444, 523, 607, 301, 142, 183, 102, 157, 149
            BinaryTree<int> tree = new BinaryTree<int>(342);

            tree.Insert(206);
            tree.Insert(444);
            tree.Insert(523);
            tree.Insert(607);
            tree.Insert(301);
            tree.Insert(142);
            tree.Insert(183);
            tree.Insert(102);
            tree.Insert(157);
            tree.Insert(149);


            tree.BreathFirstTraversal();

        }
    }
}
