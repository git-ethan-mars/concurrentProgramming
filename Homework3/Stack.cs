using System;
using System.Threading;

namespace Homework3
{
    public class Stack<T> : IStack<T>
    {
        public class Node
        {
            public readonly T Value;
            public Node Next;

            public Node(T value)
            {
                Value = value;
            }
        }

        private Node _head;
        private int _count;

        public void Push(T item)
        {
            var head = _head;
            var newNode = new Node(item);
            var spinWait = new SpinWait();
            while (Interlocked.CompareExchange(ref _head, newNode, head) != head)
            {
                head = _head;
                spinWait.SpinOnce();
            }

            _head = newNode;
            _head.Next = head;
            Interlocked.Increment(ref _count);
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
                    Interlocked.Decrement(ref _count);
                    return true;
                }
                spinWait.SpinOnce();
            }
        }

        public int Count => _count;

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