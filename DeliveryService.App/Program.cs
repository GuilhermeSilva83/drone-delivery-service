using DeliveryService.Domain.Model;
using DeliveryService.Domain.Service;
using System;
using System.IO;
using System.Linq;

namespace DeliveryService.App
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Filename required.");
                return;
            }
            
            var parser = new ParserService();
            var fileName = string.Empty;
            

            if (File.Exists(args[0]))
            {
                fileName = args[0];
            }
            else
            {
                fileName = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
            }

            var parsed = parser.ParseText(File.ReadAllLines(args[0]));
            var service = new TripService(parsed) as ITripService;

            foreach(var drone in service.ResolveAssignments())
            {
                Console.WriteLine($"[{drone.Name}]");

                for (int i = 0; i < drone.Trips.Count; i++)
                {
                    var trip = drone.Trips[i];
                    Console.WriteLine($"Trip {i + 1}");
                    var names = trip.Packages.Select(c => c.Name).ToArray();
                    Console.WriteLine(string.Join(',', names));
                }
                Console.WriteLine();
            }

        }
    }
}