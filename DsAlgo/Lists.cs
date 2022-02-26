using System;
using System.Collections;
using System.Collections.Generic;

namespace DsAlgo
{
    public class LinkedList<T> : IList<T>, IReadOnlyList<T>
    {
        Node Head { get; set; }
        Node Tail { get; set; }
        int Version { get; set; }
        public int Count { get; private set; }
        public bool IsReadOnly => false;
        public Node? First => Count > 0 ? Head.R : null;
        public Node? Last => Count > 0 ? Tail.L : null;

        public LinkedList()
        {
            Head = new Node { List = this };
            Tail = new Node { List = this };
            Node.Link(Head, Tail);
        }

        public LinkedList(IEnumerable<T> enumerable) : this()
        {
            foreach (var item in enumerable)
            {
                Add(item);
            }
        }

        void RequireAttached(Node attachedNode)
        {
            if (!object.ReferenceEquals(this, attachedNode.List))
            {
                throw new InvalidOperationException("The attachedNode is not attached to this collection");
            }
        }

        void RequireDetached(Node detachedNode)
        {
            if (detachedNode.List is not null)
            {
                throw new InvalidOperationException("The detachedNode is not detached from its collection");
            }
        }

        public Node FindAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            var bound = Count;
            if (index < bound / 2)
            {
                var cur = Head;
                for (var i = 0; i <= index; ++i)
                {
                    cur = cur!.R;
                }
                return cur!;
            }
            else
            {
                var cur = Tail;
                for (var i = bound - 1; i >= index; --i)
                {
                    cur = cur!.L;
                }
                return cur!;
            }
        }

        public Node? FindFirst(T item) => FindFirstIndex(item).node;

        (int index, Node? node) FindFirstIndex(T item)
        {
            var i = 0;
            for (var cur = Head.R; cur != Tail; cur = cur.R)
            {
                if (object.Equals(item, cur!.Value))
                {
                    return (i, cur);
                }
                ++i;
            }
            return (-1, null);
        }

        public Node? FindLast(T item) => FindLastIndex(item).node;

        (int index, Node? node) FindLastIndex(T item)
        {
            var i = Count - 1;
            for (var cur = Tail.L; cur != Head; cur = cur.L)
            {
                if (object.Equals(item, cur!.Value))
                {
                    return (i, cur);
                }
                --i;
            }
            return (-1, null);
        }

        public void AttachAfter(Node attachedNode, Node detachedNode)
        {
            RequireAttached(attachedNode);
            RequireDetached(detachedNode);

            detachedNode.List = this;
            var old = attachedNode.R;
            Node.Link(attachedNode, detachedNode);
            Node.Link(detachedNode, old!);
            ++Count;
            ++Version;
        }

        public Node AttachAfter(Node attachedNode, T item)
        {
            var cur = new Node(item);
            AttachAfter(attachedNode, cur);
            return cur;
        }

        public void AttachBefore(Node attachedNode, Node detachedNode)
        {
            RequireAttached(attachedNode);
            RequireDetached(detachedNode);

            detachedNode.List = this;
            var old = attachedNode.L;
            Node.Link(detachedNode, attachedNode);
            Node.Link(old!, detachedNode);
            ++Count;
            ++Version;
        }

        public Node AttachBefore(Node attachedNode, T item)
        {
            var cur = new Node(item);
            AttachBefore(attachedNode, cur);
            return cur;
        }

        public void AttachFirst(Node detachedNode) => AttachAfter(Head, detachedNode);

        public Node AttachFirst(T item)
        {
            var cur = new Node(item);
            AttachFirst(cur);
            return cur;
        }

        public void AttachLast(Node detachedNode) => AttachBefore(Tail, detachedNode);

        public Node AttachLast(T item)
        {
            var cur = new Node(item);
            AttachLast(cur);
            return cur;
        }

        public T this[int index]
        {
            get => FindAt(index).Value!;
            set => FindAt(index).Value = value;
        }

        public void Add(T item) => AttachLast(new Node(item));

        public void Clear()
        {
            while (First is not null)
            {
                DetachFirst();
            }
        }

        public bool Contains(T item) => FindFirstIndex(item).node is not null;

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }
            if (Count + arrayIndex > array.Length)
            {
                throw new ArgumentException("The array is too small to fit the elements of this collection");
            }

            for (var cur = Head.R; cur != Tail; cur = cur.R)
            {
                array[arrayIndex++] = cur!.Value!;
            }
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => FindFirstIndex(item).index;

        public void Insert(int index, T item)
        {
            if (index == Count)
            {
                AttachLast(item);
                return;
            }
            var old = FindAt(index);
            AttachBefore(old, item);
        }

        void DetachNode(Node node)
        {
            Node.Link(node.L!, node.R!);
            node.Detach();
            --Count;
            ++Version;
        }

        public bool Remove(T item)
        {
            var cur = FindFirstIndex(item).node;
            if (cur is null)
            {
                return false;
            }
            DetachNode(cur);
            return true;
        }

        public void RemoveAt(int index) => DetachNode(FindAt(index));

        public void Detach(Node attachedNode)
        {
            RequireAttached(attachedNode);
            DetachNode(attachedNode);
        }

        public void DetachFirst()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The collection is empty");
            }
            DetachNode(First!);
        }

        public void DetachLast()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The collection is empty");
            }
            DetachNode(Last!);
        }


        public sealed class Node
        {
            internal Node? L { get; set; }
            internal Node? R { get; set; }
            internal LinkedList<T>? List { get; set; }
            public T? Value { get; set; }
            public Node? Left => List?.Head == L ? null : L;
            public Node? Right => List?.Tail == R ? null : R;
            public bool Detached => List is null && L is null && R is null;

            internal Node()
            {
                Value = default(T);
            }

            public Node(T value)
            {
                Value = value;
            }

            internal void Detach()
            {
                L = null;
                R = null;
                List = null;
            }

            internal static void Link(Node left, Node right)
            {
                left.R = right;
                right.L = left;
            }
        }


        class Enumerator : IEnumerator<T>
        {
            LinkedList<T> List { get; set; }
            Node Cursor { get; set; }
            int Version { get; set; }
            public T Current => Cursor.Value!;
            object IEnumerator.Current => Current!;

            internal Enumerator(LinkedList<T> list)
            {
                List = list;
                Version = list.Version;
                Cursor = list.Head;
            }

            void RequireNotModified()
            {
                if (Version != List.Version)
                {
                    throw new InvalidOperationException("The collection was modified after the enumerator was created");
                }
            }

            public bool MoveNext()
            {
                RequireNotModified();
                if (Cursor.R == List.Tail)
                {
                    return false;
                }

                Cursor = Cursor.R!;
                return true;
            }

            public void Reset()
            {
                RequireNotModified();
                Cursor = List.Head;
            }

            public void Dispose()
            {
                // no-op.
            }
        }
    }
}
