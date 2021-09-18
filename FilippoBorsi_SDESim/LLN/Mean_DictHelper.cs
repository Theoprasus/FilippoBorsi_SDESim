using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLN
{
    class Mean_DictHelper
    {




        public void updateDictDiscrete(int elem, SortedDictionary<int, int> dict)
        {
            if (dict.ContainsKey(elem))
            {
                dict[elem] += 1;
            }
            else
            {
                dict.Add(elem, 1);
            }
        }








        public string printDictDiscrete(SortedDictionary<int, int> dict, int total_number_observations)
        {
            String text = "";
            foreach (KeyValuePair<int, int> pair in dict)
            {

                text += "total number of " + pair.Key.ToString().PadRight(6) + "  =   " +
                    pair.Value.ToString().PadRight(10) + "relative frequence ".PadLeft(38) +
                    String.Format("{0:0.00}", pair.Value / (double)total_number_observations) + "\n";

            }
            return text;
        }






        //---------------------------------------------------------------------------------------------------------------------------------------------


        public double onlineMean(double oldAvg, double n, int scanned_number_up_to_now)
        {
            double avg = oldAvg + (n - oldAvg) / (double)scanned_number_up_to_now;
            return avg;
        }



        public void UpdateDict(SortedDictionary<Interval, int> dict, double elem, double step)
        {
            if (dict.Count == 0 && step != 0)
            {
                Interval inter = new Interval();
                double resto = (elem % step);
                if (elem >= 0)
                {
                    inter.lowerBound = elem - resto;
                    inter.upperBound = elem - resto + step;
                    dict.Add(inter, 1);
                    inter.count = 1;
                    inter.updateMean(elem);
                }
                else
                {
                    inter.upperBound = elem - resto;
                    inter.lowerBound = elem - resto - step;
                    dict.Add(inter, 1);
                    inter.count = 1;
                    inter.updateMean(elem);
                }


            }
            else if (ElemIsindict(elem, dict))
            {

                int? k = getKey(elem, dict);
                if (k != null)
                {
                    Interval tmp = dict.ElementAt((int)k).Key;
                    tmp.count += 1;
                    tmp.updateMean(elem);
                }


            }


            else
            {
                if (dict.Count != 0 && (elem >= dict.ElementAt(0).Key.upperBound))
                {
                    while (elem >= dict.ElementAt(0).Key.upperBound)
                    {

                        Interval inter_n = new Interval();
                        inter_n.lowerBound = dict.ElementAt(0).Key.upperBound;
                        inter_n.upperBound = inter_n.lowerBound + step;
                        dict.Add(inter_n, 1);
                    }

                    dict.ElementAt(0).Key.count += 1;
                    dict.ElementAt(0).Key.updateMean(elem);

                }

                if (dict.Count != 0 && (elem < dict.ElementAt(dict.Count - 1).Key.lowerBound))
                {
                    while (elem < dict.ElementAt(dict.Count - 1).Key.lowerBound)
                    {

                        Interval inter_n = new Interval();
                        inter_n.upperBound = dict.ElementAt(dict.Count - 1).Key.lowerBound;
                        inter_n.lowerBound = inter_n.upperBound - step;
                        dict.Add(inter_n, 1);
                    }

                    dict.ElementAt(dict.Count - 1).Key.count += 1;
                    dict.ElementAt(dict.Count - 1).Key.updateMean(elem);

                }

                //Interval inter = new Interval();
                //int resto = (int)elem % step;
                //inter.lowerBound = (int)elem - resto;
                //inter.upperBound = (int)elem - resto + step;
                //dict.Add(inter, 1);
                //inter.updateMean(elem);
                //inter.count = 1;
            }


        }














        public int? getKey(double elem, SortedDictionary<Interval, int> dict)
        {
            for (int i = 0; i < dict.Count; i++)
            {


                if (elem < dict.ElementAt(i).Key.upperBound && elem >= dict.ElementAt(i).Key.lowerBound)
                {
                    return i;
                }

            }
            return null;
        }

        public bool ElemIsindict(double elem, SortedDictionary<Interval, int> dict)
        {
            foreach (KeyValuePair<Interval, int> pair in dict)
            {
                if (elem < pair.Key.upperBound && elem >= pair.Key.lowerBound)
                {
                    return true;
                }

            }
            return false;
        }




        public double onlineSquaredSum(double[] array)
        {
            double mean = 0;
            double squared_sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                double x = array[i];
                double old_mean = mean;
                mean = onlineMean(mean, x, i + 1);
                squared_sum = squared_sum + (x - mean) * (x - old_mean);
            }
            return squared_sum; //suqared_sum/array.length for variance
        }


        // Cn = Cn-1 + (Xn - mean(Xn-1)) * (Yn -mean(Yn)) 

        public double onlineCoMoment(double[] array_x, double[] array_y)
        {
            if (array_x.Length == array_y.Length)
            {
                double mean_x = 0;
                double mean_y = 0;
                double coMomentsum = 0;
                for (int i = 0; i < array_x.Length; i++)
                {
                    double x = array_x[i];
                    double y = array_y[i];
                    double old_mean = mean_x;
                    mean_x = onlineMean(mean_x, x, i + 1);
                    mean_y = onlineMean(mean_y, y, i + 1);
                    coMomentsum += (x - old_mean) * (y - mean_y);
                }
                return coMomentsum; //suqared_sum/array.length for variance
            }
            return 0;

        }
        public double onlineMean(double[] array)
        {
            double old_avg = 0;
            double mean = 0;
            for (int i = 0; i < array.Length; i++)
            {
                old_avg = mean;
                mean = onlineMean(old_avg, array[i], i + 1);

            }
            return mean;
        }



        public int getCountAt_i(SortedDictionary<Interval, int> dictionary_x, int i)
        {
            return dictionary_x.ElementAt(i).Key.count;

        }
        public int getTotalCountDictionary(SortedDictionary<Interval, int> dictionary_x)
        {
            int sum_rows = 0;
            foreach (KeyValuePair<Interval, int> kvp in dictionary_x)
            {
                sum_rows += kvp.Key.count;
            }
            return sum_rows;
        }

        public int getTotalCountLess(SortedDictionary<Interval, int> dictionary_x, double less)
        {
            int sum_rows = 0;
            foreach (KeyValuePair<Interval, int> kvp in dictionary_x)
            {
                if (kvp.Key.lowerBound <= less)
                    sum_rows += kvp.Key.count;
            }
            return sum_rows;
        }

    }
    }
