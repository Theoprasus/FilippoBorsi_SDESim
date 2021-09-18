using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LLN
{
    public partial class FormGlivenkCantelli : Form
    {
        public FormGlivenkCantelli()
        {
            InitializeComponent();
        }

        public Bitmap b;
        public Graphics g;

        Rectangle viewPort;
        double x_min = 0;
        double x_max = 6;
        double y_min = 0;
        double y_max = 1;
        int globalcounter = 0;





    
        private void button1_Click(object sender, EventArgs e)
        {

            //initializeGraphics();


            //this.pictureBox1.Image = b;
            if (timer1.Enabled)
            {
                this.timer1.Stop();

            }
            else
            {
                this.timer1.Start();
            }








        }


        private SortedDictionary<Interval, int> populateDictionary(List<(double, double)> ls, int numberHisto)
        {
            SortedDictionary<Interval, int> dictDistributions = new SortedDictionary<Interval, int>();
            double min = getMinMean(ls);
            double max = getMaxMean(ls);
            Mean_DictHelper mdh = new Mean_DictHelper();
            foreach ((double, double) tuple in ls)
            {
                double step_dict = (max - min) / numberHisto;
                if (step_dict == 0)
                {
                    step_dict = 0.01;
                }

                mdh.UpdateDict(dictDistributions, tuple.Item1, step_dict);
            }

            return dictDistributions;


        }


        Random rand = new Random();

        private double getSampleMean(int min, int max, int sample_size)
        {
            double sample_mean = 0;

            Mean_DictHelper mdh = new Mean_DictHelper();
            for (int i = 0; i < sample_size; i++)
            {
                int elem = rand.Next(min, max);
                sample_mean = mdh.onlineMean(sample_mean, elem, i + 1);
            }
            return sample_mean;
        }

        private List<(double, double)> getListOfMeanAndFreq(int number_sample, int min, int max, int sample_size)
        {
            List<(double, double)> ls_tuple = new List<(double, double)>();
            for (int i = 0; i < number_sample; i++)
            {
                double mean = getSampleMean(min, max, sample_size);
                ls_tuple.Add((mean, (double)1 / number_sample));
            }

            return ls_tuple;
        }

        private List<DataPoints> getPointsMeanCdf(List<(double, double)> ls, int npoints, double minMean, double maxMean)
        {
            List<DataPoints> ls_punti = new List<DataPoints>();
            double step = (maxMean - minMean) / npoints;
            if (step == 0)
                step += 0.01;
            double start = minMean;
            while (start <= maxMean)
            {
                //start += step;
                //data points with mean + step startimg values
                DataPoints p = new DataPoints();
                p.x = start;
                foreach ((double, double) tuple in ls)
                {
                    if (tuple.Item1 <= start)
                    {
                        //item2 is frequency 
                        p.y += tuple.Item2;

                    }
                }
                ls_punti.Add(p);
                start += step;
            }
            ls_punti.RemoveAt(ls_punti.Count - 1);
            DataPoints pf = new DataPoints();
            start -= step;
            pf.x = start;
            pf.y = 1;
            ls_punti.Add(pf);
            return ls_punti;
        }


        private PointF[] ListPointsToArray(List<DataPoints> lista_punti)
        {
            PointF[] array = new PointF[lista_punti.Count];
            for (int i = 0; i < lista_punti.Count; i++)
            {
                array[i] = new PointF((float)lista_punti[i].x, (float)lista_punti[i].y);
            }
            return array;
        }

        private double getMinMean(List<(double, double)> ls)
        {
            double min = ls[0].Item1;
            for (int i = 1; i < ls.Count; i++)
            {
                if (ls[i].Item1 < min)
                    min = ls[i].Item1;
            }
            return min;
        }

        private double getMaxMean(List<(double, double)> ls)
        {
            double max = ls[0].Item1;
            for (int i = 1; i < ls.Count; i++)
            {
                if (ls[i].Item1 > max)
                    max = ls[i].Item1;
            }
            return max;
        }







        public void initializeGraphics()
        {

            b = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(b);


            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.Clear(Color.White);
        }


        public struct DataPoints
        {

            public double x;
            public double y;

        }



        public (double, double) getMin(List<DataPoints> lista)
        {
            double[] x = new double[lista.Count];
            double[] y = new double[lista.Count];
            for (int i = 0; i < lista.Count; i++)
            {
                x[i] = lista[i].x;
                y[i] = lista[i].y;
            }
            return (x.Min(), y.Min());
        }

        

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            globalcounter++;
            initializeGraphics();
            //creazione lista di tuple mean and frequency 1000 samples of size 20 of [1,7)
            List<(double, double)> lista_tuple = getListOfMeanAndFreq(1000, 1, 7, globalcounter);


            double min = getMinMean(lista_tuple);
            double max = getMaxMean(lista_tuple);

            // 150 is the number of points when aggregating 
            List<DataPoints> listaPoint = getPointsMeanCdf(lista_tuple, 150, min, max);

            //number of histo 12
            SortedDictionary<Interval, int> dictDistributions = populateDictionary(lista_tuple, 12);




            viewPort = new Rectangle(20, 50, 600, 400);

            //invoke the draw method from g that draw on bitmap
            g.DrawRectangle(Pens.DarkGreen, viewPort);



            HelpGraphics hg = new HelpGraphics();

            // draw axis name of axis
            hg.draw_axis(g, viewPort, x_min, x_max, y_min, y_max);
            hg.DrawRotatedTextAt(g, 0, "n = " + globalcounter.ToString(), Top, Left, DefaultFont, Brushes.Red);

            //g.DrawLine(Pens.Black, new Point(transform_x((int)x_min+10, viewPort, x_min, range_x), transform_y(-3, viewPort, y_min, range_y)), new Point(transform_x((int)x_min+10, viewPort, x_min, range_x), transform_y(3, viewPort, y_min, range_y)));
            //draw rugplot and values


            hg.rugPlot(g, x_min, x_max, y_min, y_max, viewPort, 10);


            PointF[] listaPointTrasf = hg.GetArrayOfPointsTrasformed(g, viewPort, ListPointsToArray(listaPoint), x_min, x_max - x_min, y_min, y_max - y_min);



            //try to drow histogram
            hg.drawHistogram(g, dictDistributions, viewPort, x_min, x_max - x_min, y_min, y_max - y_min, false, 1);

            //g.DrawLines(Pens.Red, listaPointTrasf);
            hg.drawZigZAgLine(g, listaPointTrasf, Pens.Red);


            this.pictureBox1.Image = b;

            if (globalcounter == 200)
            {
                globalcounter = 0;
            }

        }
    }
}

  
