using System;

namespace Minglesports.Tasks.BuildingBlocks
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }
    }

    public class TimeProvider : ITimeProvider
    {
        private readonly DateTime? _presetDateTime;

        private TimeProvider(DateTime? presetDateTime)
        {
            _presetDateTime = presetDateTime;
        }

        public TimeProvider()
        {
        }

        public DateTime UtcNow => _presetDateTime ?? DateTime.UtcNow;
    }
}