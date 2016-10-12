/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates class for drawing main menu screen and all pages
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
    class PageManager
    {
        //declares fonts and format for text drawing use
        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
        System.Drawing.Font titleFont = new System.Drawing.Font("Courier New", 50, FontStyle.Bold);
        System.Drawing.Font textFont = new System.Drawing.Font("Courier New", 12);

        /* draws the title screen
           Postcondition: Prints the space invaders title at middle of screen  */
        public void DrawTitleScreen(Graphics g)
        {
            //draws space invaders title
            g.DrawString("SPACE INVADERS", titleFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 300,
                InvadersFrm.ActiveForm.Height / 2 - 120, drawFormat);
        }

        /* draws the pause screen
           Postcondition: Prints paused message and how to unpause instruction  */
        public void DrawPauseScreen(Graphics g)
        {
            //draws paused title and unpause prompt
            g.DrawString("PAUSED", titleFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 150,
                InvadersFrm.ActiveForm.Height / 2 - 150, drawFormat);
            g.DrawString("Press 'p' to Unpause", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width - 230, InvadersFrm.ActiveForm.Height - 60);
        }

        /* draws the instruction screen
           Postcondition: Draws all of the instructions */
        public void DrawInstructionsScreen(Graphics g)
        {
            System.Drawing.Font subHeadingFont = new System.Drawing.Font("Courier New", 24, FontStyle.Bold);  //font for subheadings

            //draws the introduction
            g.DrawString("Introduction", subHeadingFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 150,
                InvadersFrm.ActiveForm.Height / 2 - 300);
            g.DrawString(@"
                     Welcome to space invaders! 
 Your mission is to save the planet from invaders falling from the sky.
  Destroy each wave of invaders to advance to the next one. Good luck!", textFont, Brushes.Yellow,
                            InvadersFrm.ActiveForm.Width / 2 - 360, InvadersFrm.ActiveForm.Height / 2 - 280);

            //draws the controls
            g.DrawString("Controls", subHeadingFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 110, InvadersFrm.ActiveForm.Height / 2 - 180);
            g.DrawString(@"
One Player Functions:               Space == Fire!     Arrow Keys == Move
Two Player Functions:   Player 1:   Up Key == Fire!    Arrow Keys == Move
                        Player 2:   W Key == Fire!     WASD Keys == Move
Additional Functions:               'q' == Quit Game   'p' == Pause Game
                                    's' == Restart Game", textFont, Brushes.Yellow,
                           InvadersFrm.ActiveForm.Width / 2 - 380, InvadersFrm.ActiveForm.Height / 2 - 160);

            //draws the points and power ups
            g.DrawString("Points and Powerups", subHeadingFont, Brushes.Yellow,
                InvadersFrm.ActiveForm.Width / 2 - 200, InvadersFrm.ActiveForm.Height / 2 - 30);

            //draws images to illustrate points system
            g.DrawImage(Properties.Resources.star1, InvadersFrm.ActiveForm.Width / 2 - 300, InvadersFrm.ActiveForm.Height / 2 - 10);
            g.DrawImage(Properties.Resources.spaceship1, InvadersFrm.ActiveForm.Width / 2 - 300, InvadersFrm.ActiveForm.Height / 2 + 40);
            g.DrawImage(Properties.Resources.flyingsaucer1, InvadersFrm.ActiveForm.Width / 2 - 300, InvadersFrm.ActiveForm.Height / 2 + 80);
            g.DrawImage(Properties.Resources.bug1, InvadersFrm.ActiveForm.Width / 2 - 300, InvadersFrm.ActiveForm.Height / 2 + 120);
            g.DrawImage(Properties.Resources.satellite1, InvadersFrm.ActiveForm.Width / 2 - 300, InvadersFrm.ActiveForm.Height / 2 + 170);
            g.DrawImage(Properties.Resources.watchit1, InvadersFrm.ActiveForm.Width / 2 - 300, InvadersFrm.ActiveForm.Height / 2 + 220);

            //draws points for each invader
            g.DrawString("Star == 10 Points", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 250, InvadersFrm.ActiveForm.Height / 2);
            g.DrawString("Spaceship == 20 Points", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 250, InvadersFrm.ActiveForm.Height / 2 + 50);
            g.DrawString("Flying Saucer == 30 Points", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 250, InvadersFrm.ActiveForm.Height / 2 + 90);
            g.DrawString("Bug == 40 Points", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 250, InvadersFrm.ActiveForm.Height / 2 + 140);
            g.DrawString("Satellite == 50 Points", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 250, InvadersFrm.ActiveForm.Height / 2 + 190);
            g.DrawString("Mothership == 250 Points", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 250, InvadersFrm.ActiveForm.Height / 2 + 240);

            //draws images to illustrate power ups
            g.DrawImage(Properties.Resources.numShotUp, new Rectangle(InvadersFrm.ActiveForm.Width / 2 + 50, InvadersFrm.ActiveForm.Height / 2 + 20,40,40));
            g.DrawImage(Properties.Resources.shotSpeedUp, new Rectangle(InvadersFrm.ActiveForm.Width / 2 + 50, InvadersFrm.ActiveForm.Height / 2 + 80, 40, 40));
            g.DrawImage(Properties.Resources.shipSpeedUp, new Rectangle(InvadersFrm.ActiveForm.Width / 2 + 50, InvadersFrm.ActiveForm.Height / 2 + 140, 40, 40));
            g.DrawImage(Properties.Resources.lifeUp, new Rectangle(InvadersFrm.ActiveForm.Width / 2 + 50, InvadersFrm.ActiveForm.Height / 2 + 200, 40, 40));

            g.DrawString("== Number of Shots Up", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 + 100, InvadersFrm.ActiveForm.Height / 2 + 30);
            g.DrawString("== Shot Speed Up", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 + 100, InvadersFrm.ActiveForm.Height / 2 + 90);
            g.DrawString("== Ship Speed Up", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 + 100, InvadersFrm.ActiveForm.Height / 2 + 150);
            g.DrawString("== Extra Life", textFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 + 100, InvadersFrm.ActiveForm.Height / 2 + 210);
        }

        /* draws the high score screen
           Postcondition: draws the high score page  */
        public void DrawHighScoresScreen(Graphics g, TopList highScores)
        {
            //fonts for the high score board
            System.Drawing.Font headerFont = new System.Drawing.Font("Courier New", 40, FontStyle.Bold);
            System.Drawing.Font listFont = new System.Drawing.Font("Courier New", 15);

            //draws high scores board header
            g.DrawString("HIGH SCORES BOARD", headerFont, Brushes.Yellow,
                InvadersFrm.ActiveForm.Width / 2 - 300, InvadersFrm.ActiveForm.Height / 2 - 250);

            //draws the highscore board
            highScores.display(g, listFont, InvadersFrm.ActiveForm.Width / 2 - 250, InvadersFrm.ActiveForm.Height / 2 - 150);
        }

        /* draws instructions for inputting rank names
           Postcondition: draws text box labels and error message if no input  */
        public void DrawScoreScreen(Graphics g, bool twoPlayerMode)
        {
            System.Drawing.Font promptFont = new System.Drawing.Font("Courier New", 10);

            g.DrawString("Enter Player 1 Name:", promptFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 310,
                InvadersFrm.ActiveForm.Height / 2 + 45);
            if (twoPlayerMode)
            {
                g.DrawString("Enter Player 2 Name:", promptFont, Brushes.Pink, InvadersFrm.ActiveForm.Width / 2 - 310,
                    InvadersFrm.ActiveForm.Height / 2 + 75);
            }
        }
    }
}
