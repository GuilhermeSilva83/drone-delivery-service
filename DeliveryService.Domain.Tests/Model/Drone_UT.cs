using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using DeliveryService.Domain.Model;
// using System.Linq;

namespace DeliveryService.App.Tests.Domain.Model
{
    [TestClass]
    public class Drone_UT
    {
        [TestMethod]
        public void IsCurrentTripFull_Start_False()
        {
            // arrange
            var drone = new Drone("1", 200);

            // act
            var act = drone.IsCurrentTripFull();

            // assert
            Assert.AreEqual(act, false);
            Assert.AreEqual(drone.MaximumWeight, 200);
            Assert.AreEqual(drone.CurrentTrip.CurrentWeight, 0, "has to be 0 (zero)");
        }

        [TestMethod]
        public void IsCurrentTripFull_True()
        {
            // arrange
            var drone = new Drone("1", 150);
            drone.AssignPackageToCurrentTrip(new PackageDelivery("package1", 50));
            drone.AssignPackageToCurrentTrip(new PackageDelivery("package2", 100));

            // act
            var act = drone.IsCurrentTripFull();

            // assert
            Assert.AreEqual(act, true);
        }

        [TestMethod]
        public void CompareTo()
        {
            // arrange
            var drone1 = new Drone("1", 150);
            var drone2 = new Drone("2", 100);

            // act/assert
            Assert.AreEqual(1, drone1.CompareTo(drone2), "has to be 1");
        }

        [TestMethod]
        public void CanFitPackageOnCurrentTrip_True_C1()
        {
            // arrange
            var drone = new Drone("1", 150);
            var pack1 = new PackageDelivery("package1", 150);

            // act
            var act = drone.CanFitPackageOnCurrentTrip(pack1);

            // assert
            Assert.AreEqual(act, true);
            Assert.AreEqual(0, drone.CurrentTrip.CurrentWeight, "has to be 0");
        }

        [TestMethod]
        public void CanFitPackagesOnCurrentTrip_True()
        {
            // arrange
            var drone = new Drone("1", 450);
            var pack1 = new PackageDelivery("package1", 300);
            var pack2 = new PackageDelivery("package2", 100);
            var pack3 = new PackageDelivery("package3",  50);
            drone.AssignPackageToCurrentTrip(pack1);

            // act
            var act = drone.CanFitPackageOnCurrentTrip(new PackageDelivery[] { pack2, pack3 });
            drone.AssignPackageToCurrentTrip(pack2, pack3);

            // assert
            Assert.AreEqual(act, true);
            Assert.AreEqual(450, drone.CurrentTrip.CurrentWeight, "has to be 450");
            Assert.AreEqual(1, drone.Trips.Count, "has to be 1");
        }

        [TestMethod]
        public void AssignPackageToCurrentTrip()
        {
            // arrange
            var drone = new Drone("1", 450);
            var pack1 = new PackageDelivery("package1", 300);
            var pack2 = new PackageDelivery("package2", 100);
            var pack3 = new PackageDelivery("package3", 50);
            drone.AssignPackageToCurrentTrip(pack1);

            Assert.AreEqual(300, drone.CurrentTrip.CurrentWeight, "has to be 300");

            // act
            drone.AssignPackageToCurrentTrip(pack2, pack3);

            // assert


            Assert.AreEqual(true, pack1.IsAssigned, "pack1");
            Assert.AreEqual(true, pack2.IsAssigned, "pack2");
            Assert.AreEqual(true, pack3.IsAssigned, "pack3");
            Assert.AreEqual(450, drone.CurrentTrip.CurrentWeight, "drone.CurrentTrip.CurrentWeight");
            Assert.AreEqual(1, drone.Trips.Count, "drone.Trips.Count");
        }

        [TestMethod]
        public void CanFitPackageOnCurrentTrip_False()
        {
            // arrange
            var drone1 = new Drone("1", 200);
            var pack1 = new PackageDelivery("package1", 200);
            var pack2 = new PackageDelivery("package1", 200);

            drone1.AssignPackageToCurrentTrip(pack1);

            // act
            var act = drone1.CanFitPackageOnCurrentTrip(pack2);

            // assert
            Assert.AreEqual(act, false);
        }
    }
}
