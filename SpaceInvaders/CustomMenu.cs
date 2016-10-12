/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates an object menu which can be customized with certain options and functions
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
    class CustomMenu
    {
        private int selectedBtnHeight = 0, selectedBtnLocationOffset = 0;   //ints for height and location offsets of the button

        System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();

        /* this function will draw a menu with a max of five options
           Precondition: numOption <= 5
           Postcondition: Draws a menu that animates the currently selected option  */
        public void DrawMenu(Graphics g, int selectedOption, int xLocation, int yLocation, int numOptions,
            string option1, string option2, string option3, string option4, string option5, ref bool optionChanged)
        {
            System.Drawing.Font optionsFont = new System.Drawing.Font("Courier New", 20);

            //tracks change in selected options
            if (optionChanged)
            {
                //resets button rectangle properties when selected option switches
                selectedBtnHeight = 0;
                selectedBtnLocationOffset = 0;
                optionChanged = false;
            }

            //draws all of the options using a for loop
            for (int i = 1; i <= numOptions; i++)
            {
                //defines text that each option will state using the for loop
                string optionText;

                if (i == 1)
                    optionText = option1;
                else if (i == 2)
                    optionText = option2;
                else if (i == 3)
                    optionText = option3;
                else if (i == 4)
                    optionText = option4;
                else
                    optionText = option5;

                //if the option is selected, a box will appear behind the text with an animation, else it will print the option text normally
                if (selectedOption == i)
                {
                    g.FillRectangle(Brushes.Yellow, xLocation, yLocation + i * 50 - selectedBtnLocationOffset, 210, selectedBtnHeight);
                    g.DrawString(optionText, optionsFont, Brushes.Black, xLocation, yLocation - 17 + i * 50, drawFormat);
                }
                else
                    g.DrawString(optionText, optionsFont, Brushes.Yellow, xLocation, yLocation - 17 + i * 50, drawFormat);
            }

            //increments the button height and locations to perform an animation
            if (selectedBtnHeight < 30)
            {
                selectedBtnHeight += 4;
                selectedBtnLocationOffset += 2;
            }
        }
    }
}
