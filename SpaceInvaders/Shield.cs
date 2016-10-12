/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates class for the shields which protect the player during the game
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
    class Shield
    {
        //declares variables for location, size, area
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Rectangle Area { get { return new Rectangle(Location, Size); } }

        //declares hitboxes for invaders and player. Large hitbox is needed since player may obtain speed shot up upgrades which may cause shots to skip over true area hitbox
        public Rectangle PlayerHitBox { get { return new Rectangle(Location.X, Location.Y + Size.Height - 100, 70, 100); } }
        public Rectangle InvaderHitBox { get { return new Rectangle(Location.X, Location.Y + Size.Height - 30, 70, 30); } }

        /*  creates type for each shield
            Postcondition: allows shields to be created with a specific location and area  */
        public Shield(Point Location, Size Size)
        {
            this.Location = Location;
            this.Size = Size;
        }

        /*  draws the shield on the screen
            Poscondition: draws a yellow rectangle for the shield at specified location with a specified size  */
        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Yellow, new Rectangle(Location, Size));
        }

        /*  changes the values of the shield when hit with a shot
            Poscondition: will subtract from the size and add to the locations y value when hit by an invader  */
        public void ShieldHit(bool invaderHit, int shieldDamage)
        {
            if (invaderHit)   //y value is added when hit by invader to look like the rectangle was hit downwards
                Location = Point.Add(Location, new Size(0, shieldDamage));

                Size = Size.Subtract(Size, new Size(0, shieldDamage));   //subtract from size
        }

        /*  tracks the shields height to notify if the shield has no height
            Postcondition: returns true if the height of the shield is 0  */
        public bool shieldDisappear()
        {
            if (Size.Height <= 0)  //when size of shield is less than or equal to zero, return true
                return true;
            else
                return false;
        }
    }
}
