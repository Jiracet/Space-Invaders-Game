/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Game that plays space invaders. Player uses space to fire and arrow keys to move. Goal is to shoot all of the invaders
 * in a wave; then, progressing to the next wave. Player has 3 lives.
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
    //enumerates directions
    public enum Direction { left, right, up, down };

    public partial class InvadersFrm : Form
    {
        public InvadersFrm()
        {
            InitializeComponent();
        }

        private bool twoPlayerMode = false;   //sets whether or not the game is in two player mode
        private string player1Name, player2Name;  //strings for the player 1 and 2 names for high scores
        private int score = 0, score2 = 0; //integers to keep scores

        //declares gameover variable which would end the game if true
        private bool gameOver = false;
        private int animationCell = 0, increment = 1, spriteSkip = 0;   //vars for animation cycling
        private bool handleGameOver = false;    //bool for handling game over event
        private bool isTitleMenu = true;     //bool for representing if the title page is active
        private bool isPauseMenu = false, isInstructionsMenu = false, isHighScoresMenu = false, isGameOverMenu = false; //represents whether or not which pages are active
        private bool onlyStars = true;   //bool which declares whether only stars will be drawn since game has not started

        private bool optionChanged = false;     //bool for determining whether the user changed the selected option
        private int selectedOption = 1;    //int for determining the currently selected option on menu screen

        //creates game object
        Game game = new Game();

        //defines all the keys that the user may press
        List<Keys> keysPressed = new List<Keys>();
        List<Keys> keysPressed2 = new List<Keys>();

        //creates objects for all menus
        CustomMenu allMenus = new CustomMenu();

        //creates object for drawing all pages
        PageManager pageDrawer = new PageManager();

        //creates highScore board
        TopList highScores = new TopList("invadersHighScores");

        //creates size that the form will lock onto
        Size lockSize;

        /* timer which controls the animations of sprites
           Postcondition: animates all sprites frame by frame  */
        private void animationTimer_Tick(object sender, EventArgs e)
        {
            if (InvadersFrm.ActiveForm == null)  //when window is not active, returns
                return;

            //increments the animation cells back and forth while starting at 0.
            spriteSkip += 1;   //increments spriteSkip which allows the cell to be a certain cell longer

            //every 3 cell skips, animationcell is incremeneted
            if (spriteSkip >= 5)
            {
                spriteSkip = 0;
                animationCell += increment;
            }

            //when animationCell increments higher or lower than limit, changes increment and reverts to previous animation cell
            if (animationCell >= 4 || animationCell <= -1)
            {
                increment *= -1;
                animationCell += increment * 2;
            }

            //when a menu is open, form is resizable and the maximum and minimum sizes are set
            if (isTitleMenu || isInstructionsMenu || isHighScoresMenu || isGameOverMenu)
            {
                lockSize = new Size(InvadersFrm.ActiveForm.Width, InvadersFrm.ActiveForm.Height);  //gets the size of the current form
                MaximumSize = DefaultMaximumSize;   //sets window size restrictions to default
                MinimumSize = new Size(800, 640);
                MaximizeBox = true;
            }
            else   //when menu is not open and game has commenced, the screen size is locked
            {
                MaximumSize = lockSize;   //the form size gets locked by setting the minimum and maximum sizes to form size
                MinimumSize = lockSize;
                MaximizeBox = false;   //disables ability to maximize screen
            }

            //pulls and adds 5 new stars on to screen
            game.Twinkle();

            //refreshes the screen
            Refresh();
        }

        /* timer which controls the game controls of the program
           Postcondition: moves game ahead one frame each tick  */
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (InvadersFrm.ActiveForm == null)  //when window is not active, returns
                return;

            //when key is pressed, check if left or right key is pressed. Move the player on screen accordingly.
            if (keysPressed.Count() >= 1)
            {
                switch (keysPressed[0])   //checks most recent key press
                {
                    case Keys.Left:    //moves left if left key
                        game.MovePlayer(Direction.left);
                        break;
                    case Keys.Right:   //moves right if right key
                        game.MovePlayer(Direction.right);
                        break;
                }
            }

            //tracks key presses for second player in new list
            if (keysPressed2.Count() >= 1)
            {
                switch (keysPressed2[0])  //checks most recent key press in second player list
                {
                    case Keys.A:   //moves left if 'a'
                        game.MovePlayer2(Direction.left);
                        break;
                    case Keys.D:   //moves rigth if 'd'
                        game.MovePlayer2(Direction.right);
                        break;
                }
            }

            //if game over is being handled, call GameOverHandlerfunction
            if (handleGameOver)
            {
                GameOverHandler(sender, e);
                return;
            }

            //advance the game through Game.Go
            game.Go(ref handleGameOver, twoPlayerMode, ref score, ref score2);
        }

        /* function which controls the paint event and draws all the images
           Postcondition: paints all images on screen  */
        private void InvadersFrm_Paint(object sender, PaintEventArgs e)
        {
            //declares graphics and draws the game
            Graphics g = e.Graphics;
            game.Draw(g, animationCell, onlyStars, twoPlayerMode);

            //if the title menu and instructions menu and high scores menu is not open, only stars is false
            if (!isTitleMenu && !isInstructionsMenu && !isHighScoresMenu)
            {
                onlyStars = false;

                //when save score button is enabled, set locations of text boxes, buttons and score saving instructions
                if (NameInput.Enabled)
                {
                    pageDrawer.DrawScoreScreen(g, twoPlayerMode);  //draws the instructions on the score recording screen

                    NameInput.Location = new Point(InvadersFrm.ActiveForm.Width / 2 - 137,
                        InvadersFrm.ActiveForm.Height / 2 + 45);  //sets location of text boxd
                    if (twoPlayerMode)
                        Name2Input.Location = new Point(InvadersFrm.ActiveForm.Width / 2 - 137, InvadersFrm.ActiveForm.Height / 2 + 74);
                    SaveScoreButton.Location = new Point(InvadersFrm.ActiveForm.Width / 2 + 104,
                        InvadersFrm.ActiveForm.Height / 2 + 45);  //sets location of button
                }
            }
            else
                onlyStars = true;

            //draws the current menu and page that is active
            if (isTitleMenu)
            {
                pageDrawer.DrawTitleScreen(g);   //draws title screen and menu
                allMenus.DrawMenu(g, selectedOption, InvadersFrm.ActiveForm.Width / 2 - 270, InvadersFrm.ActiveForm.Height / 2 - 78,
                    5, "One Player", "Two Player", "Instructions", "High Scores", "Exit Game", ref optionChanged);
            }
            else if (isPauseMenu)
            {
                pageDrawer.DrawPauseScreen(g);   //draws pause menu and page
                allMenus.DrawMenu(g, selectedOption, InvadersFrm.ActiveForm.Width / 2 - 270, InvadersFrm.ActiveForm.Height / 2 - 78,
                    4, "Resume", "Restart", "Back to Main", "Exit Game", "", ref optionChanged);
            }
            else if (isGameOverMenu)
            {    //draws game over menu
                allMenus.DrawMenu(g, selectedOption, InvadersFrm.ActiveForm.Width / 2 - 270, InvadersFrm.ActiveForm.Height / 2 + 50,
                    3, "Restart", "Back to Main", "Exit Game", "", "", ref optionChanged);
            }
            else if (isInstructionsMenu || isHighScoresMenu)
            {
                if (isInstructionsMenu)
                    pageDrawer.DrawInstructionsScreen(g);    //draws instructions page
                else
                    pageDrawer.DrawHighScoresScreen(g, highScores);    //draws high scores page

                allMenus.DrawMenu(g, selectedOption, InvadersFrm.ActiveForm.Width - 100, InvadersFrm.ActiveForm.Height - 110,
                    1, "Back", "", "", "", "", ref optionChanged);
            }
        }

        /* tracks all the key presses by user
           Postcondition: responds according to key presses by user  */
        private void InvadersFrm_KeyDown(object sender, KeyEventArgs e)
        {
            //when user presses 'q', exits program
            if (e.KeyCode == Keys.Q)
                Application.Exit();

            //checks if any other menu screen is open and if save score button is open
            if (!isTitleMenu && !isInstructionsMenu && !isHighScoresMenu && !isGameOverMenu && !NameInput.Enabled)
            {
                //if user presses 'p' during the game, it pauses game timer and displays pause menu
                if (e.KeyCode == Keys.P)
                {
                    //if pause menu is not active currently, turn it active, and vice versa
                    if (!isPauseMenu)
                    {
                        gameTimer.Stop();
                        isPauseMenu = true;
                    }
                    else
                    {
                        gameTimer.Start();
                        isPauseMenu = false;
                    }
                }

                if (!isPauseMenu)  //track game key input when pause menu is not open
                {
                    //when user presses space key, player fires a shot by calling FireShot function
                    if (!twoPlayerMode)
                    {
                        if (e.KeyCode == Keys.Space)   //in single player, player uses space to fire
                            game.FireShot(false);
                    }
                    else
                    {
                        if (e.KeyCode == Keys.Up)   //in two player, player one uses Up key to fire
                            game.FireShot(false);

                        if (e.KeyCode == Keys.W)   //second player uses W to fire
                            game.FireShot(true);
                    }

                    //removes and adds the key to have the most recent key in the list
                    if (keysPressed.Contains(Keys.Left) || keysPressed.Contains(Keys.Right))
                        keysPressed.Remove(e.KeyCode);  //removes previous key

                    if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                        keysPressed.Add(e.KeyCode);   //if pressing left or right, adds left the key to the list

                    if (twoPlayerMode)
                    {
                        if (keysPressed2.Contains(Keys.A) || keysPressed2.Contains(Keys.D))
                            keysPressed2.Remove(e.KeyCode);   //removes previous key for second player

                        if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
                            keysPressed2.Add(e.KeyCode);   //if pressing a or d, adds the respective key to player two list
                    }
                }
            }

            //in main menu, can toggle through options using up or down arrow keys
            if (isTitleMenu || isInstructionsMenu || isHighScoresMenu || isPauseMenu || isGameOverMenu)
            {
                int numOptions = 1;

                //declares how many options there will be depending on the page open
                if (isTitleMenu)
                    numOptions = 5;
                else if (isPauseMenu)
                    numOptions = 4;
                else if (isGameOverMenu)
                    numOptions = 3;
                //for highscore and instructions menu, selected option is automatically set to 1

                if (!isInstructionsMenu && !isHighScoresMenu)  //restriction so option is not toggled in instructions or high scores menu
                {
                    //when pressing down, the selected option increases and goes back to one when reached the limit
                    if (e.KeyCode == Keys.Down)
                    {
                        optionChanged = true;       //when pressing up or down on the menu, the option is changed

                        if (selectedOption == numOptions)  //if selection has reached the bottom, moves back to 1 at the top
                            selectedOption = 1;
                        else
                            selectedOption++;
                    }
                    else if (e.KeyCode == Keys.Up)    //inversely, when pressing up, option number decreases
                    {
                        optionChanged = true;

                        if (selectedOption == 1)  //if selection has reached the top, moves back to bottom at number of options
                            selectedOption = numOptions;
                        else
                            selectedOption--;
                    }
                }

                //when enter or space is pressed, the option in the menu is selected
                if (e.KeyCode == Keys.Space)
                {
                    optionChanged = true;      //when selecting into a new page, the option was changed

                    //determines which menu is open provides choices and functions accordingly
                    if (isTitleMenu)
                    {
                        isTitleMenu = false;    //since once button is clicked, it will not be on the same screen, so isTitleMenu is marked false

                        //when one player mode is selected, main menu timer stops and gametimer starts
                        if (selectedOption == 1 || selectedOption == 2)
                        {
                            if (selectedOption == 2)
                            {
                                //two player mode selection
                                twoPlayerMode = true;
                            }
                            else
                                twoPlayerMode = false;  //if not two player mode, resets back to single player

                            game.ResetAll(twoPlayerMode);   //resets game
                            gameTimer.Enabled = true;
                            gameTimer.Start();   //enables and starts game timer
                        }
                        else if (selectedOption == 3)
                            isInstructionsMenu = true;
                        else if (selectedOption == 4)
                            isHighScoresMenu = true;
                        else
                            Application.Exit();   //exits when exit game option is pressed
                    }
                    else if (isPauseMenu)
                    {
                        isPauseMenu = false;    //once pause menu is opened and a button is clicked, the game is not on the pause menu

                        if (selectedOption == 1)
                            gameTimer.Start();   //resumes the game by starting timer again
                        else if (selectedOption == 2)
                        {
                            //code to reset the game and restart timers
                            game.ResetAll(twoPlayerMode);   //resets game
                            gameTimer.Start();  //starts the game again
                        }
                        else if (selectedOption == 3)
                        {
                            //code to open title menu
                            isTitleMenu = true;
                        }
                        else if (selectedOption == 4)
                            Application.Exit();   //exits program
                    }
                    else if (isGameOverMenu)
                    {
                        isGameOverMenu = false;    //once an option on game over menu is pressed, game over screen is not open
                        gameOver = false;

                        if (selectedOption == 1)
                        {
                            game.ResetAll(twoPlayerMode);
                            gameTimer.Start();   //starts the game again
                        }
                        else if (selectedOption == 2)
                        {
                            //resets gameover and opens title menu
                            isTitleMenu = true;
                        }
                        else if (selectedOption == 3)
                            Application.Exit();   //exits program
                    }
                    else if (isInstructionsMenu || isHighScoresMenu)  //combines instructions and highscore menus since they are the same menus
                    {
                        isInstructionsMenu = false;
                        isHighScoresMenu = false;
                        isTitleMenu = true;
                    }
                    selectedOption = 1;   //resets the selected option back to 1 when new page is opened
                }
            }

            //if game is over, press 's' to restart.
            if (gameOver)
            {
                if (e.KeyCode == Keys.S)
                {
                    //code to reset the game and restart timers
                    game.ResetAll(twoPlayerMode);
                    isGameOverMenu = false;
                    gameOver = false;
                    gameTimer.Start();
                }
                return;
            }
        }

        /* when key is released, the key being pressed is removed from list
           Postcondition: removes keysPressed when key is lifted  */
        private void InvadersFrm_KeyUp(object sender, KeyEventArgs e)
        {
            if (keysPressed.Contains(Keys.Left) || keysPressed.Contains(Keys.Right))
                keysPressed.Remove(e.KeyCode);  //when left or right key is lifted, removes the key in list

            if (twoPlayerMode)
            {
                if (keysPressed2.Contains(Keys.A) || keysPressed2.Contains(Keys.D))
                    keysPressed2.Remove(e.KeyCode);    //when a or d is lifted, removes the key in player two list
            }
        }

        /* handles gameover event
           Postcondition: stops gameTimer and begins handling of gameOver event  */
        private void GameOverHandler(object sender, EventArgs e)
        {
            //clears all the keys on key presses
            keysPressed.Clear();
            keysPressed2.Clear();

            //stops gametimer, sets gameover to true, and refreshes screen
            handleGameOver = false;
            gameTimer.Stop();
            NameInput.Enabled = true;
            NameInput.Visible = true;
            if (twoPlayerMode)
            {
                Name2Input.Enabled = true;
                Name2Input.Visible = true;
            }
            SaveScoreButton.Enabled = true;
            SaveScoreButton.Visible = true;
            Refresh();
        }

        /* tracks input from the save score button
           Postcondition: Allows user to save scores with a username  */
        private void SaveScoreButton_Click(object sender, EventArgs e)
        {
            NameInput.TabIndex = 1;  //sets the tab index

            player1Name = NameInput.Text;   //selects name
            if (twoPlayerMode)
                player2Name = Name2Input.Text;

            //checks player 1's name if single player, and checks both player 1 and 2 if two player mode
            if ((player1Name != "" && !twoPlayerMode) || (player1Name != "" && player2Name != "" && twoPlayerMode))
            {
                highScores.AddItem(score, player1Name);
                NameInput.Clear();    //clears text in box and disables box
                NameInput.Enabled = false;
                NameInput.Visible = false;
            }
            else
                return;  //returns if no input

            if (twoPlayerMode)
            {
                if (player2Name != "" && player1Name != "")   //checks for blank names
                {
                    highScores.AddItem(score2, player2Name);
                    Name2Input.Clear();    //clears text in box and disables box
                    Name2Input.Enabled = false;
                    Name2Input.Visible = false;
                }
                else
                    return;   //returns if no input

                player2Name = "";    //resets name
            }
            player1Name = "";   //resets name

            //disables button and turns handleGameOver to false
            handleGameOver = false;
            SaveScoreButton.Enabled = false;
            SaveScoreButton.Visible = false;
            isGameOverMenu = true;    //opens game over menu
            gameOver = true;    //sets game over to true

            //gives the form focus
            InvadersFrm.ActiveForm.Focus();
        }
    }
}
