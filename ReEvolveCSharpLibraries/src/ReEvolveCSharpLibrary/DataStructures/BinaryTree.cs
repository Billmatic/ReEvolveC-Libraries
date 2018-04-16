using System;
using System.Collections.Generic;

namespace ReEvolveCSharpLibrary.DataStructures
{
    public class BinaryTree<T>
    {
        public TreeNode<T> root {get;set;}

        public BinaryTree(T newData)
        {
            TreeNode<T> newNode = new TreeNode<T>(newData);
            this.root = newNode;
        }
            

        /// <summary>
        /// user interface version
        /// </summary>
        /// <param name="newNode"></param>
        public void Insert(T newNode)
        {
            this.root = Insert(this.root, newNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        private TreeNode<T> Insert(TreeNode<T> currentNode, T newData)
        {
            if(currentNode == null)
            {
                TreeNode<T> newNode = new TreeNode<T>(newData);
                currentNode = newNode;
                return currentNode;
            }

            if(Comparer<T>.Default.Compare(currentNode.data, newData) > 0)
            {
                currentNode.leftNode = Insert(currentNode.leftNode, newData);
                currentNode.leftNode.parentNode = currentNode;
            }
            else
            {
                currentNode.rightNode = Insert(currentNode.rightNode, newData);
                currentNode.rightNode.parentNode = currentNode;
            }

            currentNode = CheckBalance(currentNode);

            return currentNode;

        }

        public TreeNode<T> CheckBalance(TreeNode<T> n)
        {
            int balance = n.GetBalance();

            if (balance <= 1 && balance >= -1)
            {
                return n;
            }
            else if (balance == 2)
            {
                if (n.leftNode.GetBalance() == 1)
                {
                    n = RotateRight(n);
                }
                else
                {
                    n = RotateLeftRight(n);
                }
            }
            else if (balance == -2)
            {
                if (n.rightNode.GetBalance() == -1)
                {
                    n = RotateLeft(n);
                }
                else
                {
                    n = RotateRightLeft(n);
                }
            }

            return n;
        }

        public TreeNode<T> RotateRight(TreeNode<T> n)
        {
            TreeNode<T> left = n.leftNode;
            TreeNode<T> leftRight = left.rightNode;
            TreeNode<T> parent = n.parentNode;

            left.parentNode = parent;  //Set the new current's parent
            left.rightNode = n;  //set old current to the new currents right.  Left node is already set
            n.leftNode = leftRight;  //If the leftnode has a right node then set it to the new rights.leftnode
            n.parentNode = left;  //Reparent new right to the new current

            //Reparent the leaf
            if (leftRight != null)
            {
                leftRight.parentNode = n;
            }

            //Make the left node the new current
            if (n == this.root)
            {
                this.root = left;
            }
            else if (parent.leftNode == n)
            {
                parent.leftNode = left;
            }
            else
            {
                parent.rightNode = left;
            }

            return left;
        }

        public TreeNode<T> RotateLeft(TreeNode<T> n)
        {
            TreeNode<T> right = n.rightNode;
            TreeNode<T> rightLeft = right.leftNode;
            TreeNode<T> parent = n.parentNode;

            right.parentNode = parent;  //Reparent the new current
            right.leftNode = n;  
            n.rightNode = rightLeft;
            n.parentNode = right;

            //reparent the leaf
            if (rightLeft != null)
            {
                rightLeft.parentNode = n;
            }

            //Make the right node the new current
            if (n == this.root)
            {
                this.root = right;
            }
            else if (parent.rightNode == n)
            {
                parent.rightNode = right;
            }
            else
            {
                parent.leftNode = right;
            }

            return right;

        }

        public TreeNode<T> RotateLeftRight(TreeNode<T> n)
        {
            //Get nodes to move.  Copying them allows to easily overwrite nodes
            //with eachother
            TreeNode<T> left = n.leftNode;
            TreeNode<T> leftRight = left.rightNode;
            TreeNode<T> parent = n.parentNode;
            TreeNode<T> leftRightRight = leftRight.rightNode;
            TreeNode<T> leftRightLeft = leftRight.leftNode;

            //Transpose the nodes to their right place
            leftRight.parentNode = parent;  //First reparent the new current node
            n.leftNode = leftRightRight; //Set old current's leftnode to the right most leaf
            left.rightNode = leftRightLeft;  //Set left's rightnode to the left leaf
            leftRight.leftNode = left;  //Set the new current left to the left node
            leftRight.rightNode = n;  //set the new current's right node to the old current.
            left.parentNode = leftRight;  //Set left's parent to the new current
            n.parentNode = leftRight; //Set old current's (New Right) parent to the new current

            //Reparent the right leaf
            if (leftRightRight != null)
            {
                leftRightRight.parentNode = n;
            }

            //Reparent the left leaf
            if (leftRightLeft != null)
            {
                leftRightLeft.parentNode = left;
            }

            //Make leftRight the new current node
            if (n == this.root)
            {
                this.root = leftRight;
            }
            else if (parent.leftNode == n)
            {
                parent.leftNode = leftRight;
            }
            else
            {
                parent.rightNode = leftRight;
            }

            return leftRight;
        }

        public TreeNode<T> RotateRightLeft(TreeNode<T> n)
        {
            TreeNode<T> right = n.rightNode;
            TreeNode<T> rightLeft = right.leftNode;
            TreeNode<T> parent = n.parentNode;
            TreeNode<T> rightLeftLeft = rightLeft.leftNode;
            TreeNode<T> rightLeftRight = rightLeft.rightNode;

            rightLeft.parentNode = parent;
            n.rightNode = rightLeftLeft;
            right.leftNode = rightLeftRight;
            rightLeft.rightNode = right;
            rightLeft.leftNode = n;
            right.parentNode = rightLeft;
            n.parentNode = rightLeft;

            if (rightLeftLeft != null)
            {
                rightLeftLeft.parentNode = n;
            }

            if (rightLeftRight != null)
            {
                rightLeftRight.parentNode = right;
            }

            if (n == this.root)
            {
                this.root = rightLeft;
            }
            else if (parent.rightNode == n)
            {
                parent.rightNode = rightLeft;
            }
            else
            {
                parent.leftNode = rightLeft;
            }

            return rightLeft;
        }

        public bool Search(TreeNode<T> node, T searchData)
        {
            TreeNode<T> searchNode = new TreeNode<T>(searchData);

            if(node == null )
            {
                return false;
            }

            int compareValue = Comparer<T>.Default.Compare(node.data, searchData);

            if (node == searchNode)
            {
                return true;
            }
            else if (node < searchNode)
            {
                return Search(node.leftNode, searchData);
            }
            else
            {
                return Search(node.rightNode, searchData);
            }

            //switch (compareValue)
            //{
            //    case 0:
            //        return true;
            //    case 1:
            //        return Search(node.leftNode, searchData);
            //    case -1:
            //        return Search(node.rightNode, searchData);
            //};

            //return false;
        }

        public void Remove(T key)
        {
            this.root = Remove(this.root, key);
        }

        private TreeNode<T> Remove(TreeNode<T> node, T key)
        {
            /* Base Case: If the tree is empty */
            if (node == null)
            {
                return node;
            }

            int compareValue = Comparer<T>.Default.Compare(node.data, key);

            /* Otherwise, recur down the tree */
            if (compareValue == 1)
            {
                node.leftNode = Remove(node.leftNode, key);
            }
            else if (compareValue == -1)
            {
                node.rightNode = Remove(node.rightNode, key);
            }

            // if key is same as root's key, then This is the node
            // to be deleted
            else
            {
                // node with only one child or no child
                if (node.leftNode == null)
                {
                    return node.rightNode;
                }
                else if (node.rightNode == null)
                {
                    return node.leftNode;
                }

                // node with two children: Get the inorder successor (smallest
                // in the right subtree)
                node.data = minValue(node.rightNode);

                // Delete the inorder successor
                node.rightNode = Remove(node.rightNode, node.data);
            }

            return node;
        }

        private T minValue(TreeNode<T> n)
        {
            T minv = n.data;
            while (n.leftNode != null)
            {
                minv = n.leftNode.data;
                n = n.leftNode;
            }
            return minv;
        }

        private T maxValue(TreeNode<T> n)
        {
            T maxv = n.data;
            while (n.rightNode != null)
            {
                maxv = n.rightNode.data;
                n = n.rightNode;
            }
            return maxv;
        }

        public void BreathFirstTraversal()
        {
            // breadth-first Traversal
            Queue<TreeNode<T>> q = new Queue<TreeNode<T>>();

            //Add the root in the queue
            q.Enqueue(this.root);

            //Dequeue the top and then add its childern.
            //until the queue is empty.
            while (q.Count > 0)
            {
                
                TreeNode<T> n = q.Dequeue();
                Console.WriteLine(n.data);

                if (n.leftNode != null)
                {
                    q.Enqueue(n.leftNode);
                }

                if (n.rightNode != null)
                {
                    q.Enqueue(n.rightNode);
                }    
            }
        }

        public void DepthFirstPreOrderTraversal()
        {
            // depth-first using a stack
            Stack<TreeNode<T>> s = new Stack<TreeNode<T>>();
            s.Push(this.root);

            while (s.Count > 0)
            {
                TreeNode<T> n = s.Pop();

                Console.WriteLine(n.data);

                if (n.rightNode != null)
                {
                    s.Push(n.rightNode);
                }

                if (n.leftNode != null)
                {
                    s.Push(n.leftNode);
                }

            }
        }

        public void DepthFirstPostOrderTraversal_R(TreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            // first recur on left subtree
            DepthFirstPostOrderTraversal_R(node.leftNode);

            // then recur on right subtree
            DepthFirstPostOrderTraversal_R(node.rightNode);

            // now deal with the node
            Console.WriteLine(node.data);
        }

        public void DepthFirstPreOrderTraversal_R(TreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            // first recur on left subtree
            DepthFirstPreOrderTraversal_R(node.leftNode);

            // now deal with the node
            Console.WriteLine(node.data);

            // then recur on right subtree
            DepthFirstPreOrderTraversal_R(node.rightNode);

            
        }

        public void DepthFirstInOrderTraversal_R(TreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            // now deal with the node
            Console.WriteLine(node.data);

            // first recur on left subtree
            DepthFirstInOrderTraversal_R(node.leftNode);

            // then recur on right subtree
            DepthFirstInOrderTraversal_R(node.rightNode);
              
        }

        


        public class TreeNode<T>
        {
            public T data { get; set; }

            public TreeNode<T> parentNode { get; set; }

            public TreeNode<T> leftNode { get; set; }

            public TreeNode<T> rightNode { get; set; }

            public TreeNode(T newData)
            {
                data = newData;
            }

            public TreeNode(T newData, TreeNode<T> newLeftNode, TreeNode<T>newRightNode)
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

            private int GetHeight(TreeNode<T> node)
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

            public static bool operator ==(TreeNode<T> node1, TreeNode<T> node2)
            {
                if (object.ReferenceEquals(node2, null))
                {
                    return object.ReferenceEquals(node1, null);
                }

                return (Comparer<T>.Default.Compare(node1.data, node2.data) == 0);
            }

            public static bool operator !=(TreeNode<T> node1, TreeNode<T> node2)
            {
                if (object.ReferenceEquals(node2, null))
                {
                    return !object.ReferenceEquals(node1, null);
                }

                return (Comparer<T>.Default.Compare(node1.data, node2.data) != 0);
            }

            public static bool operator <(TreeNode<T> node1, TreeNode<T> node2)
            {

                return (Comparer<T>.Default.Compare(node1.data, node2.data) < 0);

            }

            public static bool operator >(TreeNode<T> node1, TreeNode<T> node2)
            {

                return (Comparer<T>.Default.Compare(node1.data, node2.data) > 0);

            }

            public static bool operator <=(TreeNode<T> node1, TreeNode<T> node2)
            {

                return (Comparer<T>.Default.Compare(node1.data, node2.data) <= 0);

            }

            public static bool operator >=(TreeNode<T> node1, TreeNode<T> node2)
            {

                return (Comparer<T>.Default.Compare(node1.data, node2.data) >= 0);

            }

        }
    }
}
