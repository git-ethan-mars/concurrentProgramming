using System;
using System.Threading;

namespace Homework3
{
    public class Stack<T> : IStack<T>
    {
        public class Node
        {
            public readonly T Value;
            public readonly Node Next;
            public readonly int Count;

            public Node(T value, int count, Node next)
            {
                Value = value;
                Count = count;
                Next = next;
            }
        }

        private Node _head;
        
        public void Push(T item)
        {
            var spinWait = new SpinWait();
            while (true)
            {
                var head = _head;
                var count = 1;
                if (head is not null)
                    count = head.Count + 1;
                var newNode = new Node(item, count, head);
                if (Interlocked.CompareExchange(ref _head, newNode, newNode.Next) == newNode.Next)
                {
                    break;
                }

                spinWait.SpinOnce();
            }
        }

        public bool TryPop(out T item)
        {
            var spinWait = new SpinWait();
            while (true)
            {
                var head = _head;
                if (head is null)
                {
                    item = default;
                    return false;
                }

                if (Interlocked.CompareExchange(ref _head, head.Next, head) == head)
                {
                    item = head.Value;
                    return true;
                }

                spinWait.SpinOnce();
            }
        }

        public int Count {
            get
            {
                var head = _head;
                if (head is not null)
                    return head.Count;
                return 0;
            }
        }


        public int SlowCount()
        {
            var count = 0;
            var currentNode = _head;
            while (currentNode is not null)
            {
                currentNode = currentNode.Next;
                count++;
            }

            return count;
        }

        public void Print()
        {
            var currentNode = _head;
            while (currentNode is not null)
            {
                Console.WriteLine(currentNode.Value);
                currentNode = currentNode.Next;
            }
        }
    }
}