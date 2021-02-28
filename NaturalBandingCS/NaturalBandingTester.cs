using System;
using System.Collections.Generic;
using System.IO;

namespace NaturalBandingCS {
    public class NaturalBandingTester {
        public static void Main() {
            var inputList = new List<double>();
            using (var fileStream = File.OpenRead("../../data/TIV2020_ByCounty.csv")) {
                using (var streamReader = new StreamReader(fileStream)) {
                    string line;
                    while ((line = streamReader.ReadLine()) != null) {
                        inputList.Add(Convert.ToDouble(line));
                    }
                }
            }
            
            var results = NaturalBanding.Jenks(inputList, 5, 0.8);
            var thingToPrint = "";
            foreach (var myList in results) {
                thingToPrint += "[";
                thingToPrint += string.Join(", ", myList);
                thingToPrint += "]";
            }
            Console.WriteLine(thingToPrint);
        }
    }
}