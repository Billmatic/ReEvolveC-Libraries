using System;
using System.Collections.Generic;

namespace ReEvolveCSharpLibrary.DataStructures
{
    public class AdjGraphNode<T>
    {
        public T data { get; set; }
        public List<AdjGraphNode<T>> neighbours { get; set; }

        Guid visitId { get; set; }

        public AdjGraphNode(T newData)
        {
            this.data = newData;
            this.neighbours = new List<AdjGraphNode<T>>();
        }

        public  void DepthFirstTraversal(AdjGraphNode<T> node, Guid visitId)
        {
            if (node.visitId == visitId)
            {
                return;
            }

            node.visitId = visitId;
            Console.WriteLine(node.data);

            foreach (AdjGraphNode<T> n in node.neighbours)
            {
                DepthFirstTraversal(n, visitId);
            }
        }

        public void DepthFirstTraversal()
        {
            Guid visitedId = Guid.NewGuid();
            DepthFirstTraversal(this, visitedId);
        }

        public void BreadthFirstTraversal(AdjGraphNode<T> node, Guid visitId)
        {
            Queue <AdjGraphNode<T>> q = new Queue<AdjGraphNode<T>>();
            node.visitId = visitId;
            q.Enqueue(node);

            while(q.Count > 0)
            {
                AdjGraphNode<T> n = q.Dequeue();
                Console.WriteLine(n.data.ToString());

                foreach (AdjGraphNode<T> adj in n.neighbours)
                {
                    if (adj.visitId != visitId)
                    {
                        adj.visitId = visitId;
                        q.Enqueue(adj);
                    }
                    
                }

            }
        }

        public void BreadthFirstTraversal()
        {
            Guid visitedId = Guid.NewGuid();
            BreadthFirstTraversal(this, visitedId);
        }

    }
}
