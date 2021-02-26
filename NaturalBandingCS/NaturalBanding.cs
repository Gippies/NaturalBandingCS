using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalBandingCS {
    public class NaturalBanding {

        private static double GetSdcmAll(List<List<double>> inputListList) {
            var sdcmAll = (from iList in inputListList let mean = iList.Sum() / iList.Count select iList.Sum(value => Math.Pow(value - mean, 2))).Sum();
            return sdcmAll;
        }
        
        public static List<Tuple<double, double>> Jenks(List<double> inputList, int numOfBands) {
            inputList.Sort();
            var mean = inputList.Sum() / inputList.Count;
            var sdam = inputList.Sum(input => Math.Pow(input - mean, 2.0));

            var sdcmAllList = new List<Tuple<double, double>>();
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
                sdcmAllList.Add(new Tuple<double, double>(inputList[i], sdcmAll));
            }

            return sdcmAllList.Select(t => new Tuple<double, double>(t.Item1, (sdam - t.Item2) / sdam)).ToList();
        }

        public static void Main() {
            var inputList = new List<double> {12, 13, 14, 15, 16, 17, 18, 19, 20, 21};
            Jenks(inputList, 5).ForEach(Console.WriteLine);
        }
    }
}