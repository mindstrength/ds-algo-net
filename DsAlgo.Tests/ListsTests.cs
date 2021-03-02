using System;
using Xunit;

namespace DsAlgo.Tests
{
    public class LinkedListTests
    {
        [Fact]
        public void Ctor_Empty()
        {
            var list = new LinkedList<int>();
            Assert.Empty(list);
            Assert.Null(list.First);
            Assert.Null(list.Last);
        }

        [Fact]
        public void Ctor_Populated()
        {
            var list = new LinkedList<int>(new int[] {0, 1, 2});
            Assert.Equal(3, list.Count);
            Assert.Equal(0, list.First.Value);
            Assert.Equal(2, list.Last.Value);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void FindAt_IndexOutOfRange(int index)
        {
            var list = new LinkedList<int> { 1 };
            Assert.Throws<IndexOutOfRangeException>(() => list.FindAt(index));
        }

        [Fact]
        public void FindAt()
        {
            var count = 10;
            var list = new LinkedList<int>();
            for (var i = 0; i < count; ++i)
            {
                var item = i + 1;
                list.Add(item);
            }
            for (var i = 0; i < list.Count; ++i)
            {
                var expected = i + 1;
                var result = list.FindAt(i);
                Assert.Equal(expected, result.Value);
            }
        }

        [Fact]
        public void FindFirst_Present()
        {
            string first = new String('a', 5), last = new String('a', 5);
            var list = new LinkedList<string> { first, last };
            var result = list.FindFirst(first);
            Assert.Same(first, result.Value);
        }

        [Fact]
        public void FindFirst_Absent()
        {
            var list = new LinkedList<string> { "a" };
            var result = list.FindFirst("b");
            Assert.Null(result);
        }

        [Fact]
        public void FindLast_Present()
        {
            string first = new String('a', 5), last = new String('a', 5);
            var list = new LinkedList<string> { first, last };
            var result = list.FindLast(last);
            Assert.Same(last, result.Value);
        }

        [Fact]
        public void FindLast_Absent()
        {
            var list = new LinkedList<string> { "a" };
            var result = list.FindLast("b");
            Assert.Null(result);
        }

        [Fact]
        public void AttachAfter_Node_AttachedNull()
        {
            var list = new LinkedList<int> { 1 };
            Assert.Throws<ArgumentNullException>(() => list.AttachAfter(null, new LinkedList<int>.Node(2)));
        }

        [Fact]
        public void AttachAfter_Node_DetachedNull()
        {
            var list = new LinkedList<int> { 1 };
            Assert.Throws<ArgumentNullException>(() => list.AttachAfter(list.First, null));
        }

        [Fact]
        public void AttachAfter_Node_AttachedIsDetached()
        {
            var list = new LinkedList<int> { 1 };
            Assert.Throws<InvalidOperationException>(() => list.AttachAfter(new LinkedList<int>.Node(1), new LinkedList<int>.Node(2)));
        }

        [Fact]
        public void AttachAfter_Node_DetachedIsAttached()
        {
            var list = new LinkedList<int> { 1, 2 };
            Assert.Throws<InvalidOperationException>(() => list.AttachAfter(list.First, list.Last));
        }

        [Fact]
        public void AttachAfter_Node()
        {
            var list = new LinkedList<int> { 1 };
            var node = new LinkedList<int>.Node(2);
            list.AttachAfter(list.Last, node);
            Assert.Equal(list.Last, node);
        }

        [Fact]
        public void AttachAfter_Item()
        {
            var list = new LinkedList<int> { 1 };
            var item = 2;
            list.AttachAfter(list.Last, 2);
            Assert.Equal(list.Last.Value, item);
        }
    }
}
