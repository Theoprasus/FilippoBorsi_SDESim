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
    public partial class NewRandomProcess : Form
    {
        public NewRandomProcess()
        {
            InitializeComponent();
            this.textBoxLambda.Text = "50";
            this.textBoxN.Text = "1000";
            this.textBoxM.Text = "500";
            
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

            double lambda = 50;
            UserInput ui = new UserInput(); 
            
            if (ui.getDouble(this.textBoxLambda.Text, 1, n) != null)
                lambda = (double)ui.getDouble(this.textBoxLambda.Text, 1, n);


            // try to take input from user
            //if (this.textBoxLambda.Text != null && !string.IsNullOrEmpty(this.textBoxLambda.Text))
            //{
            //    string l_string = this.textBoxLambda.Text.Trim();
            //    double l_temp = 0;

            //    if (double.TryParse(l_string, out l_temp))
            //    {
            //       if (l_temp <= n && l_temp>= 0 )
            //           lambda = l_temp;
            //    }

            //}
            this.textBoxLambda.Text = lambda.ToString();










            List<List<PointF>> listLinesRandom = getListOfLinesBernoulliUpdate(m, n, lambda);





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


            //get list of distances jump-origin
            List<List<int>> listDistancesFromOrigin = GetListOfDistancesJumpsOrigin(listLinesRandom);
            // 10 is the number of histogram 
            SortedDictionary<Interval, int> intervalJumpsOrigin = getIntervalYForWholeMatrix(listDistancesFromOrigin,10);

            //-------------------------------------------------------------------------------------------------
            // now i should plot them same picture box is fine
            Rectangle viewPort2 = new Rectangle(50,  10+ 720 , 1200 / 4, 200);
            double y_min2 = 0;
            double range_y2 = 1;
            (int,int) tuple = getMinMax(listDistancesFromOrigin);
            double x_min2 = tuple.Item1;
            double range_x2 = tuple.Item2-tuple.Item1;
            graph.drawSmallerHistogram(g, viewPort2, intervalJumpsOrigin, x_min2, y_min2, range_x2, range_y2);



            //get list of distances jump-jump
            List<List<int>> listDistancesAMongJumps = GetListOfDistancesBetweenJumps(listDistancesFromOrigin);
            // 10 is the number of histogram 
            SortedDictionary<Interval, int> intervalJumpsJumps = getIntervalYForWholeMatrix(listDistancesAMongJumps, 10);

            // now i should plot them same picture box is fine
            Rectangle viewPort3 = new Rectangle(50+ 350 , 10 + 720, 1200 / 4, 200);
            double y_min3 = 0;
            double range_y3 = 1;
            (int, int) tuple2 = getMinMax(listDistancesAMongJumps);
            double x_min3 = tuple.Item1;
            double range_x3 = tuple2.Item2 - tuple2.Item1;
            graph.drawSmallerHistogram(g, viewPort3, intervalJumpsJumps, x_min3, y_min3, range_x3, range_y3);





            //trasform lines
            listLinesRandom = GetListOfLinesTrasformed(listLinesRandom);

            DrawLines(listLinesRandom);


            graph.drawHistogram_y(g, intervalYR, range_x, range_y, x_min, y_min, viewPort, listLinesRandom[0].Count - 1,2);
            graph.drawHistogram_y(g, intervalY2R, range_x, range_y, x_min, y_min, viewPort, (listLinesRandom[0].Count - 1) / 2,2);
            graph.drawHistogram_y(g, intervalY3R, range_x, range_y, x_min, y_min, viewPort, (listLinesRandom[0].Count - 1) / 4,2);

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


        public List<List<PointF>> getListOfLinesBernoulliUpdate(int m, int n, double lambda)
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
                    // after we update beginning point with Bernoulli +1 or 0
                    // SHOULD I USE GLOBAL N INSTEAD (not clear)
                    Y_past += BernoulliUpdate(lambda,ranGenerator,n);




                }
            }

            return listaLinee;

        }


        //n_uptonow is total n instead
        private int BernoulliUpdate(double lambda, Random rangen, int n_uptonow)
        {
            double ran_double = rangen.NextDouble();
            double p_success = lambda * ((double)1 / n_uptonow);
            if (ran_double <= p_success)
                return 1;
            
            else return 0;

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

        public List<List<int>> GetListOfDistancesJumpsOrigin(List<List<PointF>> lista_linee)
        {
            List<List<int>> lista_dist_jumps_origin = new List<List<int>>();
            HelpGraphics hg = new HelpGraphics();
            for (int i = 0; i < lista_linee.Count; i++)
            {
                List<int> list_temp_int = new List<int>();
                int last_value_y = 0;
                //List<PointF> lista_traf = new List<PointF>();
                for (int j = 0; j < lista_linee[i].Count; j++)
                {
                   
                    
                    int jump1 = (int) Math.Round(Convert.ToDouble(lista_linee[i][j].Y));
                    if (jump1 > last_value_y)
                    {
                        list_temp_int.Add((int)Math.Round(Convert.ToDouble(lista_linee[i][j].X)));
                        last_value_y = (int)Math.Round(Convert.ToDouble(lista_linee[i][j].Y));

                    }
                   


                }
                lista_dist_jumps_origin.Add(list_temp_int);


            }
            return lista_dist_jumps_origin;
        }
        public List<List<int>> GetListOfDistancesBetweenJumps(List<List<int>> lista_distances_from_origin)
        {
            List<List<int>> lista_dist_among_jumps = new List<List<int>>();

            for (int i = 0; i < lista_distances_from_origin.Count; i++)
            {
                List<int> list_temp_int = new List<int>();

                //List<PointF> lista_traf = new List<PointF>();
                if (lista_distances_from_origin.Count >= 2)
                {
                    for (int j = 0; j + 1 < lista_distances_from_origin[i].Count; j++)
                    {
                        list_temp_int.Add((int)(lista_distances_from_origin[i][j + 1] - lista_distances_from_origin[i][j]));

                    }


                    lista_dist_among_jumps.Add(list_temp_int);
                }
            }

            return lista_dist_among_jumps;
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

        private SortedDictionary<Interval, int> getIntervalY(List<List<PointF>> ll, int Nbernoulli, double h_size, double p, double epsilon)
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



        private SortedDictionary<Interval, int> getIntervalYForWholeMatrix(List<List<int>> listDistancesOrigin, int numberOfHisto)
        {
            SortedDictionary<Interval, int> dictionary_Y = new SortedDictionary<Interval, int>();
            DictInter dizioInter = new DictInter();
            //find ymin ymax
           (int,int) tuple =  getMinMax(listDistancesOrigin);

                for (int i = 0; i < listDistancesOrigin.Count; i++)
                {

                    for (int j = 0; j < listDistancesOrigin[i].Count(); j++)
                        {

                            dizioInter.UpdateDict(dictionary_Y, listDistancesOrigin[i][j], (double)(tuple.Item2 - tuple.Item1) / numberOfHisto);
                        }
                    
                }
            
            return dictionary_Y;
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
                    if (ll[i][j]> max)

                        max = (int)ll[i][j];
                    if (ll[i][j] < min)
                        min = (int)ll[i][j];
                }
            }
            return (min, max);
        }

        private int getMaxIndexatBernoulliN(List<List<PointF>> ll, int Nbernoulli)
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

        private int getIntervalCount(int n, List<List<PointF>> listLines, double p, double epsilon)
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
