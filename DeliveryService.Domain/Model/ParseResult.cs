using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace DeliveryService.Domain.Model
{
    public class ParseResult
    {
        public ParseResult(List<Drone> drones, List<PackageDelivery> locations)
        {
            Drones = drones;
            Deliveries = locations;

            if(drones.GroupBy(x => x.Name).All(a => a.Count() > 1))
            {
                throw new InvalidOperationException("Check for duplicate drones");
            }

            if (locations.GroupBy(x => x.Name).All(a => a.Count() > 1))
            {
                throw new InvalidOperationException("Check for duplicate locations");
            }
        }

        public List<Drone> Drones { get; }
        public List<PackageDelivery> Deliveries { get; }

    }
}
