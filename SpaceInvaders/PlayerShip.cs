/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates class for the player ship.
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
    public class PlayerShip
    {
        //declares variables for location, area, speed, image and alive state
        private int shipSpeed = 7;
        private int deadShipHeight = 33;
        private int deadShipLocation = 0;
        private Bitmap image;

        public Point Location { get; set; }
        public Rectangle Area { get { return new Rectangle(Location, image.Size); } }
        public bool Alive { get; set; }

        /* creates type for playerShip
           Postcondition: allows playerShip to have location and alive parameters  */
        public PlayerShip(Point location, bool alive, bool secondPlayer)
        {
            this.Location = location;
            this.Alive = alive;
            if (!secondPlayer)
                image = (Bitmap)Properties.Resources.player;
            else
                image = (Bitmap)Properties.Resources.player2;
        }

        /* function which draws player ship
           Postcondition: draws playerShip and crushes playerShip when not alive  */
        public void Draw(Graphics g)
        {
            //checks if the player is past the control width boundaries due to resizing
            if (Location.X > InvadersFrm.ActiveForm.Width - 70)
                Location = new Point(InvadersFrm.ActiveForm.Width - 70, Location.Y);  //sets location to 20 pixels before right border

            //sets player's y location to 80 pixels above bottom of screen
            Location = new Point(Location.X, InvadersFrm.ActiveForm.Height - 80);

            if (Alive)
            {
                //reset the deadShipHeight field and draw the ship
                deadShipHeight = 33;
                deadShipLocation = 0;
                g.DrawImage(image, new Rectangle(Location.X, Location.Y, 54, 33));
            }
            else
            {
                //check deadShipHeight field. If greater than 0, decrease by 1 and use DrawImage to draw ship
                if (deadShipHeight > 0)
                {
                    deadShipHeight -= 1;
                    deadShipLocation += 1;
                    g.DrawImage(image, Location.X, Location.Y + deadShipLocation, image.Size.Width, deadShipHeight);
                }
            }
        }

        /* moves the player ship in a certain direction
           Postcondition: moves player ship in specified direction if not going past the boundaries  */
        public void Move(Direction direction, int shipSpeedUp)
        {
            if (Alive)
            {
                //checks if ship is not on right edge of window
                if (!Area.Contains(new Point(5, InvadersFrm.ActiveForm.Height - 80)))
                {
                    if (direction == Direction.left)
                        Location = Point.Subtract(Location, new Size(shipSpeed + shipSpeedUp, 0));   //ship speed powerup is added on top of ship speed
                }
                //checks if ship is not on left ledge of window
                if (!Area.Contains(new Point(InvadersFrm.ActiveForm.Width - 20, InvadersFrm.ActiveForm.Height - 80)))
                {
                    if (direction == Direction.right)
                        Location = Point.Add(Location, new Size(shipSpeed + shipSpeedUp, 0));
                }
            }
        }
    }
}
