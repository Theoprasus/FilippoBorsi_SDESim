using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LLN
{
    public partial class FormLLN : Form
    {

        public Bitmap b;
        public Graphics g;
        Rectangle viewPort = new Rectangle(50, 10, 1500, 1200);

        double x_min = -4;
        double y_min = -1;
        double x_max = 200;
        double y_max = 1;
        double range_x = 204;
        double range_y = 2;

        public FormLLN()
        {
            InitializeComponent();
            this.textBoxN.Text = "2000";
            this.textBoxE.Text = "0.025";
            this.textBoxP.Text = "0.50";
            this.textBoxM.Text = "10000";
            this.textBoxH.Text = "0.0025";

        }

        private void button1_Click(object sender, EventArgs e)
        {

            // n is the number of bernoulli variables
            int n = 2000;
            // try to take input from user
            if (this.textBoxN.Text != null && !string.IsNullOrEmpty(this.textBoxN.Text))
            {
                string n_string = this.textBoxN.Text.Trim();
                int n_temp = 0;

                if (int.TryParse(n_string, out n_temp))
                {
                    if (n_temp > 0)
                        n = n_temp;
                }

            }
            this.textBoxN.Text = n.ToString();
            //print that input or default in textBoxN



            x_max = n;
            range_x = x_max - x_min;
            double epsilon = 0.025;
            // try to take input from user
            if (this.textBoxE.Text != null && !string.IsNullOrEmpty(this.textBoxE.Text))
            {
                string e_string = this.textBoxE.Text.Trim();
                e_string = e_string.Replace(",", ".");
                double e_temp = 0;

                if (double.TryParse(e_string, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out e_temp))
                {
                    if (e_temp <= 1 && e_temp >= 0)
                        epsilon = e_temp;
                }


            }
            this.textBoxE.Text = epsilon.ToString();
            //print that input or default in textBoxE


            //draw axis 
            initializeGraphics();
            HelpGraphics graph = new HelpGraphics();
            graph.draw_axis(g, viewPort, x_min, x_max, y_min, y_max);





            //create lines and plot them
            double p = 0.50;
            // try to take input from user
            if (this.textBoxP.Text != null && !string.IsNullOrEmpty(this.textBoxP.Text))
            {
                string p_string = this.textBoxP.Text.Trim();
                p_string = p_string.Replace(",", ".");
                double p_temp = 0;

                if (double.TryParse(p_string, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out p_temp))
                {
                    if (p_temp <= 1 && p_temp >= 0)
                        p = p_temp;
                }


            }
            this.textBoxP.Text = p.ToString();
            //print that input or default in textBoxE




            int m = 10000;

            // try to take input from user
            if (this.textBoxM.Text != null && !string.IsNullOrEmpty(this.textBoxM.Text))
            {
                string m_string = this.textBoxM.Text.Trim();
                int m_temp = 0;

                if (int.TryParse(m_string, out m_temp))
                {
                    if (m_temp > 0)
                        m = m_temp;
                }

            }
            this.textBoxM.Text = m.ToString();
            //print that input or default in textBoxE





            double h_size = 0.0025;
            // try to take input from user
            if (this.textBoxH.Text != null && !string.IsNullOrEmpty(this.textBoxH.Text))
            {
                string h_string = this.textBoxH.Text.Trim();
                h_string = h_string.Replace(",", ".");
                double h_temp = 0;

                if (double.TryParse(h_string, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US"), out h_temp))
                {
                    if (h_temp <= 1 && h_temp >= 0)
                        h_size = h_temp;
                }


            }
            this.textBoxH.Text = h_size.ToString();
            //print that input or default in textBoxE


            Global.g = g;
            Global.viewPort = viewPort;

            Global.x_min = x_min;
            Global.y_min = y_min;
            Global.x_max = x_max;
            Global.y_max = y_max;
            Global.range_x = range_x;
            Global.range_y = range_y;






            List<List<PointF>> listLines = Global.getListOfLines(m, n, p);




            //very important to this this here
            SortedDictionary<Interval, int> intervalY = Global.getIntervalY(listLines, listLines[0].Count - 1, h_size, p, epsilon);
            SortedDictionary<Interval, int> intervalY2 = Global.getIntervalY(listLines, (listLines[0].Count - 1) / 2, h_size, p, epsilon);
            SortedDictionary<Interval, int> intervalY3 = Global.getIntervalY(listLines, (listLines[0].Count - 1) / 4, h_size, p, epsilon);

            //ADD TO INVERVAL P+e ,P-e

            int i1 = Global.getIntervalCount(listLines[0].Count - 1, listLines, p, epsilon);
            int i2 = Global.getIntervalCount((listLines[0].Count - 1) / 2, listLines, p, epsilon);
            int i3 = Global.getIntervalCount((listLines[0].Count - 1) / 4, listLines, p, epsilon);




            listLines = Global.GetListOfLinesTrasformed(listLines);

            Global.DrawLines(listLines);

            // get Dictionary of interval for y

            HelpGraphics hg = new HelpGraphics();


            //hg.DrawRotatedTextAt(g, 0, i1.ToString(), listLines[0][listLines[0].Count - 1].X, listLines[0][listLines[0].Count - 1].Y, SystemFonts.DefaultFont, Brushes.Black);
            hg.DrawRotatedTextAt(g, 0, i1.ToString() + " p+e, p-e", listLines[0][listLines[0].Count - 1].X, hg.transform_y((float)0, viewPort, y_min, range_y), SystemFonts.DefaultFont, Brushes.Black);
            //hg.DrawRotatedTextAt(g, 0, i2.ToString(), listLines[0][(listLines[0].Count - 1) / 2].X, (listLines[getMaxIndexatBernoulliN(listLines, ((listLines[0].Count - 1) / 2)) ][(listLines[0].Count - 1) / 2].Y) , SystemFonts.DefaultFont, Brushes.Black);
            hg.DrawRotatedTextAt(g, 0, i2.ToString() + " p+e, p-e", listLines[0][(listLines[0].Count - 1) / 2].X, hg.transform_y((float)0, viewPort, y_min, range_y), SystemFonts.DefaultFont, Brushes.Black);
            //hg.DrawRotatedTextAt(g, 0, i2.ToString(), listLines[0][(listLines[0].Count - 1) / 4].X, listLines[0][(listLines[0].Count - 1) / 4].Y, SystemFonts.DefaultFont, Brushes.Black);
            hg.DrawRotatedTextAt(g, 0, i3.ToString() + " p+e, p-e", listLines[0][(listLines[0].Count - 1) / 4].X, hg.transform_y((float)0, viewPort, y_min, range_y), SystemFonts.DefaultFont, Brushes.Black);
            hg.drawHistogram_y(g, intervalY, range_x, range_y, x_min, y_min, viewPort, listLines[0].Count - 1,1 );
            hg.drawHistogram_y(g, intervalY2, range_x, range_y, x_min, y_min, viewPort, (listLines[0].Count - 1) / 2,1 );
            hg.drawHistogram_y(g, intervalY3, range_x, range_y, x_min, y_min, viewPort, (listLines[0].Count - 1) / 4,1 );

            // now I should insert p+e and p+e

            hg.draw_Line_trasformed(g, viewPort, x_min, x_max, y_min, y_max, 0, (float)p, n, (float)p);

            hg.draw_Line_trasformed(g, viewPort, x_min, x_max, y_min, y_max, 0, (float)(p + epsilon), n, (float)(p + epsilon));
            hg.draw_Line_trasformed(g, viewPort, x_min, x_max, y_min, y_max, 0, (float)(p - epsilon), n, (float)(p - epsilon));

            this.pictureBox1.Image = b;

















































            //public List<List<PointF>> getListOfLines(int m, int n, double p)
            //{
            //    List<List<PointF>> listaLinee = new List<List<PointF>>();
            //    Random ranGenerator = new Random();

            //    for (int index_listaLinee = 0; index_listaLinee < m; index_listaLinee++)
            //    {
            //        listaLinee.Add(new List<PointF>());

            //        int Bernoulli_success = 0;
            //        int Bernoulli_failures = 0;
            //        //p = 1 - p;
            //        for (int i = 1; i <= n; i++)
            //        {
            //            double value_for_Bernoulli = ranGenerator.NextDouble();
            //            // p must be 0<=p<=1 
            //            if (value_for_Bernoulli < p)
            //            {
            //                Bernoulli_success += 1;
            //                PointF point = new PointF(i, (float)Bernoulli_success / (float)(Bernoulli_success + Bernoulli_failures));
            //                listaLinee[index_listaLinee].Add(point);
            //            }
            //            else
            //            {


            //                Bernoulli_failures += 1;
            //                PointF point = new PointF(i, (float)Bernoulli_success / (float)(Bernoulli_success + Bernoulli_failures));
            //                listaLinee[index_listaLinee].Add(point);
            //            }


            //        }
            //    }

            //    return listaLinee;

            //}




            //public List<List<PointF>> getListOfLinesRandomWalk(int m, int n)
            //{
            //    List<List<PointF>> listaLinee = new List<List<PointF>>();
            //    Random ranGenerator = new Random();

            //    //for all lines m
            //    for (int index_listaLinee = 0; index_listaLinee < m; index_listaLinee++)
            //    {
            //        listaLinee.Add(new List<PointF>());

            //        //reset Y_past to 0 for each line
            //        int Y_past = 0;


            //        for (int i = 0; i < n; i++)
            //        {

            //            // p must be 0<=p<=1 


            //           //begginnig point is 0      
            //           PointF point = new PointF(i, Y_past);
            //           listaLinee[index_listaLinee].Add(point);
            //            // after we update beginning point with radamcher +1 or -1
            //            Y_past += rademacher(ranGenerator);                




            //        }
            //    }

            //    return listaLinee;

            //}


            //private int rademacher(Random ran)
            //{
            //    int choice = ran.Next(2);
            //    if (choice == 0)
            //    {
            //        return -1;

            //    }
            //    else
            //    {
            //        return 1;
            //    }

            //}



            //public List<List<PointF>> GetListOfLinesTrasformed(List<List<PointF>> lista_linee)
            //{
            //    HelpGraphics hg = new HelpGraphics();
            //    for (int i = 0; i < lista_linee.Count; i++)
            //    {
            //        //List<PointF> lista_traf = new List<PointF>();
            //        for (int j = 0; j < lista_linee[i].Count; j++)
            //        {
            //            lista_linee[i][j] = hg.transformPointF(lista_linee[i][j], g, viewPort, x_min, range_x, y_min, range_y);
            //        }


            //    }
            //    return lista_linee;
            //}

            //public void DrawLines(List<List<PointF>> lista_linee)
            //{

            //    List<Color> lista_colori = new List<Color>
            //    {
            //        Color.Red,
            //        Color.Blue,
            //        Color.Maroon,
            //        Color.Beige,
            //        Color.CadetBlue,
            //        Color.Green,
            //        Color.DarkOrange,
            //        Color.Lime,
            //        Color.HotPink,
            //        Color.Yellow,
            //        Color.Salmon,
            //        Color.Cyan,
            //        Color.MediumPurple

            //        };

            //    for (int i = 0; i < lista_linee.Count; i++)
            //    {

            //        g.DrawLines(new Pen(lista_colori[i % lista_colori.Count]), lista_linee[i].ToArray());
            //    }
            //}

            //private SortedDictionary<Interval, int> getIntervalY(List<List<PointF>> ll, int Nbernoulli , double h_size, double p, double epsilon)
            //{
            //    SortedDictionary<Interval, int> dictionary_Y = new SortedDictionary<Interval, int>();
            //    DictInter dizioInter = new DictInter();
            //    if (Nbernoulli< ll[0].Count && Nbernoulli >=0)
            //    {
            //        // this part is for minimum and maximum
            //        double min = ll[0][Nbernoulli].Y;
            //        double max = ll[0][Nbernoulli].Y;
            //        // find min max 
            //        for (int i = 0; i < ll.Count; i++)
            //        {
            //            // step 0.05
            //            if (ll[i][Nbernoulli].Y >= p - epsilon && ll[i][Nbernoulli].Y <= p + epsilon)
            //            {
            //                if (ll[i][Nbernoulli].Y > max)
            //                    max = ll[i][Nbernoulli].Y;
            //                if (ll[i][Nbernoulli].Y < min)
            //                    min = ll[i][Nbernoulli].Y;
            //            }
            //        }

            //        for (int i = 0; i< ll.Count; i++)
            //        {
            //            // step 0.05
            //            if (ll[i][Nbernoulli].Y>= p-epsilon  && ll[i][Nbernoulli].Y <= p + epsilon)
            //                dizioInter.UpdateDict(dictionary_Y, ll[i][Nbernoulli].Y, h_size); /*0.0025);*/
            //            //change interval here
            //            //dizioInter.UpdateDict(dictionary_Y, ll[i][Nbernoulli].Y, (max - min) / (double)50);
            //        }
            //    }
            //    return dictionary_Y;
            //}


            //private SortedDictionary<Interval, int> getIntervalY(List<List<PointF>> ll, int slice_index_histogram, double h_size)
            //{
            //    SortedDictionary<Interval, int> dictionary_Y = new SortedDictionary<Interval, int>();
            //    DictInter dizioInter = new DictInter();
            //    if (slice_index_histogram < ll[0].Count && slice_index_histogram >= 0)
            //    {
            //        // this part is for minimum and maximum
            //        int min = 0;
            //        int max = 0;
            //        // find min max 
            //        for (int i = 0; i < ll.Count; i++)

            //        {
            //            //update dictionary with result for histogram on that particular x_index
            //           dizioInter.UpdateDict(dictionary_Y, ll[i][slice_index_histogram].Y, h_size);

            //         }
            //            // step 0.05





            //    }
            //    return dictionary_Y;
            //}




            //private (int, int) getMinMax(List<List<PointF>> ll)
            //{
            //    int min = 0;
            //    int max = 0;
            //    // find min max 
            //    for (int i = 0; i < ll.Count; i++)
            //    {
            //        for (int j = 0; j < ll[i].Count; j++)
            //        {
            //            if (ll[i][j].Y > max)

            //                max = (int)ll[i][j].Y;
            //            if (ll[i][j].Y < min)
            //                min = (int)ll[i][j].Y;
            //        }
            //    }
            //    return (min, max);
            //}


            //private int getMaxIndexatBernoulliN(List<List<PointF>> ll, int Nbernoulli)
            //{
            //    if (Nbernoulli < ll[0].Count && Nbernoulli >= 0)
            //    {
            //        // this part is for minimum and maximum

            //        double max = ll[0][Nbernoulli].Y;
            //        int i_max = 0;
            //        // find min max 
            //        for (int i = 0; i < ll.Count; i++)
            //        {
            //            // step 0.05
            //            if (ll[i][Nbernoulli].Y > max)
            //                max = ll[i][Nbernoulli].Y;
            //            i_max = i;

            //        }
            //        return i_max;

            //    }
            //    return 0;
            //}

            //private int getIntervalCount(int n, List<List<PointF>> listLines, double p, double epsilon)
            //{
            //    Interval pEpsilon = new Interval();
            //    pEpsilon.upperBound = p + epsilon;
            //    pEpsilon.lowerBound = p - epsilon;
            //    for (int i = 0; i < listLines.Count; i++)
            //    {
            //        if (listLines[i][n].Y <= (float)pEpsilon.upperBound && listLines[i][n].Y >= (float)pEpsilon.lowerBound)
            //        {
            //            pEpsilon.count++;
            //        }
            //    }
            //    return pEpsilon.count;
            //}



        }

        private void initializeGraphics()
        {

            b = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        }
    }
}
