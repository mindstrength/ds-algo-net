using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace DsAlgo.Tests
{
    public class HeapTests
    {
        [Fact]
        public void Ctor_Empty()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.Empty(heap);
        }

        [Fact]
        public void Capacity()
        {
            var heap = new Heap<int>(1, Comparer<int>.Default);
            Assert.Equal(1, heap.Capacity);
            Assert.Empty(heap);
        }

        [Fact]
        public void IsReadOnly()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.False(heap.IsReadOnly);
        }

        [Fact]
        public void Add()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            var items = new List<int> { 1, 2, 3, 4, 5, 5, 6, 7 };
            foreach (var item in items)
            {
                heap.Add(item);
            }
            Assert.Equal(items, heap);
        }

        [Fact]
        public void Clear()
        {
            var heap = new Heap<int>(4, Comparer<int>.Default, new List<int> { 1, 2, 3, 4 });
            heap.Clear();
            Assert.Empty(heap);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void Contains(int item, bool expected)
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 1, 3 };
            Assert.Equal(expected, heap.Contains(item));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void CopyTo_ArgumentOutOfRange(int arrayIndex)
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 1 };
            int[] array = new int[heap.Count];
            Assert.Throws<ArgumentOutOfRangeException>(() => heap.CopyTo(array, arrayIndex));
        }

        [Fact]
        public void CopyTo_ArgumentException()
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 1, 2 };
            int[] array = new int[4];
            // [0, 0, 0, 1] 2 // Not all the elements fit in the array at the given arrayIndex.
            Assert.Throws<ArgumentException>(() => heap.CopyTo(array, 3));
        }

        [Fact]
        public void CopyTo()
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 1, 2 };
            int[] array = new int[4];
            // [0, 0, 1, 2] // All the elements fit in the array at the given arrayIndex.
            heap.CopyTo(array, 2);
            Assert.Equal(new int[] { 0, 0, 1, 2 }, array);
        }

        [Fact]
        public void GetEnumerator_Generic()
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 1, 2, 3, 4 };
            var enumerator = heap.GetEnumerator();
            for (var item = 1; enumerator.MoveNext(); ++item)
            {
                Assert.Equal(item, enumerator.Current);
            }
            heap.Add(5);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void GetEnumerator()
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 1, 2, 3, 4 };
            var enumerator = ((IEnumerable)heap).GetEnumerator();
            for (var item = 1; enumerator.MoveNext(); ++item)
            {
                Assert.Equal(item, enumerator.Current);
            }
            heap.Add(5);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void Peek_Empty()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.Throws<InvalidOperationException>(() => heap.Peek());
        }

        [Fact]
        public void Peek_Min()
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 3, 1, 2 };
            Assert.Equal(1, heap.Peek()); // returns the minimum value.
        }

        [Fact]
        public void Peek_Max()
        {
            var heap = new Heap<int>(Comparer<int>.Create((a, b) => b.CompareTo(a))) { 3, 1, 2 };
            Assert.Equal(3, heap.Peek()); // returns the maximum value.
        }

        [Fact]
        public void Poll_Empty()
        {
            var heap = new Heap<int>(Comparer<int>.Default);
            Assert.Throws<InvalidOperationException>(() => heap.Poll());
        }

        [Fact]
        public void Poll()
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 3, 1, 2 };
            for (var i = 1; i <= 3; ++i)
            {
                Assert.Equal(i, heap.Poll());
            }
            Assert.Empty(heap);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void Remove(int item, bool expected)
        {
            var heap = new Heap<int>(Comparer<int>.Default) { 3, 1, 8, 7, 5, 4, 6 };
            Assert.Equal(expected, heap.Remove(item));

            // Poll the remaining items into a list
            // and compare against a sorted ordering of that list
            // to ensure that the Heap maintained its heap property (i.e. poll always returns the "minimum").
            var items = new List<int>();
            while (heap.Count > 0)
            {
                items.Add(heap.Poll());
            }
            var itemsSorted = new List<int>(items);
            itemsSorted.Sort();
            Assert.Equal(itemsSorted, items);
        }

        [Fact]
        public void Trim_Empty()
        {
            var heap = new Heap<int>(0, Comparer<int>.Default);
            heap.Trim();
            Assert.Equal(0, heap.Capacity);
        }

        [Fact]
        public void Trim()
        {
            var heap = new Heap<int>(8, Comparer<int>.Default) { 1, 2 };
            heap.Trim();
            Assert.Equal(heap.Count, heap.Capacity);
        }
    }
}