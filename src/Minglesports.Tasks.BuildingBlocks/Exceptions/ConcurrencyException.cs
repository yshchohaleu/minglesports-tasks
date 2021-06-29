using System;

namespace Minglesports.Tasks.BuildingBlocks.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}