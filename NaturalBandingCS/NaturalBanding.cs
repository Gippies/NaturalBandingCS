using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalBandingCS {
    public static class NaturalBanding {

        public static List<List<double>> Jenks(List<double> inputList, int numOfBands, double targetGvf) {
            inputList.Sort();
            inputList.Reverse();
            var mean = inputList.Sum() / inputList.Count;
            var sdam = inputList.Sum(input => Math.Pow(input - mean, 2.0));

            var bandIndexList = GetInitialBandIndexList(inputList.Count, numOfBands);
            var hasIncremented = true;
            
            var largestGvf = 0.0;
            List<List<double>> currentResult = null;

            while (hasIncremented && largestGvf < targetGvf) {
                var dividedList = GetDividedList(inputList, bandIndexList);
                var sdcmAll = GetSdcmAll(dividedList);
                var gvf = (sdam - sdcmAll) / sdam;
                if (gvf > largestGvf) {
                    largestGvf = gvf;
                    currentResult = dividedList;
                }
                hasIncremented = IncrementBandIndexList(bandIndexList, bandIndexList.Count);
            }

            return currentResult;
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
            while (true) {
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
                    continue;
                }

                return false;
            }
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
    }
}