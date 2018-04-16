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
            AdjGraphNode<string> node1 = new AdjGraphNode<string>("VP and Ellesemere");
            AdjGraphNode<string> node2 = new AdjGraphNode<string>("Markham and Ellesemere");
            AdjGraphNode<string> node3 = new AdjGraphNode<string>("Markam and Eglinton");
            AdjGraphNode<string> node4 = new AdjGraphNode<string>("VP and Eglington");
            AdjGraphNode<string> node5 = new AdjGraphNode<string>("VP and Kingston Rd");
            AdjGraphNode<string> node6 = new AdjGraphNode<string>("Kingston Rd and Eglinton");

            node1.neighbours.Add(node4);
            node1.neighbours.Add(node2);
            node2.neighbours.Add(node3);
            node4.neighbours.Add(node3);
            node4.neighbours.Add(node5);
            node5.neighbours.Add(node6);
            node6.neighbours.Add(node3);

            //node1.DepthFirstTraversal();
            node1.BreadthFirstTraversal();


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
