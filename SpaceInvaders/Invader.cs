/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates class for a list of invaders
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
    public class Invader
    {
        //declares enumeration of type of invader
        public enum Type { star, spaceship, flyingsaucer, bug, satellite, watchit }

        //declares the amount each invader will move vertically and horizontally
        private const int HorizontalInterval = 10;
        private const int VerticalInterval = 40;

        private Bitmap image;

        //declares types for invader list
        public Point Location { get; set; }
        public Type InvaderType { get; private set; }
        public Rectangle Area { get { return new Rectangle(Location, image.Size); } }
        public int Score { get; private set; }

        /* creates constructor for each invader
           Precondition: type, location and score
           Postcondition: assigns type, location and score to invader elements when constructed  */
        public Invader(Type invaderType, Point location, int score)
        {
            this.InvaderType = invaderType;
            this.Location = location;
            this.Score = score;
            image = InvaderImage(0);
        }

        ///////////////////////////Private Methods//////////////////////////////

        /* moves the invader in the correct direction 
           Precondition: direction for invaders to move
           Postcondition: moves invaders in specified direction  */
        public void Move(Direction direction)
        {
            if (direction == Direction.left)   //according to the direction, and or subtract the horizontal interval from location's x value
                Location = Point.Subtract(Location, new Size(HorizontalInterval, 0));
            else if (direction == Direction.right)
                Location = Point.Add(Location, new Size(HorizontalInterval, 0));
            else
                Location = Point.Add(Location, new Size(0, VerticalInterval));  //when direction is down, add to vertical interval
        }

        /* draws the image with specified animationCell
           Precondition: valid graphics and animationCell number between 1 and 4
           Postcondition: draws the image at specified location at specified cell  */
        public void Draw(Graphics g, int animationCell)
        {
            //draws the image of the invader using correct animation cell
            InvaderImage(animationCell);
            g.DrawImage(image, Location);
        }

        /* creates directory to invader image
           Precondition: animationCell between 1 and 4. Proper image file in Resources
           Postcondition: returns invader image from file  */
        private Bitmap InvaderImage(int animationCell)
        {
            //returns bitmap of image for specified cell
            image = (Bitmap)Properties.Resources.ResourceManager.GetObject(InvaderType + (animationCell + 1).ToString());
            return image;
        }
    }
}
