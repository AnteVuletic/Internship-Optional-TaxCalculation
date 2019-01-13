using System;
using System.Collections.Generic;

namespace TaxesCalculationApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var AllRangesAdded = new List<RangeWithVat>();
            Console.WriteLine("Enter value without VAT included:");
            var gainWithoutVatIncluded = decimal.Parse(Console.ReadLine());
            var gainWithVatIncluded = 0M;
            var isFirstIteration = true;
            var isStop = false;
            var lowerLimit = 0M;
            var upperLimit = 0M;
            var areLimitsSetCorrectly = true;
            do
            {
                if (isFirstIteration)
                {
                    Console.WriteLine("Lower limit");
                    lowerLimit = decimal.Parse(Console.ReadLine());
                }
                else
                {
                    lowerLimit = upperLimit;
                }
                Console.WriteLine("Upper limit");
                upperLimit = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Charge");
                var charge = decimal.Parse(Console.ReadLine());
                if (lowerLimit < 0)
                    areLimitsSetCorrectly = false;
                if (lowerLimit > upperLimit)
                    areLimitsSetCorrectly = false;
                if (!isFirstIteration)
                {
                    if (AllRangesAdded[AllRangesAdded.Count - 1].UpperLimitVat > upperLimit)
                    {
                        areLimitsSetCorrectly = false;
                    }
                }
                AllRangesAdded.Add(new RangeWithVat(lowerLimit, upperLimit, charge));
                isFirstIteration = false;
                Console.WriteLine("Type 1 to add new value, anything else to stop");
                isStop = Console.ReadLine() != "1";
            } while (!isStop);
            //AllRangesAdded.Add(new RangeWithVat(0, 10000, 10));
            //AllRangesAdded.Add(new RangeWithVat(10000, 25000, 15));
            //AllRangesAdded.Add(new RangeWithVat(25000, 1000000, 25));

            if (AllRangesAdded[AllRangesAdded.Count - 1].UpperLimitVat < 1000000)
                areLimitsSetCorrectly = false;
            gainWithVatIncluded =
                RangeWithVat.CalculateWithVat(AllRangesAdded, gainWithoutVatIncluded);
            Console.WriteLine(!areLimitsSetCorrectly
                ? "Limits were not set up correctly"
                : $"Value without VAT included {gainWithoutVatIncluded} \n value with VAT included {gainWithVatIncluded}");
        }

        //Examples
        // gainWithoutvatIncluded = 5000;
        // Do 10000, porez = 10%
        // Od 10000 do 25000 porez = 15%
        // Od 25000 do beskonacno porez =  25%
        // gainWithVatIncluded = 4500
        //
        // gainWithoutvatIncluded = 15000;
        // Do 10000, porez = 10%
        // Od 10000 do 25000 porez = 15%
        // Od 25000 do beskonacno porez =  25%
        // gainWithVatIncluded = 13250
        //
        // gainWithoutvatIncluded = 40000;
        // Do 10000, porez = 10%
        // Od 10000 do 25000 porez = 15%
        // Od 25000 do beskonacno porez =  25%
        // gainWithVatIncluded 33000
    }

    public class RangeWithVat
    {
        public decimal LowerLimitVat { get; set; }
        public decimal UpperLimitVat { get; set; }
        public decimal Vat { get; set; }

        public RangeWithVat(decimal lowerLimitVat, decimal upperLimitVat, decimal vat)
        {
            LowerLimitVat = lowerLimitVat;
            UpperLimitVat = upperLimitVat;
            Vat = vat;
        }

        public static decimal CalculateWithVat(List<RangeWithVat> argRangeWithVats,decimal argWithoutVat)
        {
            return _calculateWithVat(argRangeWithVats,
                argRangeWithVats.FindIndex(category => category.UpperLimitVat >= argWithoutVat && category.LowerLimitVat < argWithoutVat), argWithoutVat);
        }

        private static decimal _calculateWithVat(List<RangeWithVat> argRangeWithVats, int maxIndex, decimal argWithoutVat)
        {
            if (argRangeWithVats.Count == 0) return 0;
            if (maxIndex == -1)
            {
                return 0;
            }
            return _calculateWithVat(argRangeWithVats, maxIndex - 1, argRangeWithVats[maxIndex].LowerLimitVat) +
                   (argWithoutVat - argRangeWithVats[maxIndex].LowerLimitVat ) * (1 - argRangeWithVats[maxIndex].Vat/100);
        }
    }


}
