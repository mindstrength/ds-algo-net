using System;
using System.Collections.Generic;
using Xunit;

namespace DsAlgo.Tests
{
    public class HeapStackTests
    {
        Random R = new Random();

        [Fact]
        public void Ctor_Empty()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            Assert.Equal(0, heap.Count);
        }

        [Fact]
        public void Count()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            var count = R.Next(1, 10);
            for (int i = 0; i < count; ++i) {
                heap.Add(R.Next());
            }
            Assert.Equal(count, heap.Count);
        }

        [Fact]
        public void PeekStack()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            var count = R.Next(3, 20);
            var last = 0;
            for (int i = 0; i < count; ++i) {
                var value = R.Next(int.MinValue, int.MaxValue);
                last = value;
                heap.Add(value);
            }
            Assert.Equal(last, heap.PeekStack());
        }

        [Fact]
        public void PeekStack_Empty()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            Assert.Throws<InvalidOperationException>(() => heap.PeekStack());
        }

        [Fact]
        public void PeekHeap()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            var count = R.Next(3, 20);
            var min = int.MaxValue;
            for (int i = 0; i < count; ++i) {
                var value = R.Next(int.MinValue, int.MaxValue);
                if (value < min) min = value;
                heap.Add(value);
            }
            Assert.Equal(min, heap.PeekHeap());
        }

        [Fact]
        public void PeekHeap_Empty()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            Assert.Throws<InvalidOperationException>(() => heap.PeekHeap());
        }

        [Fact]
        public void PopStack()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            var count = R.Next(3, 20);
            var min = int.MaxValue;
            var last = 0;
            for (int i = 0; i < count; ++i) {
                var value = R.Next(int.MinValue, int.MaxValue);
                if (value < min) min = value;
                last = value;
                heap.Add(value);
            }
            Assert.Equal(min, heap.PeekHeap());
            Assert.Equal(last, heap.PopStack());
        }

         [Fact]
        public void PopStack_Empty()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            Assert.Throws<InvalidOperationException>(() => heap.PopStack());
        }

        [Fact]
        public void PopStack_All()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            var stack = new Stack<int>();
            var count = R.Next(3, 20);
            for (int i = 0; i < count; ++i) {
                var value = R.Next(int.MinValue, int.MaxValue);
                heap.Add(value);
                stack.Push(value);
            }
            while (stack.Count > 0) {
                Assert.Equal(stack.Pop(), heap.PopStack());
            }
            Assert.Equal(0, heap.Count);
        }

        [Fact]
        public void PopHeap()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            var count = R.Next(3, 20);
            var min = int.MaxValue;
            var last = 0;
            for (int i = 0; i < count; ++i) {
                var value = R.Next(int.MinValue, int.MaxValue);
                if (value < min) min = value;
                last = value;
                heap.Add(value);
            }
            Assert.Equal(last, heap.PeekStack());
            Assert.Equal(min, heap.PopHeap());
        }

         [Fact]
        public void PopHeap_Empty()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            Assert.Throws<InvalidOperationException>(() => heap.PopHeap());
        }

        [Fact]
        public void PopHeap_All()
        {
            var heap = new HeapStack<int>(Comparer<int>.Default);
            var queue = new PriorityQueue<int, int>();
            var count = R.Next(3, 20);
            for (int i = 0; i < count; ++i) {
                var value = R.Next(int.MinValue, int.MaxValue);
                heap.Add(value);
                queue.Enqueue(value, value);
            }
            while (queue.Count > 0) {
                Assert.Equal(queue.Dequeue(), heap.PopHeap());
            }
            Assert.Equal(0, heap.Count);
        }
    }
}