/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates class for all the shots on field.
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
    public class Shot
    {
        //declares constants for shot speed, width and height
        private const int moveInterval = 10;
        private const int width = 2;
        private const int height = 15;

        public Point Location { get; private set; }

        private Direction direction;
        private Rectangle boundaries;
        private Brush shotColor;

        /* constructor for player and invader shots
           Postcondition: Provides a way to store data in each shot  */
        public Shot(Point location, Direction direction, Rectangle boundaries, Brush shotColor)
        {
            this.Location = location;
            this.direction = direction;
            this.boundaries = boundaries;
            this.shotColor = shotColor;
        }

        /* draws all shots on screen
           Postcondition: draws the shot at specified location  */
        public void Draw(Graphics g)
        {
            g.FillRectangle(shotColor, Location.X, Location.Y, width, height);
        }

        /* moves all shots on screen
           Postcondition: moves shots up or down and returns false when shot is not within bounds  */
        public bool Move(int shotSpeedUp)
        {
            bool withinBounds = true;

            //moves the position of shot up or down
            if (direction == Direction.up)
                Location = Point.Subtract(Location, new Size(0, moveInterval + shotSpeedUp)); //shot speed up is added on top of move interval
            else
                Location = Point.Add(Location, new Size(0, moveInterval + shotSpeedUp));

            //checks if shot is out of boundaries
            if (!boundaries.Contains(Location))
                withinBounds = false;

            return withinBounds;
        }
    }
}
