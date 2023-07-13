using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DeliveryService.Domain.Model
{
    public class PackageDelivery : IComparable<PackageDelivery>
    {
        public PackageDelivery(string name, int packageWeight)
        {
            Name = name;
            PackageWeight = packageWeight;
        }

        public string Name { get; private set; }

        public int PackageWeight { get; private set; }

        public bool IsAssigned { get; private set; }

        public int CompareTo([AllowNull] PackageDelivery other)
        {
            if (other == null)
                throw new InvalidOperationException("otherCannotBeNull");

            if (this.Name == other.Name)
            {
                throw new InvalidOperationException("Cannot compare with itself, check repeated objects");
            }

            return this.PackageWeight.CompareTo(other.PackageWeight);
        }

        public void SetAssigned()
        {
            this.IsAssigned = true;
        }
    }
}