/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: This class will generate all the powerups and draw them on screen
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
using System.Threading;
using System.Diagnostics;

namespace SpaceInvaders
{
    class Powerup
    {
        //enum for each type of power up
        public enum powerUpType { lifeUp, numShotUp, shotSpeedUp, shipSpeedUp }

        private Bitmap image;   //image bitmap

        //declares types for powerup list
        public powerUpType PowerUpType { get; private set; }
        public Point Location { get; set; }
        private Rectangle boundaries;
        public Rectangle Area { get { return new Rectangle(Location, image.Size); } }

        /* Creates type for powerups
           Postcondition: Allows powerups to be defined by a location  */
        public Powerup(Point location, powerUpType powerUpType, Rectangle boundaries)
        {
            this.PowerUpType = powerUpType;
            this.Location = location;
            this.boundaries = boundaries;

            //image is selected from the type of powerup received
            image = (Bitmap)Properties.Resources.ResourceManager.GetObject(powerUpType.ToString());
        }

        /* moves the powerup down the screen
           Postcondition: moves the powerups location down the screen  */
        public bool Move(int powerUpSpeed)
        {
            bool withinBounds = true;  //bool to check if powerup is on screen

            Location = Point.Add(Location, new Size(0, powerUpSpeed));

            //checks if shot is out of boundaries
            if (!boundaries.Contains(Location))
                withinBounds = false;

            return withinBounds;
        }

        public void Draw(Graphics g)
        {
            //draws image at specified location
            g.DrawImage(image, new Rectangle(Location.X,Location.Y,40,40));
        }
    }
}
