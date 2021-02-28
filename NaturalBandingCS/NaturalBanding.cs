using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalBandingCS {
    public static class NaturalBanding {

        public static List<double> Jenks(List<double> inputList, int numOfBands, double targetGvf) {
            inputList.Sort();
            inputList.Reverse();
            var mean = inputList.Sum() / inputList.Count;
            var sdam = inputList.Sum(input => Math.Pow(input - mean, 2.0));

            var bandIndexList = GetInitialBandIndexList(inputList.Count, numOfBands);
            var hasIncremented = true;
            
            var largestGvf = 0.0;
            List<double> currentResult = null;

            while (hasIncremented && largestGvf < targetGvf) {
                var sdcmAll = GetSdcmAll(inputList, bandIndexList);
                var gvf = (sdam - sdcmAll) / sdam;
                if (gvf > largestGvf) {
                    largestGvf = gvf;
                    currentResult = GetResultList(inputList, bandIndexList);
                }
                hasIncremented = IncrementBandIndexList(bandIndexList);
            }

            return currentResult;
        }

        private static List<double> GetResultList(List<double> inputList, List<int> bandIndexList) {
            var resultList = new List<double>();
            for (var i = 0; i < bandIndexList.Count - 1; i++) {
                resultList.Add(inputList[bandIndexList[i]]);
            }

            return resultList;
        }

        private static List<int> GetInitialBandIndexList(int inputCount, int numOfBands) {
            var resultList = new List<int>();
            for (var i = 0; i < numOfBands; i++) {
                resultList.Add(i);
            }
            resultList.Add(inputCount);
            return resultList;
        }

        private static bool IncrementBandIndexList(List<int> bandIndexList) {
            var currentIndex = bandIndexList.Count;
            while (currentIndex - 2 > 0) {
                if (bandIndexList[currentIndex - 2] + 1 < bandIndexList[currentIndex - 1]) {
                    bandIndexList[currentIndex - 2]++;
                    if (bandIndexList[currentIndex - 2] + 1 < bandIndexList[bandIndexList.Count - 1] && currentIndex != bandIndexList.Count) {
                        bandIndexList[currentIndex - 1] = bandIndexList[currentIndex - 2] + 1;
                    }
                    return true;
                }
                currentIndex--;
            }
            return false;
        }

        private static double GetSdcmAll(List<double> inputList, List<int> bandIndexList) {
            var sdcmAll = 0.0;
            for (var i = 0; i < bandIndexList.Count - 1; i++) {
                var meanSum = 0.0;
                var meanCount = 0;
                for (var j = bandIndexList[i]; j < bandIndexList[i + 1]; j++) {
                    meanSum += inputList[j];
                    meanCount++;
                }
                var mean = meanSum / meanCount;
                for (var j = bandIndexList[i]; j < bandIndexList[i + 1]; j++) {
                    sdcmAll += Math.Pow(inputList[j] - mean, 2.0);
                }
            }
            return sdcmAll;
        }
    }
}