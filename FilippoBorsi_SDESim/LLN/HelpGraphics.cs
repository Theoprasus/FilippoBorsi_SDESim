using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLN
{
    class HelpGraphics
    {



        public float transform_y(float world_y, Rectangle r, double min_y, double range_y)
        {
            return (r.Top + r.Height - ((world_y - (float)min_y) / (float)range_y) * r.Height);
        }
        public float transform_x(float world_x, Rectangle r, double min_x, double range_x)
        {
            return (r.Left + ((world_x - (float)min_x) / (float)range_x) * r.Width);
        }

        public float scaleLength(float length, Rectangle r, double min_x, double range_x)
        {
            float result = (length / (float)range_x) * r.Width;
            return result;
        }

        public void draw_axis(Graphics g, Rectangle viewPort, double x_min, double x_max, double y_min, double y_max)
        {

            g.DrawLine(Pens.Black, transform_x((float)x_min, viewPort, x_min, x_max - x_min), transform_y(0, viewPort, y_min, y_max - y_min), transform_x((float)x_max, viewPort, x_min, x_max - x_min), transform_y(0, viewPort, y_min, y_max - y_min));
            g.DrawLine(Pens.Black, transform_x((float)0, viewPort, x_min, x_max - x_min), transform_y((float)y_min, viewPort, y_min, y_max - y_min), transform_x((float)0, viewPort, x_min, x_max - x_min), transform_y((float)y_max, viewPort, y_min, y_max - y_min));

        }

        public PointF transformPointF(PointF punto, Graphics g, Rectangle viewPort, double x_min, double range_x, double y_min, double range_y)
        {
            return new PointF(transform_x(punto.X, viewPort, x_min, range_x), transform_y(punto.Y, viewPort, y_min, range_y));
        } 

        // Draw a rotated string at a particular position.
        public void DrawRotatedTextAt(Graphics gr, float angle,
            string txt, float x, float y, Font the_font, Brush the_brush)
        {
            // Save the graphics state.
            GraphicsState state = gr.Save();
            gr.ResetTransform();

            // Rotate.
            gr.RotateTransform(angle);

            // Translate to desired position. Be sure to append
            // the rotation so it occurs after the rotation.
            gr.TranslateTransform(x, y, MatrixOrder.Append);

            // Draw the text at the origin.
            gr.DrawString(txt, the_font, the_brush, 0, 0);

            // Restore the graphics state.
            gr.Restore(state);
        }






        public void rugPlot(Graphics g, double x_min, double x_max, double y_min, double y_max, Rectangle viewport, double rugNumber)

        {
            //bigger step => smaller interval 
            for (double i = x_min; i <= x_max; i += (x_max - x_min) / rugNumber)
            {
                g.DrawLine(Pens.Black, new PointF(transform_x((float)i, viewport, x_min, x_max - x_min), transform_y((float)((y_max - y_min) / -20), viewport, y_min, y_max - y_min)), new PointF(transform_x((float)i, viewport, x_min, x_max - x_min), transform_y((float)((y_max - y_min) / 20), viewport, y_min, y_max - y_min)));
                DrawRotatedTextAt(g, 0, (Math.Round(i,1)).ToString(), transform_x((float)i, viewport, x_min, x_max - x_min), transform_y(0, viewport, y_min, y_max - y_min), new Font("Calibri", 10, FontStyle.Italic, GraphicsUnit.Pixel), Brushes.Black);
            }


            for (double i = y_min; i <= y_max; i += (y_max - y_min) / rugNumber)
            {
                g.DrawLine(Pens.Black, new PointF(transform_x((float)((x_max - x_min) / -20), viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min)), new PointF(transform_x((float)((x_max - x_min) / 20), viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min)));
                DrawRotatedTextAt(g, 90, (Math.Round(i, 1)).ToString(), transform_x(0, viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min), new Font("Calibri", 10, FontStyle.Italic, GraphicsUnit.Pixel), Brushes.Black);
            }





        }


        public void LadderY(Graphics g, double x_min, double x_max, double y_min, double y_max, Rectangle viewport, double rugNumber)

        {
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            for (double i = y_min; i <= y_max; i += (y_max - y_min) / rugNumber)
            {
                g.DrawLine(Pens.Black, new PointF(transform_x((float)((x_max - x_min) / -20), viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min)), new PointF(transform_x((float)((x_max - x_min) / 20), viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min)));
                DrawRotatedTextAt(g, 90, (Math.Round(i, 1)).ToString(), transform_x(0, viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min), new Font("Calibri", 10, FontStyle.Italic, GraphicsUnit.Pixel), Brushes.Black);
            }





        }


        public void LadderYRigth(Graphics g, double x_min, double x_max, double y_min, double y_max, Rectangle viewport, double rugNumber, double sigma)

        {
            //
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            int shift = 67;

            for (double i = y_min; i <= y_max; i += (y_max - y_min) / rugNumber)
            {
                g.DrawLine(Pens.Gray, new PointF(transform_x((float)(x_max - (x_max- x_min) / 160 + shift), viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min)), new PointF(transform_x((float)(x_max - (x_max - x_min) / -160 +shift), viewport, x_min, x_max - x_min), transform_y((float)i, viewport, y_min, y_max - y_min)));
                DrawRotatedTextAt(g, 0, (Math.Round(i, 1)).ToString()+"  (" + Math.Round(i/sigma, 1).ToString() + " σ) ", transform_x((float)x_max +80, viewport, x_min, x_max - x_min), transform_y((float)i + (float)(y_max-y_min)/100, viewport, y_min, y_max - y_min), new Font("Calibri", 10, FontStyle.Italic, GraphicsUnit.Pixel), Brushes.Black);
            }

            // per cambiare  direzione - o + dopo il pèrimo x_max
            g.DrawLine(Pens.Gray, transform_x((float)(x_max - (x_max - x_min) / 160 + 80), viewport, x_min, x_max - x_min), transform_y((float)y_min, viewport, y_min, y_max - y_min), transform_x((float)(x_max - (x_max - x_min) / 160 + 80), viewport, x_min, x_max - x_min), transform_y((float)y_max, viewport, y_min, y_max - y_min));




        }


        //utils 
        public int getTotalCountDict(SortedDictionary<Interval, int> dict)
        {
            int totale = 0;
            foreach (KeyValuePair<Interval, int> kvp in dict)
            {
                totale += kvp.Key.count;
            }
            return totale;
        }


        public void drawHistogram(Graphics g, SortedDictionary<Interval, int> dictDistributions_x, Rectangle viewPort, double x_min, double range_x, double y_min, double range_y, bool MeanLine, double heigthDivisor)
        {
         

            foreach (KeyValuePair<Interval, int> pair in dictDistributions_x)
            {
                if (pair.Key.count != 0)
                {
                    float frequency = (float)pair.Key.count / getTotalCountDict(dictDistributions_x);
                    //frequency depends on range y/2
                    float heigth = (float)(frequency * range_y / heigthDivisor); //range_y/2
                    //heigth = (float)(heigth-y_min)/(float)range_y * viewPort.Height; //- transform_y(0, viewPort, y_min, range_y);
                    heigth = (float)(heigth - 0) / (float)range_y * viewPort.Height;
                    //width is basically the step of interval
                    float width = (float)(pair.Key.upperBound - pair.Key.lowerBound);
                    //genius finally you got it rigth !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
                    //width = (float)(width - x_min) / (float)range_x * (float)viewPort.Width;
                    width = (float)(width - 0) / (float)range_x * (float)viewPort.Width;

                    //x_cord
                    float x_coord = transform_x((float)pair.Key.lowerBound, viewPort, x_min, range_x);
                    float y_coord = transform_y((float)(frequency * range_y / heigthDivisor), viewPort, y_min, range_y); //heigth;//transform_y((float)heigth, viewPort, y_min, range_y); //- transform_y(0, viewPort, y_min, range_y);


                    RectangleF[] lista_rect = new RectangleF[1];
                    RectangleF rect = new RectangleF(x_coord, y_coord, width, heigth);
                    Brush brush = new SolidBrush(Color.FromArgb(100, 0, 255, 255));
                    g.FillRectangle(brush, rect);
                    lista_rect[0] = rect;
                    g.DrawRectangles(Pens.Blue, lista_rect);

                    if (MeanLine) 
                    { 
                    PointF point1 = new PointF(transform_x((float)(pair.Key.mean), viewPort, x_min, range_x), transform_y((float)0, viewPort, y_min, range_y));
                    PointF point2 = new PointF(transform_x((float)(pair.Key.mean), viewPort, x_min, range_x), transform_y((float)0 + (float)range_y, viewPort, y_min, range_y));
                    g.DrawLine(Pens.Cyan, point1, point2);
                    }
                }



            }
        }
        // shift must be scaled before
        public void drawHistogram_y(Graphics g, SortedDictionary<Interval, int> dictDistributions_y, double range_x, double range_y, double x_min, double y_min, Rectangle viewPort, float shift, double heigthDivisor)
        {
            GraphicsState state = g.Save();
            foreach (KeyValuePair<Interval, int> pair in dictDistributions_y)
            {
                if (pair.Key.count != 0)
                {
                    float frequency = (float)pair.Key.count / getTotalCountDict(dictDistributions_y);
                    //frequency depends on range y/2
                    //float heigth = frequency * (float)range_x / 2;
                    float heigth = (float)(frequency * range_x /heigthDivisor ); // float heigth = (float)(frequency * range_x /heigthDivisor ); //range_x/2
                    heigth = (float)(heigth - 0) / (float)range_x * viewPort.Width;
                    //width is basically the step of interval
                    float width = (float)(pair.Key.upperBound - pair.Key.lowerBound);
                    //genius finally you got it rigth !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
                    width = (float)(width - 0) / (float)range_y * viewPort.Height;

                    //y_cord
                    float y_coord = transform_y((float)pair.Key.upperBound, viewPort, y_min, range_y);
                    float x_coord = transform_x((float)(frequency * range_x/ heigthDivisor ), viewPort, x_min, range_x);
                    x_coord = x_coord + scaleLength(shift,viewPort,x_min, range_x);




                    Matrix myMatrix = new Matrix();

                    myMatrix.RotateAt(90, new PointF(x_coord, y_coord), MatrixOrder.Append);


                    g.Transform = myMatrix;
                    //g.DrawRectangle(Pens.BlueViolet, x_coord, y_coord, (float)width, (float)heigth);
                    RectangleF[] lista_rect = new RectangleF[1];
                    RectangleF rect = new RectangleF(x_coord, y_coord, width, heigth);
                    Brush brush = new SolidBrush(Color.FromArgb(190, 255, 165, 0));
                    g.FillRectangle(brush, rect);
                    lista_rect[0] = rect;

                    g.DrawRectangles(Pens.Orange, lista_rect);


                }

            }
            g.Restore(state);


            //foreach (KeyValuePair<Interval, int> pair in dictDistributions_y)
            //{
            //    if (pair.Key.count != 0)
            //    {

            //        PointF point1 = new PointF(transform_x((float)0, viewPort, x_min, range_x), transform_y((float)(pair.Key.mean), viewPort, y_min, range_y));
            //        PointF point2 = new PointF(transform_x((float)(x_min + range_x), viewPort, x_min, range_x), transform_y((float)(pair.Key.mean), viewPort, y_min, range_y));
            //        g.DrawLine(Pens.LightSalmon, point1, point2);
            //    }
            //}

        }
        public void draw_Line_trasformed(Graphics g, Rectangle viewPort, double x_min, double x_max, double y_min, double y_max, float x1, float y1, float x2, float y2)
        {

            g.DrawLine(Pens.Black, transform_x(x1, viewPort, x_min, x_max - x_min), transform_y(y1, viewPort, y_min, y_max - y_min), transform_x(x2, viewPort, x_min, x_max - x_min), transform_y(y2, viewPort, y_min, y_max - y_min));



        }



        public PointF[] GetArrayOfPointsTrasformed(Graphics g, Rectangle viewPort, PointF[] array_punti, double min_x, double range_x, double min_y, double range_y)
        {
            PointF[] listaTrasformed = new PointF[array_punti.Length];
            for (int i = 0; i < array_punti.Length; i++)
            {
                //List<PointF> lista_traf = new List<PointF>();

                {
                    listaTrasformed[i] = transformPointF(array_punti[i], g, viewPort, min_x, range_x, min_y, range_y);
                }


            }
            return listaTrasformed;
        }

        public void drawZigZAgLine(Graphics g, PointF[] array_punti_trasformed, Pen p)
        {
            for (int i = 0; i < array_punti_trasformed.Length - 1; i++)
            {
                g.DrawLine(p, array_punti_trasformed[i].X, array_punti_trasformed[i].Y, array_punti_trasformed[i + 1].X, array_punti_trasformed[i].Y);
                g.DrawLine(p, array_punti_trasformed[i + 1].X, array_punti_trasformed[i].Y, array_punti_trasformed[i + 1].X, array_punti_trasformed[i + 1].Y);
            }
        }


        public void drawSmallerHistogram(Graphics g, Rectangle viewPort, SortedDictionary<Interval, int> intervals, double x_min, double y_min, double range_x, double range_y)
        {
          
           draw_axis(g, viewPort, x_min, range_x - x_min, y_min, range_y - y_min);
           rugPlot(g, x_min, range_x - x_min, y_min, range_y - y_min, viewPort, 10);
           drawHistogram(g, intervals, viewPort, x_min, range_x, y_min, range_y, false, 1);

        }


    }
}
