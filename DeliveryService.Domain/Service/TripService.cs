using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using DeliveryService.Domain.Model;

namespace DeliveryService.Domain.Service
{
    public class TripService : ITripService
    {
        private ParseResult data;

        public TripService(ParseResult data)
        {
            this.data = data;
        }

        public IEnumerable<Drone> ResolveAssignments()
        {
            while (HasUnassignedPackage())
            {
                foreach (var drone in data.Drones.OrderByDescending(o => o))
                {
                    foreach (var package in ListUnassignedPackages())
                    {
                        if (drone.CanFitPackageOnCurrentTrip(package))
                        {
                            drone.AssignPackageToCurrentTrip(package);


                            if (drone.IsCurrentTripFull() || ListUnassignedPackages().Count() == 0) // or it's last
                            {
                                if (this.CanMovePackagesToSmallerDrones(drone))
                                {
                                    this.MovePackagesToSmallerDrones(drone);
                                }
                                else
                                {
                                    drone.CloseCurrentTrip();
                                }
                            }
                            else
                            {
                                // Can assign more packages
                                // try to assign to a smaller drone
                            }
                        }
                        else
                        {
                            if (drone.CanFitAnyPackageOnCurrentTrip(ListUnassignedPackages().ToArray()))
                            {
                            }
                            else
                            {
                                drone.CloseCurrentTrip();
                                break;
                            }
                        }
                    }
                }
            }

            DeleteUnassignedTrips();
            return data.Drones;
        }

        public IEnumerable<PackageDelivery> ListUnassignedPackages()
        {
            return this.data.Deliveries.Where(w => !w.IsAssigned).OrderByDescending(a => a);
        }

        public bool HasUnassignedPackage()
        {
            return data.Deliveries.Any(a => a.IsAssigned == false);
        }

        public bool CanMovePackagesToSmallerDrones(Drone drone)
        {
            if (drone.CurrentTrip.Packages.Count() == 0)
            {
                var msg = $"Current trip has no package (drone: {drone.Name})";
                throw new InvalidOperationException(msg);
            }

            return this.data.Drones
                        .Any(c => c.Name != drone.Name && c.CanFitPackageOnCurrentTrip(drone.CurrentTrip.Packages.ToArray()));
        }

        public void DeleteUnassignedTrips()
        {
            foreach (var drone in data.Drones)
            {
                drone.Trips
                    .RemoveAll(r => r.IsClosed == false || r.Packages.Count() == 0);
            }
        }

        public void MovePackagesToSmallerDrones(Drone drone)
        {
            if (data.Drones.Count == 1)
            {
                throw new InvalidOperationException("Cannot move packages. Single drone registered.");
            }

            var drones = this.data.Drones
                        .Where(c => c.Name != drone.Name)
                        .Where(c => c.CanFitPackageOnCurrentTrip(drone.CurrentTrip.Packages.ToArray()))
                        .OrderBy(o => o);

            foreach (var item in drones)
            {
                if (item.CanFitPackageOnCurrentTrip(drone.CurrentTrip.Packages.ToArray()))
                {
                    item.CurrentTrip.AssignPackages(drone.CurrentTrip.Packages);
                    if (item.IsCurrentTripFull())
                    {
                        item.CloseCurrentTrip();
                    }
                    drone.CurrentTrip.ClearPackages();
                }
            }
        }
    }
}
