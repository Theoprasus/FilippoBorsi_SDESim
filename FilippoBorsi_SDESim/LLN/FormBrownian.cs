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
    public partial class FormBrownian : Form
    {
        public FormBrownian()
        {
            InitializeComponent();
            this.textBoxN.Text = "1000";
            this.textBoxM.Text = "500";
            this.textBoxSigma.Text = "50";
        }

     


        public Bitmap b;
        public Graphics g;
        Rectangle viewPort = new Rectangle(50, 10, 1200, 700);

        double x_min = -4;
        double y_min = -1;
        double x_max = 200;
        double y_max = 1;
        double range_x = 204;
        double range_y = 2;




        private void button1_Click(object sender, EventArgs e)
        {

            // n is the number of bernoulli variables
            int n = 1000;

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






            x_max = n;
            range_x = x_max - x_min;



            int m = 500;

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

            double sigma = 50;
            UserInput ui = new UserInput();
            if (ui.getDouble(this.textBoxSigma.Text, 0, n) != null)
                sigma = (double)ui.getDouble(this.textBoxSigma.Text, 0, n);
            // t to take input from user
            //if (this.textBoxSigma.Text != null && !string.IsNullOrEmpty(this.textBoxSigma.Text))
            //{
            //    string l_string = this.textBoxSigma.Text.Trim();
            //    double l_temp = 0;

            //    if (double.TryParse(l_string, out l_temp))
            //    {
            //        if (l_temp <= n && l_temp >= 0)
            //            sigma = l_temp;
            //    }

            //}
            this.textBoxSigma.Text = sigma.ToString();










            List<List<PointF>> listLinesRandom = getListOfBrownian(m, n, sigma);





            //update min max y cooordinate
            (int, int) t = getMinMax(listLinesRandom);
            y_min = t.Item1 - 1;
            y_max = t.Item2 + 1;
            range_y = y_max - y_min;

           

            //compute inteval on that index x listLinesRandom[0].Count - 1; with interval of size h_size
            SortedDictionary<Interval, int> intervalYR = getIntervalY(listLinesRandom, listLinesRandom[0].Count - 1, range_y / 60);
            SortedDictionary<Interval, int> intervalY2R = getIntervalY(listLinesRandom, (listLinesRandom[0].Count - 1) / 2, range_y / 60);
            SortedDictionary<Interval, int> intervalY3R = getIntervalY(listLinesRandom, (listLinesRandom[0].Count - 1) / 4, range_y / 60);


            //draw axis 
            initializeGraphics();
            HelpGraphics graph = new HelpGraphics();
            graph.draw_axis(g, viewPort, x_min, x_max, y_min, y_max);


        






            //trasform lines
            listLinesRandom = GetListOfLinesTrasformed(listLinesRandom);

            DrawLines(listLinesRandom);


            graph.drawHistogram_y(g, intervalYR, range_x, range_y, x_min, y_min, viewPort, listLinesRandom[0].Count - 1, 2);
            graph.drawHistogram_y(g, intervalY2R, range_x, range_y, x_min, y_min, viewPort, (listLinesRandom[0].Count - 1) / 2, 2);
            graph.drawHistogram_y(g, intervalY3R, range_x, range_y, x_min, y_min, viewPort, (listLinesRandom[0].Count - 1) / 4, 2);
            //graph.LadderY(g, 0, n, y_min,y_max, viewPort, 10);
            graph.LadderYRigth(g, 0, n, y_min, y_max, viewPort, 10, sigma);
            this.pictureBox1.Image = b;


        }






        public void initializeGraphics()
        {

            b = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        }






        //private void drawSmallerHistogram(Graphics g, Rectangle viewPort, SortedDictionary<Interval,int> intervals, double x_min, double y_min, double range_x, double range_y)
        //{
        //    HelpGraphics hg = new HelpGraphics();
        //    hg.draw_axis(g, viewPort, x_min, range_x - x_min, y_min, range_y - y_min);
        //    hg.rugPlot(g, x_min, range_x - x_min, y_min, range_y - y_min, viewPort, 10);
        //    hg.drawHistogram(g, intervals, viewPort, x_min, range_x, y_min, range_y, false);

        //}


        public List<List<PointF>> getListOfBrownian(int m, int n, double sigma)
        {
            List<List<PointF>> listaLinee = new List<List<PointF>>();
            Random ranGenerator = new Random();

            //for all lines m
            for (int index_listaLinee = 0; index_listaLinee < m; index_listaLinee++)
            {
                listaLinee.Add(new List<PointF>());

                //reset Y_past to 0 for each line
                double Y_past = 0;


                for (int i = 0; i < n; i++)
                {

                    // p must be 0<=p<=1 


                    //begginnig point is 0      
                    PointF point = new PointF(i,(float) Y_past);
                    listaLinee[index_listaLinee].Add(point);
                    // after we update beginning point with Bernoulli +1 or 0
                    // SHOULD I USE GLOBAL N INSTEAD (not clear)
                    Y_past += Brownianupdate(sigma, ranGenerator, n);




                }
            }

            return listaLinee;

        }


        //n_uptonow is total n instead
        private double Brownianupdate(double sigma, Random rangen, int n_tot)
        {


            // : σ* sqrt(1 / n) *Z(t),

            double sqrt = Math.Sqrt((double)1 / n_tot);
            return sigma * sqrt * generateGaussian(0, 1, rangen);

        }


        private static double spare;
        private static bool hasSpare = false;

        public static double generateGaussian(double mean, double stdDev, Random rangen)
        {
            if (hasSpare)
            {
                hasSpare = false;
                return spare * stdDev + mean;
            }
            else
            {
                double u, v, s;
                do
                {
                    u = rangen.NextDouble() * 2 - 1;
                    v = rangen.NextDouble() * 2 - 1;
                    s = u * u + v * v;
                } while (s >= 1 || s == 0);
                s = Math.Sqrt(-2.0 * Math.Log(s) / s);
                spare = v * s;
                hasSpare = true;
                return mean + stdDev * u * s;
            }
        }




        public List<List<PointF>> GetListOfLinesTrasformed(List<List<PointF>> lista_linee)
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

 
   
        public void DrawLines(List<List<PointF>> lista_linee)
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

       





        private SortedDictionary<Interval, int> getIntervalY(List<List<PointF>> ll, int slice_index_histogram, double h_size)
        {
            SortedDictionary<Interval, int> dictionary_Y = new SortedDictionary<Interval, int>();
            DictInter dizioInter = new DictInter();
            if (slice_index_histogram < ll[0].Count && slice_index_histogram >= 0)
            {
                // this part is for minimum and maximum
                //int min = 0;
                //int max = 0;
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




        private (int, int) getMinMax(List<List<PointF>> ll)
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
        private (int, int) getMinMax(List<List<int>> ll)
        {
            int min = 0;
            int max = 0;
            // find min max 
            for (int i = 0; i < ll.Count; i++)
            {
                for (int j = 0; j < ll[i].Count; j++)
                {
                    if (ll[i][j] > max)

                        max = (int)ll[i][j];
                    if (ll[i][j] < min)
                        min = (int)ll[i][j];
                }
            }
            return (min, max);
        }

    

    


    }
}
