/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates class for stars object
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpaceInvaders
{
    public class Stars
    {
        private List<Star> stars = new List<Star>();

        //creates struct for each star object
        public struct Star
        {
            public Point point;
            public Brush brush;
            public int starSize;

            public Star(Point point, Brush brush, int starSize)
            {
                this.point = point;
                this.brush = brush;
                this.starSize = starSize;
            }
        }

        /* Creates a specific number of stars inside boundaries using a random generator
           Precondition: numStars > 0
           Postcondition: Adds specific number of random stars inside boundaires  */
        public void CreateStar(Rectangle boundaries, Random randy, int numstars)
        {
            for (int i = 0; i < numstars; i++)
                stars.Add(new Star(new Point(randy.Next(0, boundaries.Width),randy.Next(0, boundaries.Height)), RandomBrush(randy), randy.Next(1, 5)));
        }

        /* draws each star using paint function
           Postcondition: draws every star in the star list  */
        public void Draw(Graphics g)
        {
            foreach (Star star in stars)
                g.FillRectangle(star.brush, star.point.X, star.point.Y, star.starSize, star.starSize);
        }

        /* removes five stars and adds five new stars
           Postcondition: 5 stars are removed and five new random stars with random sizes and random colors is added  */
        public void Twinkle(Random randy, Rectangle boundaries)
        {
            for (int i = 0; i < 5; i++)
                stars.RemoveAt(randy.Next(0, stars.Count()));

            CreateStar(boundaries, randy, 5);
        }

        /////////////////////PRIVATE METHODS////////////////////////////

        /* returns a brush with a random color
           Postcondition: A random brush out of 5 colors will be returned  */
        private Brush RandomBrush(Random randy)
        {
            //declares random brush number
            int brushNumber = randy.Next(0, 5);
            Brush randomBrush;

            //according to random number, assigns the brush one color out of five
            if (brushNumber == 0)
                randomBrush = Brushes.Red;
            else if (brushNumber == 1)
                randomBrush = Brushes.Blue;
            else if (brushNumber == 2)
                randomBrush = Brushes.Yellow;
            else if (brushNumber == 3)
                randomBrush = Brushes.Green;
            else
                randomBrush = Brushes.Cyan;

            //returns the random brush
            return randomBrush;
        }
    }
}
