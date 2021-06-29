using System.Collections.Generic;
using System.Linq;

namespace Minglesports.Tasks.BuildingBlocks.Domain
{
    public abstract class EquatableObject
    {
        private static bool EqualOperator(EquatableObject left, EquatableObject right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left is null || left.Equals(right);
        }

        public static bool operator ==(EquatableObject a, EquatableObject b)
        {
            return EqualOperator(a, b);
        }

        public static bool operator !=(EquatableObject a, EquatableObject b) => !(a == b);
        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (EquatableObject)obj;
            using IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator()!;
            using IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current is null ^ otherValues.Current is null)
                {
                    return false;
                }

                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            var equal = !thisValues.MoveNext() && !otherValues.MoveNext();

            return equal;
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}