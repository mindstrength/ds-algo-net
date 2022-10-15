using System;
using System.Collections.Generic;

namespace DsAlgo
{
    public class HeapStack<T>
    {
        class Node<V>
        {
            public V Value { get; set; }
            public Node<V>? Left { get; set; }
            public Node<V>? Right { get; set; }

            public Node(V value)
            {
                Value = value;
            }

            public static void Link(Node<V>? Left, Node<V>? Right)
            {
                if (Left is not null) Left.Right = Right;
                if (Right is not null) Right.Left = Left;
            }
        }

        IComparer<Node<T>> cmp;

        Heap<Node<T>> heap;

        Node<T>? stackHead;

        class NodeComparer<V> : IComparer<Node<V>>
        {
            IComparer<V> itemCmp;

            public NodeComparer(IComparer<V> comparer)
            {
                itemCmp = comparer;
            }

            public int Compare(HeapStack<T>.Node<V>? x, HeapStack<T>.Node<V>? y)
            {
                return itemCmp.Compare(x is null ? default : x.Value, y is null ? default : y.Value);
            }
        }

        public HeapStack(IComparer<T> comparer)
        {
            this.cmp = new NodeComparer<T>(comparer);
            heap = new Heap<Node<T>>(cmp);
        }

        public int Count => heap.Count;

        public void Add(T item)
        {
            // O(log n)
            var node = new Node<T>(item);
            Node<T>.Link(stackHead, node);
            stackHead = node;
            heap.Add(node);
        }

        public T PeekStack()
        {
            // O(1)
            if (stackHead is null) throw new InvalidOperationException();

            return stackHead.Value;
        }

        public T PeekHeap()
        {
            // O(1)
            return heap.Peek().Value; // throws InvalidOperationException if empty.
        }

        public T PopStack()
        {   // O(n)
            if (stackHead is null) throw new InvalidOperationException();
            var item = stackHead.Value;
            heap.RemoveReference(stackHead);
            Node<T>.Link(stackHead.Left, stackHead.Right);
            stackHead = stackHead.Left;
            return item;
        }

        public T PopHeap()
        {   // O(log n)
            var node = heap.Poll(); // throws InvalidOperationException if empty.
            Node<T>.Link(node.Left, node.Right);
            if (object.ReferenceEquals(stackHead, node)) stackHead = node.Left;
            return node.Value;
        }

        public bool Contains(T item)
        {
            // O(n)
            return heap.Contains(new Node<T>(item));
        }
    }
}