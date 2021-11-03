using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0
        /// </summary>
        /// <param name="backgroundHeight">The backgroundHeight of the game play window.</param>
        /// <param name="backgroundWidth">The backgroundWidth of the game play window.</param>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;
        }

        #endregion

        #region Data members

        private const int MaxTimeBetweenPlayerBullets = 10;
        private const int MaxLives = 3;
        private const int PlayerBulletAmount = 3;
        private const double PlayerShipBottomOffset = 30;
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private int timeBetweenPlayerBullets;
        private int lives;

        private int score;

        public int Score
        {
            get => this.score;
            set
            {
                this.score = this.Score;
                this.scoreChanged(this.score);
            }
        }

        private DispatcherTimer timer;
        private Random random;

        private IList<Bullet> playerBullets;
        private Bullet enemyBullet;

        private PlayerShip playerShip;

        private EnemyManager enemyManager;

        private TextBlock pointsDisplay;
        private TextBlock messageDisplay;

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the game placing player ship and enemy ship in the game.
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="background">The background canvas.</param>
        public void InitializeGame(Canvas background)
        {
            if (background == null)
            {
                throw new ArgumentNullException(nameof(background));
            }

            this.enemyManager = new EnemyManager(background, this.backgroundHeight, this.backgroundWidth);

            this.score = 0;
            this.lives = MaxLives;
            this.timeBetweenPlayerBullets = MaxTimeBetweenPlayerBullets;

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            this.timer.Start();

            this.random = new Random();

            this.createAndPlacePlayerShip(background);

            this.pointsDisplay = new TextBlock();
            this.pointsDisplay.Text = this.score.ToString();
            this.pointsDisplay.Foreground = new SolidColorBrush(Colors.White);
            background.Children.Add(this.pointsDisplay);
            Canvas.SetLeft(this.pointsDisplay, 25);

            this.messageDisplay = new TextBlock();
            this.messageDisplay.Visibility = Visibility.Collapsed;
            this.messageDisplay.Foreground = new SolidColorBrush(Colors.White);
            background.Children.Add(this.messageDisplay);
            Canvas.SetTop(this.messageDisplay, this.backgroundHeight / 2);
            Canvas.SetLeft(this.messageDisplay, this.backgroundWidth / 3);

            this.playerBullets = new List<Bullet>();

            for(int i = 0; i < PlayerBulletAmount; i++)
            {
                this.playerBullets.Add(new Bullet(true));
                background.Children.Add(this.playerBullets[this.playerBullets.Count - 1].Sprite);
                this.playerBullets[this.playerBullets.Count - 1].Sprite.Visibility = Visibility.Collapsed;
            }

            this.enemyBullet = new Bullet(false);
            background.Children.Add(this.enemyBullet.Sprite);
            this.enemyBullet.Sprite.Visibility = Visibility.Collapsed;
        }

        private void timerTick(object sender, object e)
        {
            this.timeBetweenPlayerBullets++;
            this.movePlayerBullets();

            this.enemyManager.OnTick();
            this.didPlayerBulletHitEnemy();

            if (!this.enemyManager.EnemiesRemain())
            {
                this.gameOver(true);
            }

            if (this.enemyManager.EnemyBullet.CheckForCollision(this.playerShip) != null)
            {
                this.playerHit();
            }
        }

        private void didPlayerBulletHitEnemy()
        {
            foreach(Bullet bullet in this.playerBullets)
            {
                this.scoreChanged(this.enemyManager.DidPlayerBulletHitEnemy(bullet));
            }
        }
        private void movePlayerBullets()
        {
            foreach(var bullet in this.playerBullets)
            {
                if(bullet.Sprite.Visibility == Visibility.Visible)
                {
                    bullet.MoveUp();
                }

                if (bullet.Y <= 0)
                {
                    bullet.Sprite.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void scoreChanged(int pointsToAdd)
        {
            this.score += pointsToAdd;
            this.pointsDisplay.Text = this.score.ToString();
        }


        private void gameOver(bool playerWon)
        {
            var message = "GAME OVER!\n";
            this.timer.Stop();
            if (playerWon)
            {
                message += "You won!\n";
            }
            else
            {
                message += "You lost!\n";
            }

            this.messageDisplay.Text = message;
            this.messageDisplay.Visibility = Visibility.Visible;
        }

        private void createAndPlacePlayerShip(Canvas background)
        {
            this.playerShip = new PlayerShip();
            background.Children.Add(this.playerShip.Sprite);

            this.placePlayerShipNearBottomOfBackgroundCentered();
        }

        private void placePlayerShipNearBottomOfBackgroundCentered()
        {
            this.playerShip.X = this.backgroundWidth / 2 - this.playerShip.Width / 2.0;
            this.playerShip.Y = this.backgroundHeight - this.playerShip.Height - PlayerShipBottomOffset;
        }

        /// <summary>
        ///     Moves the player ship to the left.
        ///     Precondition: Player ship is not at the left boundary of the background
        ///     Postcondition: The player ship has moved left.
        /// </summary>
        public void MovePlayerShipLeft()
        {
            if (this.playerShip.X > 0)
            {
                this.playerShip.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the player ship to the right.
        ///     Precondition: Player ship is not at the right boundary of the background
        ///     Postcondition: The player ship has moved right.
        /// </summary>
        public void MovePlayerShipRight()
        {
            if (this.playerShip.X < this.backgroundWidth - this.playerShip.Width)
            {
                this.playerShip.MoveRight();
            }
        }

        private void playerHit()
        {
            this.lives--;

            if(this.lives <= 0)
            {
                this.playerShip.Sprite.Visibility = Visibility.Collapsed;
                this.gameOver(false);
            }
        }
        private void activatePlayerBullet(Bullet bullet)
        {
            bullet.X = this.playerShip.X + .5 * this.playerShip.Width - .5 * bullet.Width;
            bullet.Y = this.playerShip.Y - bullet.Height;
            bullet.Sprite.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// Shoots the player bullet.
        /// </summary>
        public void ShootPlayerBullet()
        {
            int bulletCount = 0;

            bool bulletFired = this.timeBetweenPlayerBullets >= MaxTimeBetweenPlayerBullets;

            do
            {
                if(this.playerBullets[bulletCount].Sprite.Visibility == Visibility.Collapsed)
                {
                    this.activatePlayerBullet(this.playerBullets[bulletCount]);
                    bulletFired = true;
                }
                bulletCount++;
            } while (bulletFired == false && bulletCount < this.playerBullets.Count);
        }

        #endregion
    }
}