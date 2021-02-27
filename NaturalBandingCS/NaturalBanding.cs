using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NaturalBandingCS {
    public class NaturalBanding {

        public static List<Tuple<List<List<double>>, double>> Jenks(List<double> inputList, int numOfBands) {
            inputList.Sort();
            var mean = inputList.Sum() / inputList.Count;
            var sdam = inputList.Sum(input => Math.Pow(input - mean, 2.0));

            var sdcmAllList = new List<Tuple<List<List<double>>, double>>();
            var bandIndexList = GetInitialBandIndexList(inputList.Count, numOfBands);
            var hasIncremented = true;

            while (hasIncremented) {
                var dividedList = GetDividedList(inputList, bandIndexList);
                var sdcmAll = GetSdcmAll(dividedList);
                sdcmAllList.Add(new Tuple<List<List<double>>, double>(dividedList, sdcmAll));
                hasIncremented = IncrementBandIndexList(bandIndexList, bandIndexList.Count);
            }
            
            return sdcmAllList.Select(t => new Tuple<List<List<double>>, double>(t.Item1, (sdam - t.Item2) / sdam)).ToList();
        }

        private static List<int> GetInitialBandIndexList(int inputCount, int numOfBands) {
            var resultList = new List<int>();
            for (var i = 0; i < numOfBands; i++) {
                resultList.Add(i);
            }
            resultList.Add(inputCount);
            return resultList;
        }

        private static bool IncrementBandIndexList(List<int> bandIndexList, int currentIndex) {
            if (currentIndex - 2 == 0) {
                return false;
            }
            if (bandIndexList[currentIndex - 2] + 1 < bandIndexList[currentIndex - 1]) {
                bandIndexList[currentIndex - 2]++;
                if (bandIndexList[currentIndex - 2] + 1 < bandIndexList[bandIndexList.Count - 1] && currentIndex != bandIndexList.Count) {
                    bandIndexList[currentIndex - 1] = bandIndexList[currentIndex - 2] + 1;
                }
                return true;
            }
            if (currentIndex - 2 > 0) {
                currentIndex--;
                return IncrementBandIndexList(bandIndexList, currentIndex);
            }
            return false;
        }

        private static List<List<double>> GetDividedList(List<double> inputList, List<int> bandIndexList) {
            bandIndexList.Sort();
            var resultList = new List<List<double>>();
            for (var i = 0; i < bandIndexList.Count - 1; i++) {
                resultList.Add(inputList.GetRange(bandIndexList[i], bandIndexList[i + 1] - bandIndexList[i]));
            }
            return resultList;
        }
        
        private static double GetSdcmAll(List<List<double>> inputListList) {
            var sdcmAll = (from iList in inputListList let mean = iList.Sum() / iList.Count select iList.Sum(value => Math.Pow(value - mean, 2))).Sum();
            return sdcmAll;
        }

        public static void Main() {
            var inputList = new List<double> {12, 13, 14, 15, 16, 17, 18, 19, 20, 21};
            var results = Jenks(inputList, 5);
            foreach (var myTuple in results) {
                var thingToPrint = "";
                foreach (var myList in myTuple.Item1) {
                    thingToPrint += "[";
                    thingToPrint += string.Join(", ", myList);
                    thingToPrint += "]";
                }
                thingToPrint += " " + myTuple.Item2.ToString(Thread.CurrentThread.CurrentCulture);
                Console.WriteLine(thingToPrint);
            }
        }
    }
}