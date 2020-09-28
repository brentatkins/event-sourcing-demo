using System;

namespace EventSourcing
{
    public class AppendOnlyStoreConcurrencyException : Exception
    {
        public int ExpectedVersion { get; }
        public int ActualVersion { get; }

        public AppendOnlyStoreConcurrencyException(int expectedVersion, int actualVersion)
        {
            ExpectedVersion = expectedVersion;
            ActualVersion = actualVersion;
        }
    }
}