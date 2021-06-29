using System;
using System.Collections.Generic;
using System.Reflection;

namespace Minglesports.Tasks.BuildingBlocks.Domain
{
    public class SingleValueObject<T> : ValueObject, IComparable, IEquatable<T>
        where T : IComparable
    {
        private static readonly Type Type = typeof(T);
        private static readonly TypeInfo TypeInfo = typeof(T).GetTypeInfo();

        public T Value { get; private set; }

        protected SingleValueObject(T value)
        {
            if (TypeInfo.IsEnum && !Enum.IsDefined(Type, value))
            {
                throw new ArgumentException($"The value '{value}' isn't defined in enum '{Type}'");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is not SingleValueObject<T> other)
            {
                throw new ArgumentException($"Cannot compare '{GetType()}' and '{obj.GetType()}'");
            }

            return Value.CompareTo(other.Value);
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        // Non-readonly due to the serialization mechanism
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Value);
        }

        public override bool Equals(object obj) =>
            obj switch
            {
                SingleValueObject<T> singleValueObject => singleValueObject.Value != null
                    ? singleValueObject.Value.Equals(Value)
                    : Value == null,
                T value => Value != null && Value.Equals(value),
                _ => false
            };

        public override string ToString() => Value is null ? string.Empty : Value.ToString()!;

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        public static bool operator ==(SingleValueObject<T> left, T right)
        {
            if (left != null)
            {
                return Equals(left.Value, right);
            }
            return Equals(null, right);
        }

        public static bool operator !=(SingleValueObject<T> left, T right)
        {
            if (left != null)
            {
                return !Equals(left.Value, right);
            }

            return !Equals(null, right);
        }

        public static implicit operator T(SingleValueObject<T> singleValueObject)
            => singleValueObject == null ? default : singleValueObject.Value;

        public static implicit operator SingleValueObject<T>(T item)
            => new(item);

        public object GetValue()
        {
            return Value;
        }
    }
}