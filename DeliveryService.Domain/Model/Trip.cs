using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DeliveryService.Domain.Model
{
    public class Trip
    {
        public Guid TempId { get; } = Guid.NewGuid();

        private List<PackageDelivery> packages;

        public bool IsClosed { get; private set; }

        /// <summary>
        /// Each trip may consist of multiple locations
        /// </summary>
        public IEnumerable<PackageDelivery> Packages { get => packages; }

        /// <summary>
        /// Assigned weight to the current trip.
        /// </summary>
        public int CurrentWeight
        {
            get
            {
                return this.packages.Sum(s => s.PackageWeight);
            }
        }

        public int SumWeight(int addedValue = 0)
        {
            return this.Packages.Sum(s => s.PackageWeight) + addedValue;
        }

        public Trip()
        {
            packages = new List<PackageDelivery>();
        }

        public void AssignPackage(PackageDelivery package)
        {
            this.packages.Add(package);
            package.SetAssigned();
        }

        public void AssignPackages(IEnumerable<PackageDelivery> packages)
        {
            foreach (var item in packages)
            {
                this.packages.Add(item);
                item.SetAssigned();
            }
        }

        public void ClearPackages()
        {
            this.packages.Clear();
        }

        public void CloseTrip()
        {
            this.IsClosed = true;
        }
    }
}
