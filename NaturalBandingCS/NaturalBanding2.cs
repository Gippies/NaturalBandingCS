using System;
using System.Collections.Generic;

namespace NaturalBandingCS {
    public class NaturalBanding2 {
        public static List<double> JenksBreakValues(List<double> values, int nb_class) {
            int i3, i4;
            int i = 0, j = 0, l = 0, m = 0, k = nb_class;
            double v, val, s1, s2, w;
            double[] kclass;
            var mat1 = new List<List<double>>();
            var mat2 = new List<List<double>>();
            List<double> row = null;
            var breaks = new List<double>();
            var length_array = values.Count;

            values.Sort();

            for (i = 0; i < length_array; i++) {
                row = new List<double>();
                for (j = 0; j < k; j++) {
                    row.Add(1.0);
                }
                mat1.Add(row);
            }

            for (i = 0; i < length_array; i++) {
                row = new List<double>();
                for (j = 0; j < k; j++) {
                    row.Add(Double.MaxValue);
                }
                mat2.Add(row);
            }

            v = 0;
            for (l = 2; l <= length_array; l++) {
                s1 = s2 = w = 0;
                for (m = 1; m <= l; m++) {
                    i3 = l - m + 1;
                    val = values[i3 - 1];
                    s2 += val * val;
                    s1 += val;
                    w++;
                    v = s2 - (s1 * s1) / w;
                    i4 = i3 - 1;

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

            kclass = new double[k];
            k = length_array;
            for (j = nb_class; j > 1; j--) {
                kclass[j - 2] = k = (int) mat1[k - 1][j - 1] - 1;
            }

            breaks.Add(values[0]);
            for (i = 1; i < nb_class; i++) {
                breaks.Add(values[(int) kclass[i - 1] - 1]);
            }
            breaks.Add(values[length_array - 1]);
            return breaks;
        }
    }
}