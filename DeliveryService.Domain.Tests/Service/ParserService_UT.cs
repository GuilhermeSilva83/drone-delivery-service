using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using DeliveryService.Domain.Service;
// using System.Linq;

namespace DeliveryService.Domain.Tests
{
    [TestClass]
    public class ParserService_UT
    {
        [TestMethod]
        public void ParseTextTest()
        {
            // arrange
            var text = new string[]
            {
                "[DroneA], [300], [DroneB], [350], [DroneC], [200]", "[LocationA], [200]",
                "[LocationB], [150]",
                "[LocationC], [50]",
                "[LocationD], [150]",
                "[LocationE], [100] [LocationF],",
                "[200]",
                "[LocationG], [50] [LocationH],",
                "[80]",
                "[LocationI], [70]",
                "[LocationJ], [50]"
            };

            var service = new ParserService();
            // act

            var result = service.ParseText(text);

            // assert
            Assert.AreEqual(result.Drones.First(a => a.Name == "DroneA").MaximumWeight, 300);
            Assert.AreEqual(result.Drones.First(a => a.Name == "DroneB").MaximumWeight, 350);
            Assert.AreEqual(result.Drones.First(a => a.Name == "DroneC").MaximumWeight, 200);

            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationA").PackageWeight, 200);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationB").PackageWeight, 150);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationC").PackageWeight, 50);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationD").PackageWeight, 150);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationE").PackageWeight, 100);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationF").PackageWeight, 200);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationG").PackageWeight, 50);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationH").PackageWeight, 80);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationI").PackageWeight, 70);
            Assert.AreEqual(result.Deliveries.First(a => a.Name == "LocationJ").PackageWeight, 50);

        }
    }
}
