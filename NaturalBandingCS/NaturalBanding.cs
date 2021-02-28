﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NaturalBandingCS {
    public class NaturalBanding {

        public static List<List<double>> Jenks(List<double> inputList, int numOfBands) {
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
            
            var gvfList = sdcmAllList.Select(t => new Tuple<List<List<double>>, double>(t.Item1, (sdam - t.Item2) / sdam)).ToList();
            var largestGvf = 0.0;
            List<List<double>> currentResult = null;
            foreach (var (item1, item2) in gvfList.Where(gvf => largestGvf < gvf.Item2)) {
                currentResult = item1;
                largestGvf = item2;
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
            var inputList = new List<double>();
            using (var fileStream = File.OpenRead("../../data/TIV2020_ByCounty.csv")) {
                using (var streamReader = new StreamReader(fileStream)) {
                    string line;
                    while ((line = streamReader.ReadLine()) != null) {
                        inputList.Add(Convert.ToDouble(line));
                    }
                }
            }
            
            var results = Jenks(inputList, 5);
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