using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public readonly struct DebitId : IEquatable<DebitId>
    {
        public Guid Id { get; }

        public DebitId(Guid id)
        {
            Id = id;
        }

        public bool Equals(DebitId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is DebitId o && Equals(o);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public static bool operator ==(DebitId left, DebitId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DebitId left, DebitId right)
        {
            return !left.Equals(right);
        }
    }

    


}
