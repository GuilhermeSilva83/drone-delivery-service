
using DeliveryService.Domain.Model;

using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.Domain.Service
{
    public interface ITripService
    {
        IEnumerable<Drone> ResolveAssignments();
    }
}