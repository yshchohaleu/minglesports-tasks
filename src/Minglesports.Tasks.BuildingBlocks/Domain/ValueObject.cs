using System;

namespace Minglesports.Tasks.BuildingBlocks.Domain
{
    public abstract class ValueObject : EquatableObject
    {
    }

    public abstract class ValueObject<T> : ValueObject, IEquatable<ValueObject<T>>
        where T : ValueObject
    {
        public bool Equals(ValueObject<T> other)
        {
            return base.Equals(other);
        }

    }
}