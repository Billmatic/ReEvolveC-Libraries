using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReEvolveCSharpLibrary.Entities
{
    public class Node
    {
        Node next = null;
        Node prev = null;
        string data = "";

        public Node(string content)
        {
            data = content;
        }

        void AppendToTail(string content)
        {
            Node tail = new Node(content);
            Node n = this;

            while (n.next != null)
            {
                n = n.next;
            }
            n.next = tail;
        }

        public Node DeleteNode(Node head, string content)
        {
            Node n = head;

            if (n.data == content)
            {
                return head.next;
            }

            while (n.next != null)
            {
                if (n.next.data == content)
                {
                    n.next = n.next.next;
                    return head;
                }
                n = n.next;
            }
            return head;
        }
    }
}
