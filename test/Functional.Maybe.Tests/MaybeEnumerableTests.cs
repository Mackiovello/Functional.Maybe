
namespace Functional.Maybe.Tests
{
    using System;
    using Xunit;
    using System.Linq;

    public class MaybeEnumerableTests
    {
        [Fact]
        public void WhereValueExist_Should_remove_Nothing_values()
        {
            var sequence = new Maybe<int>[] { 1.ToMaybe(), Maybe<int>.Nothing, 2.ToMaybe() };
            int[] expected = { 1, 2 };

            var actual = sequence.WhereValueExist().ToArray();

            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void Given_ThreeSome_UnionReturnsCollectionOfAll()
        {
            var one = 1.ToMaybe();
            var two = 2.ToMaybe();
            var three = 3.ToMaybe();

            var res = one.Union(two, three);
            Assert.Equal(3, res.Count());
            Assert.True(res.SequenceEqual(new[] { 1, 2, 3 }));
        }

        [Fact]
        public void Given_OneSome_UnionReturnsCollectionOfOne()
        {
            var one = 1.ToMaybe();
            var two = Maybe<int>.Nothing;

            var res = one.Union(two);
            Assert.Single(res);
            Assert.True(res.SequenceEqual(new[] { 1 }));
        }

        [Fact]
        public void Given_CollectionAndSome_UnionReturnsCollectionPlusSome()
        {
            var one = new[] { 1, 3 };
            var two = 2.ToMaybe();

            var res = one.Union(two);
            Assert.Equal(3, res.Count());
            Assert.True(res.SequenceEqual(new[] { 1, 3, 2 }));
        }

        [Fact]
        public void FirstMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
        {
            var maybe = Enumerable.Empty<object>().FirstMaybe();

            Assert.True(maybe.IsNothing());
        }

        [Fact]
        public void FirstMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
        {
            var collection = new[] { 1, 2 };
            var itemToSearch = 3;

            var maybe = collection.FirstMaybe(i => i == itemToSearch);

            Assert.True(maybe.IsNothing());
        }

        [Fact]
        public void FirstMaybe_WhenCalledOnEnumerableWithMatches_ReturnsFirstMatchingElement()
        {
            var expectedItem = Tuple.Create(2);
            var collection = new[]
            {
                Tuple.Create(1),
                expectedItem,    // first matching element
                Tuple.Create(2), // last matching element
                Tuple.Create(3)
            };

            var maybe = collection.FirstMaybe(i => i.Item1 == 2);

            Assert.True(maybe.IsSomething());
            Assert.True(Object.ReferenceEquals(expectedItem, maybe.Value));
        }

        [Fact]
        public void SingleMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
        {
            var maybe = Enumerable.Empty<object>().SingleMaybe();

            Assert.True(maybe.IsNothing());
        }

        [Fact]
        public void SingleMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
        {
            var collection = new[] { 1, 2 };
            var itemToSearch = 3;

            var maybe = collection.SingleMaybe(i => i == itemToSearch);

            Assert.True(maybe.IsNothing());
        }

        [Fact]
        public void SingleMaybe_WhenCalledOnNonEmptyEnumerableWithMultipleMatches_ReturnsNothing()
        {
            var collection = new[] { 1, 1, 2 };
            var itemToSearch = 1;

            var maybe = collection.SingleMaybe(i => i == itemToSearch);

            Assert.True(maybe.IsNothing());
        }

        [Fact]
        public void SingleMaybe_WhenCalledOnNonEmptyEnumerableWithSingleMatch_ReturnsSingleMatchingElement()
        {
            var collection = new[] { 1, 2, 3 };
            var itemToSearch = 2;

            var maybe = collection.SingleMaybe(i => i == itemToSearch);

            Assert.True(maybe.IsSomething());
            Assert.Equal(itemToSearch, maybe.Value);
        }

        [Fact]
        public void LastMaybe_WhenCalledOnEmptyEnumerable_ReturnsNothing()
        {
            var maybe = Enumerable.Empty<object>().LastMaybe();

            Assert.True(maybe.IsNothing());
        }

        [Fact]
        public void LastMaybe_WhenCalledOnEnumerableWithNoMatches_ReturnsNothing()
        {
            var collection = new[] { 1, 2 };
            var itemToSearch = 3;

            var maybe = collection.LastMaybe(i => i == itemToSearch);

            Assert.True(maybe.IsNothing());
        }

        [Fact]
        public void LastMaybe_WhenCalledOnEnumerableWithMatches_ReturnsLastMatchingElement()
        {
            var expectedItem = Tuple.Create(2);
            var collection = new[]
            {
                Tuple.Create(1),
                Tuple.Create(2), // first matching element
                expectedItem,    // last matching element
                Tuple.Create(3)
            };

            var maybe = collection.LastMaybe(i => i.Item1 == 2);

            Assert.True(maybe.IsSomething());
            Assert.True(Object.ReferenceEquals(expectedItem, maybe.Value));
        }
    }
}
