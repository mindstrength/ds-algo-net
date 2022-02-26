using System;
using System.Collections;
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
            Assert.Equal(0, list.First!.Value);
            Assert.Equal(2, list.Last!.Value);
        }

        [Fact]
        public void IsReadOnly()
        {
            var list = new LinkedList<int>();
            Assert.False(list.IsReadOnly);
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
            Assert.Same(first, result!.Value);
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
            Assert.Same(last, result!.Value);
        }

        [Fact]
        public void FindLast_Absent()
        {
            var list = new LinkedList<string> { "a" };
            var result = list.FindLast("b");
            Assert.Null(result);
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
            Assert.Throws<InvalidOperationException>(() => list.AttachAfter(list.First!, list.Last!));
        }

        [Fact]
        public void AttachAfter_Node()
        {
            var list = new LinkedList<int> { 1 };
            var node = new LinkedList<int>.Node(2);
            list.AttachAfter(list.Last!, node);
            Assert.Equal(list.Last, node);
        }

        [Fact]
        public void AttachAfter_Item()
        {
            var list = new LinkedList<int> { 1 };
            var item = 2;
            list.AttachAfter(list.Last!, item);
            Assert.Equal(list.Last!.Value, item);
        }

        [Fact]
        public void AttachBefore_Node_AttachedIsDetached()
        {
            var list = new LinkedList<int> { 1 };
            Assert.Throws<InvalidOperationException>(() => list.AttachBefore(new LinkedList<int>.Node(1), new LinkedList<int>.Node(2)));
        }

        [Fact]
        public void AttachBefore_Node_DetachedIsAttached()
        {
            var list = new LinkedList<int> { 1, 2 };
            Assert.Throws<InvalidOperationException>(() => list.AttachBefore(list.Last!, list.First!));
        }

        [Fact]
        public void AttachBefore_Node()
        {
            var list = new LinkedList<int> { 1 };
            var node = new LinkedList<int>.Node(2);
            list.AttachBefore(list.First!, node);
            Assert.Equal(list.First, node);
        }

        [Fact]
        public void AttachBefore_Item()
        {
            var list = new LinkedList<int> { 2 };
            var item = 1;
            list.AttachBefore(list.First!, item);
            Assert.Equal(list.First!.Value, item);
        }

        [Fact]
        public void AttachFirst_Node()
        {
            var list = new LinkedList<int> { 2 };
            var node = new LinkedList<int>.Node(1);
            list.AttachFirst(node);
            Assert.Equal(list.First, node);
        }

        [Fact]
        public void AttachFirst_Item()
        {
            var list = new LinkedList<int> { 2 };
            var item = 1;
            var result = list.AttachFirst(item);
            Assert.Equal(list.First, result);
        }

        [Fact]
        public void AttachLast_Node()
        {
            var list = new LinkedList<int> { 1 };
            var node = new LinkedList<int>.Node(2);
            list.AttachLast(node);
            Assert.Equal(list.Last, node);
        }

        [Fact]
        public void AttachLast_Item()
        {
            var list = new LinkedList<int> { 1 };
            var item = 2;
            var result = list.AttachLast(item);
            Assert.Equal(list.Last, result);
        }

        [Fact]
        public void Indexer()
        {
            var list = new LinkedList<char> { 'a', 'b'};
            list[0] = 'c';
            Assert.Equal('c', list[0]);
        }

        [Fact]
        public void Clear()
        {
            var list = new LinkedList<int> { 1, 2 };
            // Hold a reference to a node to ensure it is detached when the list is cleared.
            var node = list.First;
            list.Clear();
            Assert.Empty(list);
            Assert.Null(list.First);
            Assert.Null(list.Last);
            Assert.True(node!.Detached);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(3, false)]
        public void Contains(int item, bool expected)
        {
            var list = new LinkedList<int> { 1, 2 };
            var result = list.Contains(item);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(1)]
        public void CopyTo_ArgumentOutOfRange(int arrayIndex)
        {
            var list = new LinkedList<int> { 1 };
            int[] array = new int[list.Count];
            Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(array, arrayIndex));
        }

        [Fact]
        public void CopyTo_ArgumentException()
        {
            var list = new LinkedList<int> { 1, 2 };
            int[] array = new int[4];
            // [0, 0, 0, 1] 2 // Not all the elements fit in the array at the given arrayIndex.
            Assert.Throws<ArgumentException>(() => list.CopyTo(array, 3));
        }

        [Fact]
        public void CopyTo()
        {
            var list = new LinkedList<int> { 1, 2 };
            int[] array = new int[4];
            // [0, 0, 1, 2] // All the elements fit in the array at the given arrayIndex.
            list.CopyTo(array, 2);
            Assert.Equal(new int[] { 0, 0, 1, 2 }, array);
        }

        [Fact]
        public void GetEnumerator_Generic()
        {
            var list = new LinkedList<int> { 1, 2, 3, 4 };
            var enumerator = list.GetEnumerator();
            for (var item = 1; enumerator.MoveNext(); ++item)
            {
                Assert.Equal(item, enumerator.Current);
            }
            enumerator.Reset();
            for (var item = 1; enumerator.MoveNext(); ++item)
            {
                Assert.Equal(item, enumerator.Current);
            }
            list.Add(5);
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
        }

        [Fact]
        public void GetEnumerator()
        {
            var list = new LinkedList<int> { 1, 2, 3, 4 };
            var enumerator = ((IEnumerable)list).GetEnumerator();
            for (var item = 1; enumerator.MoveNext(); ++item)
            {
                Assert.Equal(item, enumerator.Current);
            }
            enumerator.Reset();
            for (var item = 1; enumerator.MoveNext(); ++item)
            {
                Assert.Equal(item, enumerator.Current);
            }
            list.Add(5);
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 0)]
        [InlineData(3, -1)]
        public void IndexOf(int item, int expected)
        {
            var list = new LinkedList<int> { 1, 2 };
            var result = list.IndexOf(item);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1, 2)]
        [InlineData(3, 3)]
        public void Insert_OutOfBounds(int index, int item)
        {
            var list = new LinkedList<int> { 1, 2 };
            Assert.Throws<IndexOutOfRangeException>(() => list.Insert(index, item));
        }

        [Fact]
        public void Insert_First()
        {
            var list = new LinkedList<int> { 2 };
            list.Insert(0, 1);
            Assert.Equal(1, list.First!.Value);
        }

        [Fact]
        public void Insert_Last()
        {
            var list = new LinkedList<int> { 1 };
            list.Insert(list.Count, 2);
            Assert.Equal(2, list.Last!.Value);
        }

        [Fact]
        public void Remove_Absent()
        {
            var list = new LinkedList<int> { 1 };
            var result = list.Remove(2);
            Assert.False(result);
        }
        [Fact]
        public void Remove()
        {
            string first = new String('a', 5), last = new String('a', 5);
            var list = new LinkedList<string> { first, last };
            var result = list.Remove("aaaaa");
            Assert.True(result);
            Assert.Collection<string>(
                list,
                (item) => Assert.Same(last, item)
            );
        }

        [Fact]
        public void RemoveAt()
        {
            var list = new LinkedList<int> { 1 };
            var node = list.First;
            list.RemoveAt(0);
            Assert.True(node!.Detached);
        }

        [Fact]
        public void Detach_Detached()
        {
            var node = new LinkedList<int>.Node(1);
            var list = new LinkedList<int> { 1 };
            Assert.Throws<InvalidOperationException>(() => list.Detach(node));
        }

        [Fact]
        public void Detach_Attached()
        {
            var list = new LinkedList<int> { 1, 2 };
            var node = list.First;
            list.Detach(node!);
            Assert.True(node!.Detached);
            Assert.Collection<int>(
                list,
                (item) => Assert.Equal(2, item)
            );
        }

        [Fact]
        public void DetachFirst_Empty()
        {
            var list = new LinkedList<int>();
            Assert.Throws<InvalidOperationException>(list.DetachFirst);
        }

        [Fact]
        public void DetachFirst()
        {
            var list = new LinkedList<int> { 1, 2 };
            var node = list.First;
            list.DetachFirst();
            Assert.True(node!.Detached);
            Assert.Collection<int>(
                list,
                (item) => Assert.Equal(2, item)
            );
        }

        [Fact]
        public void DetachLast_Empty()
        {
            var list = new LinkedList<int>();
            Assert.Throws<InvalidOperationException>(list.DetachLast);
        }

        [Fact]
        public void DetachLast()
        {
            var list = new LinkedList<int> { 1, 2 };
            var node = list.Last;
            list.DetachLast();
            Assert.True(node!.Detached);
            Assert.Collection<int>(
                list,
                (item) => Assert.Equal(1, item)
            );
        }

        [Fact]
        public void Node_Left()
        {
            var list = new LinkedList<int> { 1, 2 };
            var node = list.Last;
            Assert.Equal(1, node!.Left!.Value);
            node = node.Left; // First.
            Assert.Null(node.Left);
        }

        [Fact]
        public void Node_Right()
        {
            var list = new LinkedList<int> { 1, 2 };
            var node = list.First;
            Assert.Equal(2, node!.Right!.Value);
            node = node.Right; // Last.
            Assert.Null(node.Right);
        }
    }
}
