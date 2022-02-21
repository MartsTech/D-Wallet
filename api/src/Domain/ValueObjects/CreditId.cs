using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public readonly struct CreditId : IEquatable<CreditId>
    {
        public Guid Id { get; }

        public CreditId(Guid id)
        {
            Id = id;
        }

        public bool Equals(CreditId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is CreditId o && Equals(o);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        public static bool operator ==(CreditId left, CreditId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CreditId left, CreditId right)
        {
            return !left.Equals(right);
        }
    }
