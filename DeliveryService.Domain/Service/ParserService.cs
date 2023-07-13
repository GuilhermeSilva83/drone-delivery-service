
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DeliveryService.Domain.Model;

namespace DeliveryService.Domain.Service
{
    public class ParserService
    {

        public ParseResult ParseText(string[] text)
        {
            var drones = new List<Drone>();
            var locations = new List<PackageDelivery>();
            var previousLine = string.Empty;

            for (int i = 0; i < text.Length; i++)
            {
                var line = previousLine + new StringBuilder(text[i])
                                                    .Replace(" ", "")
                                                    .Replace("][", ",")
                                                    .Replace("[", "")
                                                    .Replace("]", "")
                                                    .Replace(" ", "")
                                                    .ToString();

                var splitLines = line.Split(',');

                // if not even, skip current line
                if (splitLines.Length % 2 != 0 || splitLines.Last().Equals(""))
                {
                    previousLine = line;
                    continue;
                }
                else
                {
                    previousLine = string.Empty;
                }

                var count = 1;
                var key = string.Empty;

                for (int j = 0; j < splitLines.Length; j++)
                {
                    KeyValuePair<string, int> kvp;

                    // if count is even
                    if (count % 2 == 1)
                    {
                        key = splitLines[j];
                    }
                    else
                    {
                        kvp = new KeyValuePair<string, int>(key, int.Parse(splitLines[j]));

                        if (i == 0)
                        {
                            drones.Add(new Drone(kvp.Key, kvp.Value));
                        }
                        else
                        {
                            locations.Add(new PackageDelivery(kvp.Key, kvp.Value));
                        }
                    }

                    count++;
                }
            }
            return new ParseResult(drones, locations);
        }
    }
}
