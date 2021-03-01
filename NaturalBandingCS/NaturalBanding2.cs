using System;
using System.Collections.Generic;

namespace NaturalBandingCS {
    public class NaturalBanding2 {
        public static List<double> JenksBreakValues(List<double> values, int nbClass) {
            int i, j, l, k = nbClass;
            var mat1 = new List<List<double>>();
            var mat2 = new List<List<double>>();
            List<double> row;
            var breaks = new List<double>();
            var lengthArray = values.Count;

            values.Sort();

            for (i = 0; i < lengthArray; i++) {
                row = new List<double>();
                for (j = 0; j < k; j++) {
                    row.Add(1.0);
                }
                mat1.Add(row);
            }

            for (i = 0; i < lengthArray; i++) {
                row = new List<double>();
                for (j = 0; j < k; j++) {
                    row.Add(Double.MaxValue);
                }
                mat2.Add(row);
            }

            double v = 0;
            for (l = 2; l <= lengthArray; l++) {
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
                        for (j = 2; j <= k; j++) {
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
            k = lengthArray;
            for (j = nbClass; j > 1; j--) {
                kclass[j - 2] = k = (int) mat1[k - 1][j - 1] - 1;
            }

            breaks.Add(values[0]);
            for (i = 1; i < nbClass; i++) {
                breaks.Add(values[(int) kclass[i - 1] - 1]);
            }
            breaks.Add(values[lengthArray - 1]);
            return breaks;
        }
    }
}