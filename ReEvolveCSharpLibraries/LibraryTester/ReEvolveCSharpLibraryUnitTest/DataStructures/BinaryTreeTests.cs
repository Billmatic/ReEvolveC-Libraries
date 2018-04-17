using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReEvolveCSharpLibrary.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReEvolveCSharpLibraryUnitTest.DataStructures
{
    [TestClass()]
    public class BinaryTreeTests
    {
        [TestMethod()]
        public void BreathFirstTraversalTest()
        {
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

        [TestMethod()]
        public void CommonAncestorTest()
        {
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

            TreeNode<int> node = new TreeNode<int>(444);
            TreeNode<int> node2 = new TreeNode<int>(607);
            tree.BreathFirstTraversal();
            TreeNode<int> ancestor = tree.CommonAncestor(node, node2);

            if (ancestor.data != 523)
            {
                Assert.Fail();
            }
        }
    }
}