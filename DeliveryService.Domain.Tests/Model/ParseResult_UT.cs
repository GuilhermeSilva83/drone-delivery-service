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
    public class ParseResult_UT
    {
        [TestMethod]
        public void ParseResult_Defaults()
        {
            // arrange
            var drone1 = new Drone("1", 400);
            var drone2 = new Drone("2", 200);
            var drone3 = new Drone("3", 100);

            var pack1 = new PackageDelivery("p1", 200);
            var pack2 = new PackageDelivery("p2", 200);
            var pack3 = new PackageDelivery("p3", 200);
            var pack4 = new PackageDelivery("p4", 200);

            var result = new ParseResult(
                new List<Drone>()
                {
                    drone1,
                    drone2,
                    drone3
                },
                new List<PackageDelivery>
                {
                    pack1,
                    pack2,
                    pack3,
                    pack4
                });

            // act/assert
            Assert.AreEqual(result.Drones.Count, 3);
            Assert.AreEqual(result.Deliveries.Count, 4);
        }
    }
}