namespace Functional.Maybe.Tests
{
    using System;
    using Xunit;

    public class MaybeCannotContainNull
    {
        private class User
        {
            public string Name { get; set; }
        }

        [Fact]
        public void WhenSelectingNull_GettingNothing()
        {
            var user = new User { Name = null };

            var maybeUser = user.ToMaybe();

            Assert.Equal(Maybe<string>.Nothing, maybeUser.Select(_ => _.Name));

        }
    }
}
