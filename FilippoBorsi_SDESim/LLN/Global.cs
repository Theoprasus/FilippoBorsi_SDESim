using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLN
{
    static class Global
    {
        public static double y_min;
        public static double x_min;
        public static double range_x;
        public static double range_y;
        public static Rectangle viewPort;
        public static Graphics g;
        public static double y_max;
        public static double x_max;

        public static List<List<PointF>> getListOfLines(int m, int n, double p)
        {
            List<List<PointF>> listaLinee = new List<List<PointF>>();
            Random ranGenerator = new Random();

            for (int index_listaLinee = 0; index_listaLinee < m; index_listaLinee++)
            {
                listaLinee.Add(new List<PointF>());

                int Bernoulli_success = 0;
                int Bernoulli_failures = 0;
                //p = 1 - p;
                for (int i = 1; i <= n; i++)
                {
                    double value_for_Bernoulli = ranGenerator.NextDouble();
                    // p must be 0<=p<=1 
                    if (value_for_Bernoulli < p)
                    {
                        Bernoulli_success += 1;
                        PointF point = new PointF(i, (float)Bernoulli_success / (float)(Bernoulli_success + Bernoulli_failures));
                        listaLinee[index_listaLinee].Add(point);
                    }
                    else
                    {


                        Bernoulli_failures += 1;
                        PointF point = new PointF(i, (float)Bernoulli_success / (float)(Bernoulli_success + Bernoulli_failures));
                        listaLinee[index_listaLinee].Add(point);
                    }


                }
            }

            return listaLinee;

        }




        public static List<List<PointF>> getListOfLinesRandomWalk(int m, int n)
        {
            List<List<PointF>> listaLinee = new List<List<PointF>>();
            Random ranGenerator = new Random();

            //for all lines m
            for (int index_listaLinee = 0; index_listaLinee < m; index_listaLinee++)
            {
                listaLinee.Add(new List<PointF>());

                //reset Y_past to 0 for each line
                int Y_past = 0;


                for (int i = 0; i < n; i++)
                {

                    // p must be 0<=p<=1 


                    //begginnig point is 0      
                    PointF point = new PointF(i, Y_past);
                    listaLinee[index_listaLinee].Add(point);
                    // after we update beginning point with radamcher +1 or -1
                    Y_past += rademacher(ranGenerator);




                }
            }

            return listaLinee;

        }


        public static int rademacher(Random ran)
        {
            int choice = ran.Next(2);
            if (choice == 0)
            {
                return -1;

            }
            else
            {
                return 1;
            }

        }



        public static List<List<PointF>> GetListOfLinesTrasformed(List<List<PointF>> lista_linee)
        {
            HelpGraphics hg = new HelpGraphics();
            for (int i = 0; i < lista_linee.Count; i++)
            {
                //List<PointF> lista_traf = new List<PointF>();
                for (int j = 0; j < lista_linee[i].Count; j++)
                {
                    lista_linee[i][j] = hg.transformPointF(lista_linee[i][j], g, viewPort, x_min, range_x, y_min, range_y);
                }


            }
            return lista_linee;
        }

        public static void DrawLines(List<List<PointF>> lista_linee)
        {

            List<Color> lista_colori = new List<Color>
            {
                Color.Red,
                Color.Blue,
                Color.Maroon,
                Color.Beige,
                Color.CadetBlue,
                Color.Green,
                Color.DarkOrange,
                Color.Lime,
                Color.HotPink,
                Color.Yellow,
                Color.Salmon,
                Color.Cyan,
                Color.MediumPurple

                };

            for (int i = 0; i < lista_linee.Count; i++)
            {

                g.DrawLines(new Pen(lista_colori[i % lista_colori.Count]), lista_linee[i].ToArray());
            }
        }

        public static SortedDictionary<Interval, int> getIntervalY(List<List<PointF>> ll, int Nbernoulli, double h_size, double p, double epsilon)
        {
            SortedDictionary<Interval, int> dictionary_Y = new SortedDictionary<Interval, int>();
            DictInter dizioInter = new DictInter();
            if (Nbernoulli < ll[0].Count && Nbernoulli >= 0)
            {
                // this part is for minimum and maximum
                double min = ll[0][Nbernoulli].Y;
                double max = ll[0][Nbernoulli].Y;
                // find min max 
                for (int i = 0; i < ll.Count; i++)
                {
                    // step 0.05
                    if (ll[i][Nbernoulli].Y >= p - epsilon && ll[i][Nbernoulli].Y <= p + epsilon)
                    {
                        if (ll[i][Nbernoulli].Y > max)
                            max = ll[i][Nbernoulli].Y;
                        if (ll[i][Nbernoulli].Y < min)
                            min = ll[i][Nbernoulli].Y;
                    }
                }

                for (int i = 0; i < ll.Count; i++)
                {
                    // step 0.05
                    if (ll[i][Nbernoulli].Y >= p - epsilon && ll[i][Nbernoulli].Y <= p + epsilon)
                        dizioInter.UpdateDict(dictionary_Y, ll[i][Nbernoulli].Y, h_size); /*0.0025);*/
                    //change interval here
                    //dizioInter.UpdateDict(dictionary_Y, ll[i][Nbernoulli].Y, (max - min) / (double)50);
                }
            }
            return dictionary_Y;
        }


        public static SortedDictionary<Interval, int> getIntervalY(List<List<PointF>> ll, int slice_index_histogram, double h_size)
        {
            SortedDictionary<Interval, int> dictionary_Y = new SortedDictionary<Interval, int>();
            DictInter dizioInter = new DictInter();
            if (slice_index_histogram < ll[0].Count && slice_index_histogram >= 0)
            {
                // this part is for minimum and maximum
                int min = 0;
                int max = 0;
                // find min max 
                for (int i = 0; i < ll.Count; i++)

                {
                    //update dictionary with result for histogram on that particular x_index
                    dizioInter.UpdateDict(dictionary_Y, ll[i][slice_index_histogram].Y, h_size);

                }
                // step 0.05





            }
            return dictionary_Y;
        }




        public static (int, int) getMinMax(List<List<PointF>> ll)
        {
            int min = 0;
            int max = 0;
            // find min max 
            for (int i = 0; i < ll.Count; i++)
            {
                for (int j = 0; j < ll[i].Count; j++)
                {
                    if (ll[i][j].Y > max)

                        max = (int)ll[i][j].Y;
                    if (ll[i][j].Y < min)
                        min = (int)ll[i][j].Y;
                }
            }
            return (min, max);
        }


        public static int getMaxIndexatBernoulliN(List<List<PointF>> ll, int Nbernoulli)
        {
            if (Nbernoulli < ll[0].Count && Nbernoulli >= 0)
            {
                // this part is for minimum and maximum

                double max = ll[0][Nbernoulli].Y;
                int i_max = 0;
                // find min max 
                for (int i = 0; i < ll.Count; i++)
                {
                    // step 0.05
                    if (ll[i][Nbernoulli].Y > max)
                        max = ll[i][Nbernoulli].Y;
                    i_max = i;

                }
                return i_max;

            }
            return 0;
        }

        public static int getIntervalCount(int n, List<List<PointF>> listLines, double p, double epsilon)
        {
            Interval pEpsilon = new Interval();
            pEpsilon.upperBound = p + epsilon;
            pEpsilon.lowerBound = p - epsilon;
            for (int i = 0; i < listLines.Count; i++)
            {
                if (listLines[i][n].Y <= (float)pEpsilon.upperBound && listLines[i][n].Y >= (float)pEpsilon.lowerBound)
                {
                    pEpsilon.count++;
                }
            }
            return pEpsilon.count;
        }








    }
}
