using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReEvolveCSharpLibrary.DataStructures
{
    public class AdjGraph<K>
    {
        public AdjGraphNode<K> startNode {get;set;}

        public int count { get; set; }

        public AdjGraph(K data)
        {
            startNode = new AdjGraphNode<K>(data);
            count++;
        }

        public AdjGraph()
        {
            startNode = null;
            count = 0;
        }

        public List<K> BreadthFirstTraversal()
        {
            Guid visitedId = Guid.NewGuid();
            return BreadthFirstTraversal(visitedId);
        }

        private List<K> BreadthFirstTraversal(Guid visitId)
        {
            List<K> BFList = new List<K>();

            Queue<AdjGraphNode<K>> q = new Queue<AdjGraphNode<K>>();
            startNode.visitId = visitId;
            q.Enqueue(startNode);

            while (q.Count > 0)
            {
                AdjGraphNode<K> n = q.Dequeue();

                BFList.Add(n.data);

                foreach (AdjGraphNode<K> adj in n.neighbours)
                {
                    if (adj.visitId != visitId)
                    {
                        adj.visitId = visitId;
                        q.Enqueue(adj);
                    }
                }

            }

            return BFList;
        }

        public AdjGraphNode<K> BreadthFirstSearch(K searchNode, Guid visitId)
        {
            return BreadthFirstSearch(this.startNode, searchNode, visitId);
        }

        public AdjGraphNode<K> BreadthFirstSearch(AdjGraphNode<K> startNode, K searchNode, Guid visitId)
        {
            Queue<AdjGraphNode<K>> q = new Queue<AdjGraphNode<K>>();
            startNode.visitId = visitId;
            q.Enqueue(startNode);

            while (q.Count > 0)
            {
                AdjGraphNode<K> n = q.Dequeue();
                Console.WriteLine(n.data.ToString());

                if (n.data.Equals(searchNode))
                {
                    return n;
                }

                foreach (AdjGraphNode<K> adj in n.neighbours)
                {
                    if (adj.visitId != visitId)
                    {
                        adj.visitId = visitId;
                        q.Enqueue(adj);
                    }
                }
            }

            return null;
        }

        public List<K> DepthFirstTraversal(AdjGraphNode<K> node, Guid visitId, List<K> list)
        {
            if (node.visitId == visitId)
            {
                return list;
            }

            node.visitId = visitId;
            list.Add(node.data);

            foreach (AdjGraphNode<K> n in node.neighbours)
            {
                list = DepthFirstTraversal(n, visitId, list);
            }

            return list;
        }

        public List<K> DepthFirstTraversal()
        {
            Guid visitedId = Guid.NewGuid();
            return DepthFirstTraversal(this.startNode, visitedId, new List<K>());
        }

        /// <summary>
        /// Finds if there is a path between A and B using DFS
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool PathBetweenABExist(K a, K b, Guid visitId)
        {
            AdjGraphNode<K> nodeA = BreadthFirstSearch(this.startNode,a, Guid.NewGuid());

            if (nodeA != null)
            {
                AdjGraphNode<K> nodeB = BreadthFirstSearch(nodeA, b, Guid.NewGuid());

                if(nodeB != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool PathBetweenABExist(K a, K b)
        {
            return PathBetweenABExist(a, b, Guid.NewGuid());
        }

        public void AppendNodes(K nodeA, K nodeB)
        {
            AppendNodes(nodeA, nodeB, Guid.NewGuid());
        }

        public void AppendNodes(K nodeA, K nodeB, Guid visitId)
        {
            AdjGraphNode<K> a = BreadthFirstSearch(nodeA, Guid.NewGuid());

            if(a != null)
            {
                AdjGraphNode<K> b = BreadthFirstSearch(nodeB, Guid.NewGuid());
                if (b != null)
                {
                    a.neighbours.Add(b);
                }
                else
                {
                    b = new AdjGraphNode<K>(nodeB);
                    a.neighbours.Add(b);
                    count++;
                }
                
            }
        }

        public class AdjGraphNode<T>
        {
            public T data { get; set; }
            public List<AdjGraphNode<T>> neighbours { get; set; }

            public Guid visitId { get; set; }

            public AdjGraphNode(T newData)
            {
                this.data = newData;
                this.neighbours = new List<AdjGraphNode<T>>();
            }

            public static bool operator ==(AdjGraphNode<T> node1, AdjGraphNode<T> node2)
            {
                if (object.ReferenceEquals(node2, null))
                {
                    return object.ReferenceEquals(node1, null);
                }

                return (Comparer<T>.Default.Compare(node1.data, node2.data) == 0);
            }

            public static bool operator !=(AdjGraphNode<T> node1, AdjGraphNode<T> node2)
            {
                if (object.ReferenceEquals(node2, null))
                {
                    return !object.ReferenceEquals(node1, null);
                }

                return (Comparer<T>.Default.Compare(node1.data, node2.data) != 0);
            }

            public override bool Equals(Object obj)
            {
                // Check for null values and compare run-time types.
                if (obj == null || GetType() != obj.GetType())
                    return false;

                AdjGraphNode<T> node2 = (AdjGraphNode<T>)obj;
                return (Comparer<T>.Default.Compare(this.data, node2.data) == 0);
            }
            public override int GetHashCode()
            {
                return this.data.GetHashCode();
            }

        }
    }
}
