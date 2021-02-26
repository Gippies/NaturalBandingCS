using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalBandingCS {
    public class NaturalBanding {
        public static List<double> Jenks(List<double> inputList, int numOfBands) {
            inputList.Sort();
            var mean = inputList.Sum() / inputList.Count;
            var sdam = inputList.Sum(input => Math.Pow(input - mean, 2.0));

            var sdcmAllList = new List<double>();
            for (var i = 1; i < inputList.Count; i++) {
                var firstSum = 0.0;
                for (var j = 0; j < i; j++) {
                    firstSum += inputList[j];
                }

                var secondSum = 0.0;
                for (var j = i; j < inputList.Count; j++) {
                    secondSum += inputList[j];
                }

                var firstMean = firstSum / i;
                var secondMean = secondSum / (inputList.Count - i);

                var sdcmAll = 0.0;
                for (var j = 0; j < i; j++) {
                    sdcmAll += Math.Pow(inputList[j] - firstMean, 2);
                }

                for (var j = i; j < inputList.Count; j++) {
                    sdcmAll += Math.Pow(inputList[j] - secondMean, 2);
                }
                sdcmAllList.Add(sdcmAll);
            }

            var gvfList = sdcmAllList.Select(sdcm => (sdam - sdcm) / sdam).ToList();
            return gvfList;
        }

        public static void Main() {
            var inputList = new List<double> {4, 5, 9, 10};
            Jenks(inputList, 2).ForEach(Console.WriteLine);
            Console.WriteLine("Hello World");
        }
    }
}