using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DsAlgo
{
    public class Heap<T> : ICollection<T>
    {
        private T[] store;
        private int size;
        private IComparer<T> comparer;

        private const int INITIAL_CAPACITY = 8;
        private const int CAPACITY_FACTOR = 2;

        public Heap(IComparer<T> comparer) : this(INITIAL_CAPACITY, comparer)
        {
        }

        public Heap(int capacity, IComparer<T> comparer)
        {
            if (capacity < 0) capacity = INITIAL_CAPACITY;

            store = new T[capacity];
            this.comparer = comparer;
        }

        public Heap(int capacity, IComparer<T> comparer, IEnumerable<T> items) : this(capacity, comparer)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        private void EnsureCapacity()
        {
            var newSize = size == 0 ? INITIAL_CAPACITY : size * CAPACITY_FACTOR;
            if (size == store.Length)
            {
                Resize(newSize);
            }
        }

        public void Trim()
        {
            if (size == store.Length) return;

            Resize(size);
        }

        private void Resize(int newSize)
        {
            var copy = new T[newSize];
            Array.Copy(store, copy, size);
            store = copy;
        }

        private void Swap(int index1, int index2)
        {
            var temp = store[index1];
            store[index1] = store[index2];
            store[index2] = temp;
        }

        public int Capacity => store.Length;

        private int LeftChildIndex(int index) => 2 * index + 1;

        private int RightChildIndex(int index) => 2 * index + 2;

        private int ParentIndex(int index) => (index - 1) / 2;

        private bool HasLeftChild(int index) => LeftChildIndex(index) < size;

        private bool HasRightChild(int index) => RightChildIndex(index) < size;

        private bool HasParent(int index) => ParentIndex(index) >= 0;

        private T LeftChild(int index) => store[LeftChildIndex(index)];

        private T RightChild(int index) => store[RightChildIndex(index)];

        private T Parent(int index) => store[ParentIndex(index)];

        public int Count => size;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            EnsureCapacity();
            store[size++] = item;
            HeapifyUp();
        }

        public void Clear()
        {
            if (size == 0) return;

            size = 0;
            store = new T[INITIAL_CAPACITY];
        }

        public bool Contains(T item)
        {
            return store.Take(size).Any(elem => comparer.Compare(item, elem) == 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }
            if (size + arrayIndex > array.Length)
            {
                throw new ArgumentException("The array is too small to fit the elements of this collection");
            }

            for (var i = 0; i < size; ++i)
            {
                array[arrayIndex++] = store[i];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return store.Take(size).AsEnumerable().GetEnumerator();
        }

        public bool Remove(T item)
        {
            for (var i = 0; i < size; ++i)
            {
                if (comparer.Compare(item, store[i]) == 0)
                {
                    store[i] = store[--size];
                    HeapifyDown();
                    return true;
                }
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Peek()
        {
            if (size == 0) throw new InvalidOperationException();

            return store[0];
        }

        public T Poll()
        {
            if (size == 0) throw new InvalidOperationException();

            var item = store[0];
            store[0] = store[--size];
            HeapifyDown();
            return item;
        }

        private void HeapifyUp()
        {
            var index = size - 1;
            while (HasParent(index) && comparer.Compare(Parent(index), store[index]) > 0)
            {
                Swap(ParentIndex(index), index);
                index = ParentIndex(index);
            }
        }

        private void HeapifyDown()
        {
            var index = 0;
            while (HasLeftChild(index)) // has children? if has not left, certainly has not right.
            {
                // Get the index of the minimum child between left and right.
                var minChildIndex = LeftChildIndex(index);
                if (HasRightChild(index) && comparer.Compare(RightChild(index), LeftChild(index)) < 0)
                {
                    minChildIndex = RightChildIndex(index);
                }
                
                if (comparer.Compare(store[index], store[minChildIndex]) <= 0)
                {
                    break; // heap property is satisfied; current element is less than children.
                }
                else
                {
                    Swap(index, minChildIndex);
                }

                index = minChildIndex;
            }
        }
    }
}