using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Linq;

namespace DeliveryService.Domain.Model
{
    public class Drone : IComparable<Drone>
    {
        public string Name { get; set; }

        public int MaximumWeight { get; set; }

        public Drone(string name, int maximumWeight)
        {
            Name = name;
            MaximumWeight = maximumWeight;
            this.Trips = new List<Trip>();
            this.CurrentTrip = new Trip();
            this.Trips.Add(CurrentTrip); 
        }

        public bool IsCurrentTripFull()
        {
            return this.MaximumWeight == this.CurrentTrip.CurrentWeight;
        }

        public int CompareTo([AllowNull] Drone other)
        {
            if (other == null)
                throw new InvalidOperationException("otherCannotBeNull");

            if (this.Name == other.Name)
            {
                throw new InvalidOperationException("Cannot compare with itself, check repeated objects");
            }

            return this.MaximumWeight.CompareTo(other.MaximumWeight);
        }

        public Trip CurrentTrip { get; set; }

        public bool CanFitPackageOnCurrentTrip(params PackageDelivery[] packages)
        {
            return this.MaximumWeight >= CurrentTrip.SumWeight(packages.Sum(s => s.PackageWeight));
        }

        public bool CanFitAnyPackageOnCurrentTrip(params PackageDelivery[] packages)
        {
            return packages.Any(a => !a.IsAssigned & (this.MaximumWeight >= CurrentTrip.SumWeight(a.PackageWeight)));
        }

        /// <summary>
        /// Each trip may consist of multiple locations
        /// </summary>
        public List<Trip> Trips { get; set; }

        

        public void AssignPackageToCurrentTrip(params PackageDelivery[] packages)
        {
            foreach (var item in packages)
            {
                if (item.IsAssigned)
                {
                    throw new InvalidOperationException("AssignedDelivery");
                }

                if (!CanFitPackageOnCurrentTrip(item))
                {
                    throw new InvalidOperationException("CannotAssignDeliveryToCurrentTrip");
                }

                this.CurrentTrip.AssignPackage(item);

                //if (this.IsCurrentTripFull())
                //{
                //    this.CloseCurrentTrip();
                //}
            }
        }

        public void CloseCurrentTrip()
        {
            CurrentTrip.CloseTrip();
            this.CurrentTrip = new Trip();
            this.Trips.Add(CurrentTrip);
        }


        public override string ToString()
        {
            return $"Name: {Name} | MaximumWeight: {MaximumWeight} | ClosedTrips: {Trips.Count(c => c.IsClosed)}";
        }

    }
}