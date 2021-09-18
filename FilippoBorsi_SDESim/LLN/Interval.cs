using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLN
{
    class Interval : IComparable<Interval>
    {
       

            public double lowerBound;
            public double upperBound;
            public int count;
            public double mean;



            //public double Mean { get => mean; set => mean = value; }


            public int CompareTo(Interval other)
            {
                if (this.lowerBound < other.lowerBound)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }

            }
            public void updateMean(double elem)
            {
                mean = mean + (elem - mean) / (double)count;
            }


        }

    }

