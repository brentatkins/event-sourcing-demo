using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace EventSourcing.Tests.TestHelpers
{
    public class MatchTypeEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(
            IEquivalencyValidationContext context,
            IEquivalencyAssertionOptions config)
        {
            Type subjectType = config.GetExpectationType(context);

            return !IsCollection(subjectType) && !IsAnonymousType(subjectType);
        }

        public bool Handle(
            IEquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            if (context.Subject != null && context.Expectation != null)
            {
                context.Subject.GetType().Should().Be(context.Expectation.GetType());
            }

            return false;
        }

        private static bool IsCollection(
            Type type)
        {
            return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static bool IsAnonymousType(
            Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            bool nameContainsAnonymousType = type.FullName != null && type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }
    }
}