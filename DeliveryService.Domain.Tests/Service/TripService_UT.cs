using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using DeliveryService.Domain.Service;
using DeliveryService.Domain.Model;
// using System.Linq;

namespace DeliveryService.Domain.Tests
{
    [TestClass]
    public class TripService_UT
    {
        [TestMethod]
        public void ResolveAssignment_01()
        {
            // arrange
            var drones = new List<Drone>();
            drones.Add(new Drone("DroneA", 200));

            var locations = new List<PackageDelivery>();
            locations.Add(new PackageDelivery("LocationA", 200));
            locations.Add(new PackageDelivery("LocationB", 150));
            locations.Add(new PackageDelivery("LocationC", 50));

            var data = new ParseResult(drones, locations);

            var service = new TripService(data);

            // act
            var result = service.ResolveAssignments();

            // assert

            var drone1 = result.Single();
            var trip1 = drone1.Trips.ElementAt(0);
            var trip2 = drone1.Trips.ElementAt(1);

            Assert.AreEqual(2, drone1.Trips.Count(), "has to be 2 trips");
            Assert.AreEqual("LocationA", trip1.Packages.First().Name);
            Assert.AreEqual("LocationB", trip2.Packages.First().Name);
            Assert.AreEqual("LocationC", trip2.Packages.Last().Name);

            Assert.AreEqual(2, drone1.Trips.Count(), "drone1.Trips.Count()");
        }

        [TestMethod]
        public void ResolveAssignment_02()
        {
            // arrange
            var drones = new List<Drone>();
            drones.Add(new Drone("DroneA", 200));
            drones.Add(new Drone("DroneB", 50));

            var locations = new List<PackageDelivery>();
            locations.Add(new PackageDelivery("LocationA", 200));
            locations.Add(new PackageDelivery("LocationB", 50));
            locations.Add(new PackageDelivery("LocationC", 50));
            locations.Add(new PackageDelivery("LocationD", 50));
            locations.Add(new PackageDelivery("LocationE", 50));
            locations.Add(new PackageDelivery("LocationF", 50));

            var data = new ParseResult(drones, locations);

            var service = new TripService(data);

            // act
            var result = service.ResolveAssignments();

            // assert

            var droneA = result.First();
            var droneB = result.Last();
            Assert.AreEqual(2, droneA.Trips.Count(), "droneA");
            Assert.AreEqual(1, droneB.Trips.Count(), "droneB");
            
            
        }

        [TestMethod]
        public void ResolveAssignment_03()
        {
            // arrange
            var drones = new List<Drone>();
            drones.Add(new Drone("DroneA", 200));
            drones.Add(new Drone("DroneB", 100));
            drones.Add(new Drone("DroneC", 50));


            var locations = new List<PackageDelivery>();
            locations.Add(new PackageDelivery("LocationA", 200));
            locations.Add(new PackageDelivery("LocationB", 50));
            locations.Add(new PackageDelivery("LocationC", 50));
            locations.Add(new PackageDelivery("LocationD", 50));
            locations.Add(new PackageDelivery("LocationE", 50));
            locations.Add(new PackageDelivery("LocationF", 50));

            var data = new ParseResult(drones, locations);

            var service = new TripService(data);

            // act
            var result = service.ResolveAssignments();

            // assert

            var droneA = result.ElementAt(0);
            var droneB = result.ElementAt(1);
            var droneC = result.ElementAt(2);
            Assert.AreEqual(2, droneA.Trips.Count(), "droneA");
            Assert.AreEqual(0, droneB.Trips.Count(), "droneB");
            Assert.AreEqual(1, droneC.Trips.Count(), "droneC");


        }

        [TestMethod]
        public void ResolveAssignment_04()
        {
            // arrange
            var drones = new List<Drone>();
            drones.Add(new Drone("DroneA", 200));
            drones.Add(new Drone("DroneB", 100));
            drones.Add(new Drone("DroneC", 100));

            var locations = new List<PackageDelivery>();
            locations.Add(new PackageDelivery("LocationA", 200));
            locations.Add(new PackageDelivery("LocationB", 100));

            var data = new ParseResult(drones, locations);

            var service = new TripService(data);

            // act
            var result = service.ResolveAssignments();

            // assert

            var drone1 = result.ElementAt(0);
            var drone2 = result.ElementAt(1);

            Assert.AreEqual(1, drone1.Trips.Count(), "Drone1");
            Assert.AreEqual(200, drone1.Trips[0].CurrentWeight, "Drone1");

            Assert.AreEqual(1, drone2.Trips.Count(), "Drone2");
            Assert.AreEqual(100, drone2.Trips[0].CurrentWeight, "Drone2");
        }

        [TestMethod("TripService.CanMovePackagesToSmallerDrones")]
        public void CanMovePackagesToSmallerDrones_01()
        {
            // arrange
            var drones = new List<Drone>();
            var drone1 = new Drone("DroneA", 200);
            var drone2 = new Drone("DroneB", 100);

            drones.Add(drone1);
            drones.Add(drone2);

            var locations = new List<PackageDelivery>();
            locations.Add(new PackageDelivery("LocationA", 10));
            locations.Add(new PackageDelivery("LocationB", 20));
            locations.Add(new PackageDelivery("LocationC", 70));

            var data = new ParseResult(drones, locations);
            var service = new TripService(data);
            drone1.AssignPackageToCurrentTrip(locations.ToArray());


            // act
            var act = service.CanMovePackagesToSmallerDrones(drone1);

            Assert.AreEqual(true, act);
        }

        [TestMethod("TripService.CanMovePackagesToSmallerDrones_02")]
        public void CanMovePackagesToSmallerDrones_02()
        {
            // arrange
            var drones = new List<Drone>();
            var drone1 = new Drone("DroneA", 200);
            var drone2 = new Drone("DroneB", 100);

            drones.Add(drone1);
            drones.Add(drone2);

            var locations = new List<PackageDelivery>();
            locations.Add(new PackageDelivery("LocationA", 200));
            locations.Add(new PackageDelivery("LocationB", 100));

            var data = new ParseResult(drones, locations);
            var service = new TripService(data);

            drone1.AssignPackageToCurrentTrip(locations[0]);
            drone2.AssignPackageToCurrentTrip(locations[1]);

            // act
            var act = service.CanMovePackagesToSmallerDrones(drone2);

            Assert.AreEqual(false, act);
        }


        [TestMethod("TripService.MovePackagesToSmallerDrones")]
        public void MovePackagesToSmallerDrones()
        {
            // arrange
            var drones = new List<Drone>();
            var drone1 = new Drone("DroneA", 200);
            var drone2 = new Drone("DroneB", 100);

            drones.Add(drone1);
            drones.Add(drone2);

            var locations = new List<PackageDelivery>();
            locations.Add(new PackageDelivery("LocationA", 10));
            locations.Add(new PackageDelivery("LocationB", 20));
            locations.Add(new PackageDelivery("LocationC", 70));

            var data = new ParseResult(drones, locations);
            var service = new TripService(data);
            drone1.AssignPackageToCurrentTrip(locations.ToArray());

            var drone2TripPreviousState = drone2.CurrentTrip;
            // act
            service.MovePackagesToSmallerDrones(drone1);

            // assert
            Assert.AreEqual(0, drone1.CurrentTrip.Packages.Count());
            Assert.AreEqual(3, drone2TripPreviousState.Packages.Count());
            Assert.AreEqual(0, drone2.CurrentTrip.Packages.Count());
            ;
        }

        [TestMethod]
        public void TestCase01()
        {
            // Arrange
            var drones = new List<Drone>();
            drones.Add(new Drone("DroneA", 200));
            drones.Add(new Drone("DroneB", 250));
            drones.Add(new Drone("DroneC", 100));

            var packages = new List<PackageDelivery>();
            packages.Add(new PackageDelivery("LocationA", 200));
            packages.Add(new PackageDelivery("LocationB", 150));
            packages.Add(new PackageDelivery("LocationC", 50));
            packages.Add(new PackageDelivery("LocationD", 150));
            packages.Add(new PackageDelivery("LocationE", 100));
            packages.Add(new PackageDelivery("LocationF", 200));
            packages.Add(new PackageDelivery("LocationG", 50));
            packages.Add(new PackageDelivery("LocationH", 80));
            packages.Add(new PackageDelivery("LocationI", 70));
            packages.Add(new PackageDelivery("LocationJ", 50));
            packages.Add(new PackageDelivery("LocationK", 30));
            packages.Add(new PackageDelivery("LocationL", 20));
            packages.Add(new PackageDelivery("LocationM", 50));
            packages.Add(new PackageDelivery("LocationN", 30));
            packages.Add(new PackageDelivery("LocationO", 20));
            packages.Add(new PackageDelivery("LocationP", 90));

            var service = new TripService(new ParseResult(drones, packages));

            // act
            var dronesResult = service.ResolveAssignments();

            // assert

            var DroneA = drones.Single(a => a.Name == "DroneA");
            var DroneB = drones.Single(a => a.Name == "DroneB");
            var DroneC = drones.Single(a => a.Name == "DroneC");

            //var trip1 = DroneB.Trips.ElementAt(0);
            //var trip2 = DroneB.Trips.ElementAt(1);

            //Assert.AreEqual(trip1.Packages.First().Name, "LocationA");
            //Assert.AreEqual(trip2.Packages.First().Name, "LocationB");
            //Assert.AreEqual(trip2.Packages.Last().Name, "LocationC");

            Assert.AreEqual(2, DroneA.Trips.Count(), "DroneA");
            Assert.AreEqual(3, DroneB.Trips.Count(), "DroneB");
            Assert.AreEqual(1, DroneC.Trips.Count(), "DroneC");
        }
    }
}


