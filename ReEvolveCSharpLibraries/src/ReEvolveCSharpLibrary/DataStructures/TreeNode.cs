using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReEvolveCSharpLibrary.DataStructures
{
    public class TreeNode<K>
    {
        public K data { get; set; }

        public TreeNode<K> parentNode { get; set; }

        public TreeNode<K> leftNode { get; set; }

        public TreeNode<K> rightNode { get; set; }

        public TreeNode(K newData)
        {
            data = newData;
        }

        public TreeNode(K newData, TreeNode<K> newLeftNode, TreeNode<K> newRightNode)
        {
            data = newData;
            leftNode = newLeftNode;
            rightNode = newRightNode;
        }

        public int GetHeight()
        {
            var result = 0;

            if (this != null)
            {
                result = Math.Max(GetHeight(this.leftNode), GetHeight(this.rightNode)) + 1;
            }

            return result;
        }

        private int GetHeight(TreeNode<K> node)
        {
            var result = 0;

            if (node != null)
            {
                result = Math.Max(GetHeight(node.leftNode), GetHeight(node.rightNode)) + 1;
            }

            return result;
        }

        public int GetBalance()
        {
            int leftHeight = this.leftNode != null ? this.leftNode.GetHeight() : 0;
            int rightHeight = this.rightNode != null ? this.rightNode.GetHeight() : 0;

            return leftHeight - rightHeight;
        }

        public bool IsLeaf()
        {
            return (leftNode == null && rightNode == null);
        }

        public static bool operator ==(TreeNode<K> node1, TreeNode<K> node2)
        {
            if (object.ReferenceEquals(node2, null))
            {
                return object.ReferenceEquals(node1, null);
            }

            return node1.Equals(node2);
        }

        public static bool operator !=(TreeNode<K> node1, TreeNode<K> node2)
        {
            if (object.ReferenceEquals(node2, null))
            {
                return !object.ReferenceEquals(node1, null);
            }

            return !node1.Equals(node2);
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            TreeNode<K> node2 = (TreeNode<K>)obj;
            return (Comparer<K>.Default.Compare(this.data, node2.data) == 0);
        }
        public override int GetHashCode()
        {
            return this.data.GetHashCode();
        }

        public static bool operator <(TreeNode<K> node1, TreeNode<K> node2)
        {

            return (Comparer<K>.Default.Compare(node1.data, node2.data) < 0);

        }

        public static bool operator >(TreeNode<K> node1, TreeNode<K> node2)
        {

            return (Comparer<K>.Default.Compare(node1.data, node2.data) > 0);

        }

        public static bool operator <=(TreeNode<K> node1, TreeNode<K> node2)
        {

            return (Comparer<K>.Default.Compare(node1.data, node2.data) <= 0);

        }

        public static bool operator >=(TreeNode<K> node1, TreeNode<K> node2)
        {

            return (Comparer<K>.Default.Compare(node1.data, node2.data) >= 0);

        }

       

    }
}
