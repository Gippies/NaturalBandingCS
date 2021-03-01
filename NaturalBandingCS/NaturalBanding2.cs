﻿using System;
using System.Collections.Generic;

namespace NaturalBandingCS {
    public static class NaturalBanding2 {
        public static List<double> JenksBreakValues(List<double> values, int nbClass) {
            var k = nbClass;
            var breaks = new List<double>();
            var arrayLength = values.Count;

            values.Sort();

            var mat1 = new List<List<double>>();
            for (var i = 0; i < arrayLength; i++) {
                var row = new List<double>();
                for (var j = 0; j < k; j++) {
                    row.Add(1.0);
                }
                mat1.Add(row);
            }

            var mat2 = new List<List<double>>();
            for (var i = 0; i < arrayLength; i++) {
                var row = new List<double>();
                for (var j = 0; j < k; j++) {
                    row.Add(double.MaxValue);
                }
                mat2.Add(row);
            }

            double v = 0;
            for (var l = 2; l <= arrayLength; l++) {
                var s1 = 0.0;
                var s2 = 0.0;
                var w = 0.0;
                for (var m = 1; m <= l; m++) {
                    var i3 = l - m + 1;
                    var val = values[i3 - 1];
                    s2 += Math.Pow(val, 2.0);
                    s1 += val;
                    w++;
                    v = s2 - Math.Pow(s1, 2.0) / w;
                    var i4 = i3 - 1;

                    if (i4 != 0) {
                        for (var j = 2; j <= k; j++) {
                            if (mat2[l - 1][j - 1] >= v + mat2[i4 - 1][j - 2]) {
                                mat1[l - 1][j - 1] = i3;
                                mat2[l - 1][j - 1] = v + mat2[i4 - 1][j - 2];
                            }
                        }
                    }
                }

                mat1[l - 1][0] = 1;
                mat2[l - 1][0] = v;
            }

            var kclass = new double[k];
            k = arrayLength;
            for (var j = nbClass; j > 1; j--) {
                kclass[j - 2] = k = (int) mat1[k - 1][j - 1] - 1;
            }

            breaks.Add(values[0]);
            for (var i = 1; i < nbClass; i++) {
                breaks.Add(values[(int) kclass[i - 1] - 1]);
            }
            breaks.Add(values[arrayLength - 1]);
            return breaks;
        }
    }
}