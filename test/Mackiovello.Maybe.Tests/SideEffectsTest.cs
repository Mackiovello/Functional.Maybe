
namespace Mackiovello.Maybe.Tests
{
    using System;
    using Xunit;

    public class SideEffectsTest
    {
        [Fact]
        public void DoOnNothing_DoesNothing()
        {
            var target = "unchanged";
            Maybe<string>.Nothing.Do(_ => target = "changed");
            Assert.Equal("unchanged", target);
        }
        [Fact]
        public void DoOnSomething_DoesSomething()
        {
            var target = "unchanged";
            "changed".ToMaybe().Do(_ => target = _);
            Assert.Equal("changed", target);
        }

        [Fact]
        public void MatchOnNothing_MatchesNothing()
        {
            var target1 = "unchanged";
            var target2 = "unchanged";
            Maybe<string>.Nothing.Match(_ => target1 = "changed", () => target2 = "changed");
            Assert.Equal("unchanged", target1);
            Assert.Equal("changed", target2);
        }
        [Fact]
        public void MatchOnSomething_MatchesSomething()
        {
            var target1 = "unchanged";
            var target2 = "unchanged";
            "κατι".ToMaybe().Match(_ => target1 = "changed", () => target2 = "changed");
            Assert.Equal("changed", target1);
            Assert.Equal("unchanged", target2);
        }
    }
}
