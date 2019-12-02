using System;
using System.Collections.Generic;

namespace Day01
{
    public class Fuel
    {
        public static decimal CalculateRocketShipFuel(List<int> input)
        {
            var total = 0m;
            foreach (var module in input)
            {
                var fuelRequired = CalculateFuelRecursively(module);
                total += fuelRequired;
            }

            return total;
        }
        public static decimal CalculateFuelRecursively(int fuelWeight)
        {
            var fuelForFuel = CalculateFuelRequirement(fuelWeight);
            if (fuelForFuel <= 0)
                return 0;
            else return fuelForFuel + CalculateFuelRecursively((int)fuelForFuel);
        }

        public static decimal CalculateFuelRequirement(int weight)
        {
            var fuelRequired = Math.Round((decimal)(weight / 3), MidpointRounding.ToZero) - 2;
            return fuelRequired;
        }
    }
}