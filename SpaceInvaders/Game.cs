/* Name: Joseph Suh
 * Date: Nov. 6 2012
 * Description: Creates class for the game functions which
 * controls the main game.
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
using System.Media;

namespace SpaceInvaders
{
    public class Game
    {
        //multipurpose timer for causing delay times during certain animations
        Stopwatch timer = new Stopwatch();
        Stopwatch timer2 = new Stopwatch();
        Stopwatch nextWaveTimer = new Stopwatch();  //timer for next wave delay
        Stopwatch invincibilityTimer = new Stopwatch();  //timer for invincibility length
        Stopwatch invincibilityTimer2 = new Stopwatch();

        //creates sounds to play during the game
        SoundPlayer shoot = new SoundPlayer(Properties.Resources.shoot);
        SoundPlayer invaderkilled = new SoundPlayer(Properties.Resources.invaderkilled);
        SoundPlayer explosion = new SoundPlayer(Properties.Resources.explosion);

        //declares private variables for score, lives, wave, and frames skipped
        private int score = 0, score2 = 0;
        private int livesLeft = 3, livesLeft2 = 3;
        private int wave = 0;
        private int framesSkipped = 0;    //frames skipped for animations

        //declares variables for powerups
        private int shotSpeedUp = 0;
        private int shipSpeedUp = 0;
        private int numShotUp = 0;

        private int shotSpeedUp2 = 0;
        private int shipSpeedUp2 = 0;
        private int numShotUp2 = 0;

        //defines boundaries and random generator
        private Rectangle boundaries;  //value changes in paint function
        private Random randy = new Random();

        //declares direction for invaders to go and list of all invaders
        private Direction invaderDirection = new Direction();
        private List<Invader> invaders = new List<Invader>();
        private Invader motherShip;
        private bool motherShipOnScreen = false;

        //creates list for all shots, playerships, powerups and shields
        private PlayerShip playerShip;
        private PlayerShip playerShip2;
        private List<Shot> playerShots = new List<Shot>();
        private List<Shot> playerShots2 = new List<Shot>();
        private List<Shot> invaderShots = new List<Shot>();
        private List<Powerup> powerUps = new List<Powerup>();

        private List<Shield> shields = new List<Shield>();
        private int shieldDamage = 1;   //determines the amount of damage shots will do on the shield

        //creates object for stars and a bool for initialization for stars
        private Stars stars = new Stars();
        private bool initialization = false;
        private bool initializePlayers = false; //bool which determines if player's location should be initialized

        private bool invincibilityFrames = false, invincibilityFrames2 = false; //bool to determine if player is invincible
        private bool invincibilityFlicker = false;  //bool which oscillates to cause players to flicker when invincible
        private int invincibilityFrameCounter = 0;  //counter which slows down the invincibility flicker oscillation

        //shows exactly when wave is changed
        private bool waveChanged = false;

        /* this function draws all of the sprites and background in their current animation frame
           Precondition: graphics and valid animationCell number between and including 1 and 4  */
        public void Draw(Graphics g, int animationCell, bool onlyStars, bool twoPlayerMode)
        {
            //draws black background
            boundaries = new Rectangle(0, 0, InvadersFrm.ActiveForm.Width, InvadersFrm.ActiveForm.Height);   //refreshes boundaries
            g.FillRectangle(Brushes.Black, boundaries);

            //draws all the stars
            stars.Draw(g);

            //declares fontstyles and format
            System.Drawing.Font statsFont = new System.Drawing.Font("Arial", 16);
            System.Drawing.Font gameOverFont = new System.Drawing.Font("Courier New", 64, FontStyle.Bold);
            System.Drawing.Font exitFont = new System.Drawing.Font("Courier New", 12);
            System.Drawing.Font scoreFont = new System.Drawing.Font("Courier New", 28);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();

            //does not draw game related data when onlyStars is true
            if (!onlyStars)
            {
                //draws every invader, player, shot, powerup and shield
                foreach (Shot shot in playerShots)
                    shot.Draw(g);
                foreach (Shot shot2 in playerShots2)
                    shot2.Draw(g);
                foreach (Shot shot in invaderShots)
                    shot.Draw(g);
                foreach (Shield shield in shields)
                    shield.Draw(g);
                foreach (Powerup powerUp in powerUps)
                    powerUp.Draw(g);
                foreach (Invader invader in invaders)
                    invader.Draw(g, animationCell);
                if (motherShip != null)
                    motherShip.Draw(g, animationCell);  //draws mothership when not null

                if (waveChanged) //when wave is changed, displays wave number
                {
                    if (wave != 0)   //does not display wave when zero for a split frame
                        g.DrawString("Wave: " + wave, gameOverFont, Brushes.Cyan, InvadersFrm.ActiveForm.Width / 2 - 200,
                        InvadersFrm.ActiveForm.Height / 2 - 100, drawFormat);
                }

                //prints the score and wave
                g.DrawString("Score: " + score.ToString(), statsFont, Brushes.Yellow, 110, 0, drawFormat);
                g.DrawString("Wave: " + wave.ToString(), statsFont, Brushes.Blue, 0, 0, drawFormat);

                //draws ships on screen for each life left
                if (livesLeft > 3)
                    g.DrawString(livesLeft.ToString(), statsFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width - 170, 0, drawFormat);
                if (livesLeft >= 3)
                    g.DrawImage((Bitmap)Properties.Resources.player, InvadersFrm.ActiveForm.Width - 150, 0);
                if (livesLeft >= 2)
                    g.DrawImage((Bitmap)Properties.Resources.player, InvadersFrm.ActiveForm.Width - 80, 0);
                if (livesLeft <= 0)
                {
                    if (livesLeft2 <= 0 || !twoPlayerMode)  //when player 2 is also dead, draws game over
                    {
                        //prints game over and instructions
                        g.DrawString("GAME OVER", gameOverFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 250, 
                            InvadersFrm.ActiveForm.Height / 2 - 140, drawFormat);
                        g.DrawString("Press 'S' to start a new game or 'Q' to quit.", exitFont, Brushes.Yellow,
                                InvadersFrm.ActiveForm.Width - 480, InvadersFrm.ActiveForm.Height - 60, drawFormat);
                    }
                    if (!twoPlayerMode)
                        g.DrawString("Final Score: " + score, scoreFont, Brushes.Yellow, 
                            InvadersFrm.ActiveForm.Width / 2 - 180, InvadersFrm.ActiveForm.Height / 2 - 55, 
                            drawFormat);  //if not two player, draws final score
                }
                
                //when an invincibility frames bool is true, oscillates the invincibility flicker value to cause flickering
                if (invincibilityFrames || invincibilityFrames2)
                {
                    invincibilityFrameCounter++;  //increments invincibility frame counter

                    if (invincibilityFrameCounter >= 4)  //after five frames, invincibilityFlicker is changed
                    {
                        if (invincibilityFlicker)
                            invincibilityFlicker = false;
                        else
                            invincibilityFlicker = true;

                        invincibilityFrameCounter = 0;   //resets invincibility frame counter
                    }
                }
                else
                    invincibilityFlicker = false;   //if invincibility frames is false, invincibilityFlicker is turned false

                if (twoPlayerMode)
                {
                    if (!invincibilityFlicker || !invincibilityFrames2)  //causes flickering when invincibilityFlicker begins oscillating
                        playerShip2.Draw(g);   //draws second player ship

                    //prints the score
                    g.DrawString("Score: " + score2.ToString(), statsFont, Brushes.Pink, 300, 0, drawFormat);

                    //draws ships on screen for each life left
                    if (livesLeft2 > 3)
                        g.DrawString(livesLeft2.ToString(), statsFont, Brushes.Pink, InvadersFrm.ActiveForm.Width - 310, 0, drawFormat);
                    if (livesLeft2 >= 3)
                        g.DrawImage((Bitmap)Properties.Resources.player2, new Rectangle(InvadersFrm.ActiveForm.Width - 290, 0, 54, 33));
                    if (livesLeft2 >= 2)
                        g.DrawImage((Bitmap)Properties.Resources.player2, new Rectangle(InvadersFrm.ActiveForm.Width - 220, 0, 54, 33));
                    if (livesLeft2 <= 0 && livesLeft <= 0)
                    {
                        //prints final scores
                        g.DrawString("Player 1: " + score, scoreFont, Brushes.Yellow, InvadersFrm.ActiveForm.Width / 2 - 170, 
                            InvadersFrm.ActiveForm.Height / 2 - 55, drawFormat);
                        g.DrawString("Player 2: " + score2, scoreFont, Brushes.Pink, InvadersFrm.ActiveForm.Width / 2 - 170, 
                            InvadersFrm.ActiveForm.Height / 2 - 15, drawFormat);
                    }
                }
                if (!invincibilityFlicker || !invincibilityFrames)
                    playerShip.Draw(g);    //draws player ship in front of player 2 ship
            }
        }

        /* draws twinkling animation for stars in background
           Postcondition: draws random stars of random colors and causes them to twinkle  */
        public void Twinkle()
        {
            //initializes stars by drawing 300 of them
            if (!initialization)
            {
                stars.CreateStar(boundaries, randy, 300);
                initialization = true;
            }

            //calls the stars twinkle function
            stars.Twinkle(randy, boundaries);
        }

        /* moves the player according to key presses
           Postcondition: moves playerShip in correct direction  */
        public void MovePlayer(Direction direction)
        {
            if (playerShip.Alive)
                playerShip.Move(direction, shipSpeedUp);
        }

        /* moves the second player according to key presses
           Postcondition: moves playerShip2 in correct direction  */
        public void MovePlayer2(Direction direction)
        {
            if (playerShip2.Alive)
                playerShip2.Move(direction, shipSpeedUp2);
        }

        /* function for player to fire a shot
           Postcondition: if number of player shots < 2, add player shot at playerShip location  */
        public void FireShot(bool twoPlayerMode)
        {
            if (!waveChanged)   //checks if wave number is not being displayed
            {
                if (!twoPlayerMode)   //adds shot for player 1
                {
                    if (playerShots.Count < 2 + numShotUp && playerShip.Alive)   //numShotUp powerup is added on top of standard num shots
                    {
                        playerShots.Add(new Shot(new Point(playerShip.Location.X + 27, playerShip.Location.Y), Direction.up, boundaries, Brushes.Yellow));
                        shoot.Play();
                    }
                }
                else   //adds shot for player 2
                {
                    if (playerShots2.Count < 2 + numShotUp2 && playerShip2.Alive)
                    {
                        playerShots2.Add(new Shot(new Point(playerShip2.Location.X + 27, playerShip2.Location.Y), Direction.up, boundaries, Brushes.Pink));
                        shoot.Play();
                    }
                }
            }
        }

        /* function which initiates game functions
           Postcondition: moves the game ahead one frame  */
        public void Go(ref bool handleGameOver, bool twoPlayerMode, ref int sendScore, ref int sendScore2)
        {
            //initializes the players
            if (!initializePlayers)
            {
                if (!twoPlayerMode)   //player 1 is located differently depending on mode
                    playerShip = new PlayerShip(new Point(InvadersFrm.ActiveForm.Width / 2 - 27, InvadersFrm.ActiveForm.Height - 80), true, false);
                else
                    playerShip = new PlayerShip(new Point(InvadersFrm.ActiveForm.Width / 2 + 73, InvadersFrm.ActiveForm.Height - 80), true, false);
                playerShip2 = new PlayerShip(new Point(InvadersFrm.ActiveForm.Width / 2 - 127, InvadersFrm.ActiveForm.Height - 80), true, true);

                initializePlayers = true;  //sets to true so initialization will not repeat
            }

            //if no invaders left, call NextWave function
            if (invaders.Count == 0)
            {
                NextWave();

                if (!nextWaveTimer.IsRunning)
                {
                    waveChanged = true;   //changes wave
                    nextWaveTimer.Start();
                }
            }

            if (waveChanged)   //checks if the wave number is being displayed
            {
                if (nextWaveTimer.ElapsedMilliseconds >= 3000)
                {
                    waveChanged = false;   //after 3 seconds, game starts again
                    nextWaveTimer.Stop();
                    nextWaveTimer.Reset();
                }
                else
                    return;     //returns if 3 seconds have not passed
            }

            //checks if invincibility frames is true
            if (invincibilityFrames)
            {
                if (!invincibilityTimer.IsRunning)  //starts invincibility timer if not started already
                    invincibilityTimer.Start();

                if (invincibilityTimer.ElapsedMilliseconds >= 3000)
                {
                    invincibilityFrames = false;  //after 3 seconds, invincibility frames is turned false
                    invincibilityTimer.Stop();   //stops and resets the invincibility timer
                    invincibilityTimer.Reset();
                }
            }

            //checks invincibility frames for player 2
            if (invincibilityFrames2)
            {
                if (!invincibilityTimer2.IsRunning)
                    invincibilityTimer2.Start();

                if (invincibilityTimer2.ElapsedMilliseconds >= 3000)
                {
                    invincibilityFrames2 = false;
                    invincibilityTimer2.Stop();
                    invincibilityTimer2.Reset();
                }
            }

            //checks if player is dead
            if (!playerShip.Alive)
            {
                //turns player back alive if extra lives are available
                if (livesLeft > 0)
                {
                    if (!timer.IsRunning)
                        timer.Start();

                    //waits 3 seconds to show dying animation.
                    if (timer.ElapsedMilliseconds >= 3000)
                    {
                        //when brought back to life, powerup effects are lost
                        shotSpeedUp = 0;
                        shipSpeedUp = 0;
                        numShotUp = 0;

                        playerShip.Alive = true;   //sets player ship alive back to true
                        timer.Stop();
                        timer.Reset();

                        invincibilityFrames = true;  //invincibility frames are activated
                    }
                }
            }

            //handles game over for second player
            if (twoPlayerMode)
            {
                if (!playerShip2.Alive)
                {
                    if (livesLeft2 > 0)
                    {
                        if (!timer2.IsRunning)
                            timer2.Start();

                        //waits 3 seconds to show dying animation.
                        if (timer2.ElapsedMilliseconds >= 3000)
                        {
                            //when second player brought back to life, powerup effects are lost
                            shotSpeedUp2 = 0;
                            shipSpeedUp2 = 0;
                            numShotUp2 = 0;

                            playerShip2.Alive = true;
                            timer2.Stop();
                            timer2.Reset();

                            invincibilityFrames2 = true; //invincibility frames are activated
                        }
                    }
                }
            }

            //if no lives left, game is over
            if (livesLeft <= 0)
            {
                sendScore = score;   //sends score back to form

                if (twoPlayerMode && livesLeft2 <= 0)
                {
                    //turns handleGameOver true, which will eventually pass into gameTimer which will call the game over function
                    handleGameOver = true;
                    sendScore2 = score2;   //sends the score2 back to form
                }
                if (!twoPlayerMode)
                    handleGameOver = true;
            }

            //updates the shot locations
            ReturnFire();  //call return fire function which chooses invader to shoot

            //removes the powerups that are offscreen
            IEnumerable<Powerup> removePowerUp = powerUps.Where(powerUp => !powerUp.Move(1));  //the powerups move at 1 pixel per frame
            powerUps = powerUps.Except(removePowerUp).ToList();

            //moves location of shots and removes shots out of bounds
            IEnumerable<Shot> removeInvaderShot = invaderShots.Where(invaderShot => !invaderShot.Move(0));  //checks if shot is out of bounds
            invaderShots = invaderShots.Except(removeInvaderShot).ToList();

            IEnumerable<Shot> removePlayerShot = playerShots.Where(playerShot => !playerShot.Move(shotSpeedUp)); //checks if shot is out of bounds
            playerShots = playerShots.Except(removePlayerShot).ToList();

            //moves location of second player shots
            IEnumerable<Shot> removePlayerShot2 = playerShots2.Where(playerShot2 => !playerShot2.Move(shotSpeedUp2));
            playerShots2 = playerShots2.Except(removePlayerShot2).ToList();

            if (motherShip != null)  //when mothership is not null
            {
                //moves the location of the mothership
                motherShip.Location = Point.Subtract(motherShip.Location, new Size(5, 0));
                if (motherShip.Location.X <= -50)
                {
                    motherShip = null;   //when mothership has gone past left border, mothership becomes null
                    motherShipOnScreen = false;
                }
            }
            else if (randy.Next(0, 1000) > 998)  //creates a possibility for a mothership to be created
            {
                motherShip = new Invader(Invader.Type.watchit,
                    new Point(InvadersFrm.ActiveForm.Width + 30, 20), 250);
                motherShipOnScreen = true;
            }

            //updates the location of each invader on the screen
            MoveInvaders(twoPlayerMode);

            //if game over is being handled, returns
            if (handleGameOver)
                return;

            //check for collisions of shots to invader, player or powerup
            CheckForShieldCollisions(twoPlayerMode);
            CheckForPowerUpCollisions(twoPlayerMode);
            CheckForInvaderCollisions(twoPlayerMode);
            CheckForPlayerCollisions(twoPlayerMode);
        }

        /* resets all the game variables
           Postcondition: resets most of all game variables  */
        public void ResetAll(bool twoPlayerMode)
        {
            //resets all game elements
            score = 0;
            livesLeft = 3;
            wave = 0;
            framesSkipped = 0;
            invaders.Clear();
            nextWaveTimer.Stop(); nextWaveTimer.Reset();  //resets wave timer
            invincibilityTimer.Stop(); invincibilityTimer.Reset();  //resets invincibility timer
            timer.Stop(); timer.Reset();
            if (!twoPlayerMode)
                playerShip = new PlayerShip(new Point(InvadersFrm.ActiveForm.Width / 2 - 27, InvadersFrm.ActiveForm.Height - 80), true, false);
            invaderShots.Clear();
            playerShots.Clear();
            powerUps.Clear();
            shields.Clear();
            motherShip = null;
            motherShipOnScreen = false;
            invaderDirection = Direction.right;

            shotSpeedUp = 0;
            shipSpeedUp = 0;
            numShotUp = 0;

            invincibilityFrames = false;

            if (twoPlayerMode)
            {
                score2 = 0;
                livesLeft2 = 3;
                invincibilityTimer2.Stop(); invincibilityTimer2.Reset();  //resets invincibility timers
                timer2.Stop(); timer2.Reset();
                playerShip = new PlayerShip(new Point(InvadersFrm.ActiveForm.Width / 2 + 73, InvadersFrm.ActiveForm.Height - 80), true, false);
                playerShip2 = new PlayerShip(new Point(InvadersFrm.ActiveForm.Width / 2 - 127, InvadersFrm.ActiveForm.Height - 80), true, true);
                playerShots2.Clear();

                shotSpeedUp2 = 0;
                shipSpeedUp2 = 0;
                numShotUp2 = 0;

                invincibilityFrames2 = false;
            }
        }

        ///////////////Private Methods////////////////////

        /* when no invaders on screen, set up next wave of invaders
           Precondition: no invaders in invader list
           Postcondition: adds 30 invaders, assigns direction, adds one to wave, and clears all shots  */
        private void NextWave()
        {
            //sets invader wave properties
            int invaderColumnWidth = 80, invaderRowWidth = 60;
            int invaderStartingX = InvadersFrm.ActiveForm.Width / 2 - 300, invaderStartingY = 40;

            //assigns all invaders with specified types, locations, and scores
            for (int j = 1; j != 7; j++)
            {
                invaders.Add(new Invader((Invader.Type.star), new Point(invaderStartingX + j * invaderColumnWidth, invaderStartingY + invaderRowWidth * 4), 10));
                invaders.Add(new Invader((Invader.Type.spaceship), new Point(invaderStartingX + j * invaderColumnWidth, invaderStartingY + invaderRowWidth * 3), 20));
                invaders.Add(new Invader((Invader.Type.flyingsaucer), new Point(invaderStartingX + j * invaderColumnWidth, invaderStartingY + invaderRowWidth * 2), 30));
                invaders.Add(new Invader((Invader.Type.bug), new Point(invaderStartingX + j * invaderColumnWidth, invaderStartingY + invaderRowWidth), 40));
                invaders.Add(new Invader((Invader.Type.satellite), new Point(invaderStartingX + j * invaderColumnWidth, invaderStartingY), 50));
            }

            //clears all shields
            shields.Clear();

            //every 4 waves, the shield damage will increase by 1
            shieldDamage = 1 + (int)(wave / 4);

            //adds four shields which have a width of 70 and height of 20 - the wave number
            for (int i = 0; i < 4; i++)
            {
                shields.Add(new Shield(new Point (InvadersFrm.ActiveForm.Width / 6 + i * InvadersFrm.ActiveForm.Width / 5, 
                    InvadersFrm.ActiveForm.Height - 150), new Size(70, 20)));
            }

            //clears all shots on screen
            playerShots.Clear();
            playerShots2.Clear();
            invaderShots.Clear();
            powerUps.Clear();

            //clears the mothership
            motherShip = null;
            motherShipOnScreen = false;

            //assigns initial directon of invaders
            invaderDirection = Direction.right;

            wave++; //adds 1 to wave number
        }

        /* checks if the player or invader shot has hit the shield
           Postcondition: checks for shield collisions while deteriorating the shield and removing any shots collided with it  */
        private void CheckForShieldCollisions(bool twoPlayerMode)
        {
            //for each shield, checks for collisions with shots
            for (int i = 0; i < shields.Count(); i++)
            {
                //checks every invader shot
                for (int j = 0; j < invaderShots.Count(); j++)
                {
                    if (shields[i].InvaderHitBox.Contains(invaderShots[j].Location))   //checks if player shot is within powerup area
                    {
                        //tracks the shield hit going downwards
                        shields[i].ShieldHit(true, shieldDamage);
                        invaderShots.Remove(invaderShots[j]);   //remove player shot
                        break;     //exits loop for if one shot has hit powerup
                    }
                }

                //checks every player shot
                for (int j = 0; j < playerShots.Count(); j++)
                {
                    if (shields[i].PlayerHitBox.Contains(playerShots[j].Location))   //checks if player shot is within powerup area
                    {
                        //tracks the shield hit going upwards
                        shields[i].ShieldHit(false, shieldDamage);
                        playerShots.Remove(playerShots[j]);   //remove player shot
                        break;     //exits loop for if one shot has hit powerup
                    }
                }

                //checks shots from player 2
                if (twoPlayerMode)
                {
                    for (int j = 0; j < playerShots2.Count(); j++)
                    {
                        if (shields[i].PlayerHitBox.Contains(playerShots2[j].Location))
                        {
                            shields[i].ShieldHit(false, shieldDamage);
                            playerShots2.Remove(playerShots2[j]);   //remove player 2 shot
                            break;
                        }
                    }
                }
            }

            //removes all shields that have no size left
            IEnumerable<Shield> removeShield = shields.Where(shield => shield.shieldDisappear());
            shields = shields.Except(removeShield).ToList();
        }

        /* checks if a powerup has collided with player's shot
           Postcondition: applies effect to the players stats and removes powerup  */
        private void CheckForPowerUpCollisions(bool twoPlayerMode)
        {
            bool collisionTrue = false, collisionTrue2 = false;

            //for each powerup, checks for collision with player shots
            for (int i = 0; i < powerUps.Count(); i++)
            {
                //checks every player shot
                for (int j = 0; j < playerShots.Count(); j++)
                {
                    if (powerUps[i].Area.Contains(playerShots[j].Location))   //checks if player shot is within powerup area
                    {
                        playerShots.Remove(playerShots[j]);   //remove player shot
                        collisionTrue = true;
                        break;     //exits loop for if one shot has hit powerup
                    }
                }

                if (twoPlayerMode)
                {
                    for (int j = 0; j < playerShots2.Count(); j++)
                    {
                        if (powerUps[i].Area.Contains(playerShots2[j].Location))
                        {
                            playerShots2.Remove(playerShots2[j]);   //remove player 2 shot
                            collisionTrue2 = true;
                            break;
                        }
                    }
                }

                if (collisionTrue && collisionTrue2)   //if both players shots hit, powerup effect is distributed randomly
                {
                    if (randy.Next(0, 1) == 1)
                        AssignPowerUp(i, false);   //not second player, so bool is false
                    else
                        AssignPowerUp(i, true);   //is second player, so bool is true
                }
                else if (collisionTrue)   //if first player hit, apply effect to first player
                    AssignPowerUp(i, false);
                else if (collisionTrue2)  //if second player hit, apply effect to second player
                    AssignPowerUp(i, true);

                //when either collisiontrues are true, remove the power up
                if (collisionTrue || collisionTrue2)
                {
                    powerUps.Remove(powerUps[i]);

                    //sets collision values to false
                    collisionTrue = false;
                    collisionTrue2 = false;
                }
            }
        }

        /*  assigns powerup effects to specified player
            Precondition: valid powerup index needed
            Postcondition: assigns powerup to player according to powerup type  */
        private void AssignPowerUp(int index, bool secondPlayer)   //bool secondplayer used to determine which player gets the powerup
        {
            if (!secondPlayer)   //if first player, assign powerup to player 1 stats
            {
                if (powerUps[index].PowerUpType == Powerup.powerUpType.lifeUp)
                    livesLeft++;
                else if (powerUps[index].PowerUpType == Powerup.powerUpType.numShotUp)
                    numShotUp++;
                else if (powerUps[index].PowerUpType == Powerup.powerUpType.shotSpeedUp)
                    shotSpeedUp++;
                else if (powerUps[index].PowerUpType == Powerup.powerUpType.shipSpeedUp)
                    shipSpeedUp++;
            }
            else  //else, assign powerup to player 2 stats
            {
                if (powerUps[index].PowerUpType == Powerup.powerUpType.lifeUp)
                    livesLeft2++;
                else if (powerUps[index].PowerUpType == Powerup.powerUpType.numShotUp)
                    numShotUp2++;
                else if (powerUps[index].PowerUpType == Powerup.powerUpType.shotSpeedUp)
                    shotSpeedUp2++;
                else if (powerUps[index].PowerUpType == Powerup.powerUpType.shipSpeedUp)
                    shipSpeedUp2++;
            }
        }

        /* checks if player has collided with invader's shot
           Postcondition: removes life, sets playerShip.Alive to false and removes collided invader shot  */
        private void CheckForPlayerCollisions(bool twoPlayerMode)
        {
            bool firstCollided = false;   //if player one is hit, waits until after player 2 is checked until shot is removed

            //checks each invader shot for player collision
            for (int i = 0; i < invaderShots.Count(); i++)
            {
                if (playerShip.Alive)   //player must be alive to be shot
                {
                    if (!invincibilityFrames)  //if not in invincibility frames
                    {
                        if (playerShip.Area.Contains(invaderShots[i].Location))
                        {
                            //if hit, player ship alive property becomes false and loses one life
                            playerShip.Alive = false;
                            livesLeft--;
                            explosion.Play();
                            firstCollided = true;
                        }
                    }
                }

                if (twoPlayerMode)  //two playermode must be true for player 2 to be shot
                {
                    if (playerShip2.Alive)   //if second player is alive
                    {
                        if (!invincibilityFrames2)  //if second player does not have invincibility frames
                        {
                            if (playerShip2.Area.Contains(invaderShots[i].Location))
                            {
                                //if hit, player ship alive property becomes false and loses one life
                                playerShip2.Alive = false;
                                livesLeft2--;
                                explosion.Play();
                                if (!firstCollided)
                                    invaderShots.Remove(invaderShots[i]);  //if player1 has already been hit by same shot, don't remove shot yet
                            }
                        }
                    }
                }

                if (firstCollided)
                {
                    invaderShots.Remove(invaderShots[i]);  //removes shot after player2 has been checked
                    firstCollided = false;
                }
            }
        }

        /* checks all invaders if collided with player shot
           Precondition: number of invaders > 0
           Postcondition: removes all invaders hit along with their corresponding player shots. Adds to score when invader is hit  */
        private void CheckForInvaderCollisions(bool twoPlayerMode)
        {
            bool collisionTrue = false, collisionTrue2 = false;   //bool for determining if shot has collided with invader for each player
            bool motherShipCollision = false, motherShipCollision2 = false;

            //checks for every invader
            for (int i = 0; i < invaders.Count; i++)
            {
                //checks for every player 1 shot
                for (int j = 0; j < playerShots.Count; j++)
                {      //if a playershot is within the area of an invader, remove invader and shot
                    if (invaders[i].Area.Contains(playerShots[j].Location))
                    {
                        playerShots.Remove(playerShots[j]);   //removes player shot
                        collisionTrue = true;
                        break;
                    }
                }

                if (twoPlayerMode)
                {
                    //checks for every player 2 shot
                    for (int j = 0; j < playerShots2.Count; j++)
                    {      //if a playershot is within the area of an invader, remove invader and shot
                        if (invaders[i].Area.Contains(playerShots2[j].Location))
                        {
                            playerShots2.Remove(playerShots2[j]);   //removes player shot
                            collisionTrue2 = true;
                            break;
                        }
                    }
                }

                //if both players shots hit, score is distributed randomly
                if (collisionTrue && collisionTrue2)
                {
                    if (randy.Next(0, 2) == 1)
                        score += invaders[i].Score;
                    else
                        score2 += invaders[i].Score;
                }
                else if (collisionTrue)   //if first player hit, add to score
                    score += invaders[i].Score;
                else if (collisionTrue2)  //if second player hit, add to score2
                    score2 += invaders[i].Score;

                //when either collisiontrues are true, remove the current invader, create a powerup and set to false
                if (collisionTrue || collisionTrue2)
                {
                    createPowerUp(new Point(invaders[i].Location.X + 5, invaders[i].Location.Y + 10));
                    invaders.Remove(invaders[i]);
                    invaderkilled.Play();

                    collisionTrue = false;
                    collisionTrue2 = false;
                }
            }

            if (motherShipOnScreen)  //if the mothership is on screen
            {
                for (int j = 0; j < playerShots.Count; j++)
                {
                    if (motherShip.Area.Contains(playerShots[j].Location))
                    {
                        playerShots.Remove(playerShots[j]);   //removes player shot
                        motherShipCollision = true;
                        break;
                    }
                }

                for (int j = 0; j < playerShots2.Count; j++)
                {
                    if (motherShip.Area.Contains(playerShots2[j].Location))
                    {
                        playerShots2.Remove(playerShots2[j]);   //removes player shot
                        motherShipCollision2 = true;
                        break;
                    }
                }

                //if both collisions are true, points distributed to random player
                if (motherShipCollision && motherShipCollision2)
                {
                    if (randy.Next(0, 2) == 1)
                        score += motherShip.Score;
                    else
                        score2 += motherShip.Score;
                }
                else if (motherShipCollision)
                    score += motherShip.Score;
                else if (motherShipCollision2)
                    score2 += motherShip.Score;

                if (motherShipCollision || motherShipCollision2)  //when mothership collisions are true
                {
                    motherShip = null;
                    invaderkilled.Play();

                    motherShipOnScreen = false;  //sets mother ship on screen to false
                    motherShipCollision = false;
                    motherShipCollision2 = false;
                }
            }
        }

        /* function that moves the invader in the correct direction while checking if one invader collides with wall
           Precondition: number of invaders > 0
           Postcondition: moves all invaders to the right, left or down or skips a frame of moving  */
        private void MoveInvaders(bool twoPlayerMode)
        {
            //declares variables for border collision and off screen check
            bool borderCollision = false, offScreen = false;
            Direction invaderNextDirection = Direction.left; //sets the next direction of the invader

            //increments framesSkipped by 1 and when framesSkipped is under 20, returns function.
            framesSkipped += 1;
            if (framesSkipped <= 20)
                return;

            //depending on wave, number of framesSkipped will increase, causing the invaders to move faster
            framesSkipped = wave;

            //checks if any invader's X location is near the boundaries
            for (int i = 0; i < invaders.Count; i++)
            {
                //checks if invaders has moved past the border due to form resize
                if (invaders[i].Location.X >= InvadersFrm.ActiveForm.Width - 90)
                {
                    offScreen = true;
                    borderCollision = true;
                    break;
                }

                //checks if any invader has hit the left or right border
                if (invaders[i].Location.X >= InvadersFrm.ActiveForm.Width - 100 || invaders[i].Location.X <= 50)
                {
                    borderCollision = true;
                    break;
                }

                //checks if any invader hits bottom of screen
                if (invaders[i].Location.Y > InvadersFrm.ActiveForm.Height - 60)
                {
                    livesLeft = 0;
                    playerShip.Alive = false;
                    if (twoPlayerMode)
                    {
                        livesLeft2 = 0;
                        playerShip2.Alive = false;
                    }
                }
            }

            //groups the invaders by columns
            var sideInvaders = from invader in invaders
                               group invader by invader.Location.X into invaderColumns
                               select invaderColumns;

            if (offScreen)
            {
                //offset distance is equal to the x location of the farthest right invader to the border collision x location
                int offsetDistance = sideInvaders.Last().Last().Location.X - (InvadersFrm.ActiveForm.Width - 100);

                //offsets the x location of every invader by offset distance
                foreach (Invader invader in invaders)
                    invader.Location = new Point(invader.Location.X - offsetDistance, invader.Location.Y);
            }

            //if border collision is true, invaders first move down, then change direction following invaderNextDirection
            if (borderCollision)
            {
                //if the rightmost invader is on right side of screen, the next direction is left
                if (sideInvaders.Last().Last().Location.X >= InvadersFrm.ActiveForm.Width - 200)
                    invaderNextDirection = Direction.left;
                else if (sideInvaders.First().Last().Location.X < 200) //if leftmost invader is on left side, the next direction is right
                    invaderNextDirection = Direction.right;

                if (invaderDirection != Direction.down)   //if the direction is not down, and there is a border collision, move invaders down
                    invaderDirection = Direction.down;
                else
                    invaderDirection = invaderNextDirection;   //else specifies invader's new direction
            }
            else
            {
                if (invaderDirection == Direction.down) 
                    invaderDirection = invaderNextDirection;  //error handling when direction is down while border collision is false
            }

            //moves invaders down and to the opposite direction when collided with border
            foreach (Invader invader in invaders)
                invader.Move(invaderDirection);
        }

        /* controls and adds random shots made by the invaders
           Postcondition: invaders randomly return fire to player  */
        private void ReturnFire()
        {
            //limits the amount of shots and sets a random number resistor so invaders are not always shooting
            if (invaderShots.Count >= wave || (randy.Next(50) < 50 - wave))
                return;

            //query expression to group invaders by columns
            var frontLines = from invader in invaders
                             group invader by invader.Location.X into invaderColumns
                             select invaderColumns;

            //restricts amount of shots depending on how many lines of invaders are left so one invader is not left shooting as much as a whole army
            if (randy.Next(1, 6) > frontLines.Count())
                return;

            //generates random number between zero and number of groups for choosing random column
            int randomInvader = randy.Next(0, frontLines.Count());

            //adds the invader shot at the location of the selected invader in a random column at bottom row
            invaderShots.Add(new Shot(new Point(frontLines.ElementAtOrDefault(randomInvader).
                First().Location.X + (int)(frontLines.ElementAtOrDefault(randomInvader).First().Area.Width / 2),
                frontLines.ElementAtOrDefault(randomInvader).First().Location.Y +
                (int)(frontLines.ElementAtOrDefault(randomInvader).First().Area.Height / 2)), Direction.down, boundaries, Brushes.White));
        }

        /* generates a powerup with a certain possibility
           Postcondition: may add a new powerup to the list  */
        private void createPowerUp(Point Location)
        {
            if (invaders.Count() != 1)
            {
                if (randy.Next(0, 200) > 185)  //if life powerup is generated, gives a possibility for another power up to appear
                {
                    int randomNumber = randy.Next(0, 2);  // generates random number

                    if (randomNumber == 0)  //creates powerup depending on powerup
                        powerUps.Add(new Powerup(Location, Powerup.powerUpType.shipSpeedUp, boundaries));
                    else
                        powerUps.Add(new Powerup(Location, Powerup.powerUpType.shotSpeedUp, boundaries));
                }
                else if (randy.Next(0, 200) > 195)   //life  and numshot powerups are rarer, so randy.Next is set to a lower probability
                {
                    int randomNumber = randy.Next(0, 2);  //generates random number

                    if (randomNumber == 0)  //creates powerup depending on random number
                        powerUps.Add(new Powerup(Location, Powerup.powerUpType.lifeUp, boundaries));
                    else
                        powerUps.Add(new Powerup(Location, Powerup.powerUpType.numShotUp, boundaries));
                }
            }
        }
    }
}

