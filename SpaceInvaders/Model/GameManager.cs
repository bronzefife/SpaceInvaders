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
            if (backgroundHeight <= 0) throw new ArgumentOutOfRangeException(nameof(backgroundHeight));

            if (backgroundWidth <= 0) throw new ArgumentOutOfRangeException(nameof(backgroundWidth));

            this.backgroundHeight = backgroundHeight;
            this.backgroundWidth = backgroundWidth;
        }

        #endregion

        #region Data members

        private const int maxTimeBetweenPlayerBullets = 10;
        private const int maxLives = 3;
        private const int playerBulletAmount = 3;
        private const double PlayerShipBottomOffset = 30;
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private int timeBetweenPlayerBullets;
        private int lives;

        private int score;

        public int Score
        {
            get => score;
            set
            {
                score = Score;
                scoreChanged(score);
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
            if (background == null) throw new ArgumentNullException(nameof(background));

            enemyManager = new EnemyManager(background, backgroundHeight, backgroundWidth);

            score = 0;
            lives = maxLives;
            timeBetweenPlayerBullets = maxTimeBetweenPlayerBullets;

            timer = new DispatcherTimer();
            timer.Tick += timerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();

            random = new Random();

            createAndPlacePlayerShip(background);

            pointsDisplay = new TextBlock();
            pointsDisplay.Text = score.ToString();
            pointsDisplay.Foreground = new SolidColorBrush(Colors.White);
            background.Children.Add(pointsDisplay);
            Canvas.SetLeft(pointsDisplay, 25);

            messageDisplay = new TextBlock();
            messageDisplay.Visibility = Visibility.Collapsed;
            messageDisplay.Foreground = new SolidColorBrush(Colors.White);
            background.Children.Add(messageDisplay);
            Canvas.SetTop(messageDisplay, backgroundHeight / 2);
            Canvas.SetLeft(messageDisplay, backgroundWidth / 3);

            playerBullets = new List<Bullet>();

            for(int i = 0; i < playerBulletAmount; i++)
            {
                playerBullets.Add(new Bullet(true));
                background.Children.Add(playerBullets[playerBullets.Count - 1].Sprite);
                playerBullets[playerBullets.Count - 1].Sprite.Visibility = Visibility.Collapsed;
            }

            enemyBullet = new Bullet(false);
            background.Children.Add(enemyBullet.Sprite);
            enemyBullet.Sprite.Visibility = Visibility.Collapsed;
        }

        private void timerTick(object sender, object e)
        {
            timeBetweenPlayerBullets++;
            movePlayerBullets();

            enemyManager.OnTick();
            didPlayerBulletHitEnemy();

            if (!enemyManager.EnemiesRemain()) gameOver(true);

            if (enemyManager.EnemyBullet.CheckForCollision(playerShip) != null) playerHit();
        }

        private void didPlayerBulletHitEnemy()
        {
            foreach(Bullet bullet in playerBullets)
            {
                scoreChanged(enemyManager.DidPlayerBulletHitEnemy(bullet));
            }
        }
        private void movePlayerBullets()
        {
            foreach(var bullet in playerBullets)
            {
                if(bullet.Sprite.Visibility == Visibility.Visible) bullet.MoveUp();

                if (bullet.Y <= 0) bullet.Sprite.Visibility = Visibility.Collapsed;
            }
        }
        
        /// <summary>
        /// Did the enemy bullet hit player.
        /// </summary>
        /// <param name="bullet">The bullet.</param>
        /// <returns>True if the player was hit, false otherwise</returns>
        public bool DidEnemyBulletHitPlayer(Bullet bullet)
        {
            return bullet.CheckForCollision(playerShip) != null;
        }

        private void scoreChanged(int pointsToAdd)
        {
            score += pointsToAdd;
            pointsDisplay.Text = score.ToString();
        }


        private void gameOver(bool playerWon)
        {
            var message = "GAME OVER!\n";
            timer.Stop();
            if (playerWon)
                message += "You won!\n";
            else
                message += "You lost!\n";

            messageDisplay.Text = message;
            messageDisplay.Visibility = Visibility.Visible;
        }

        private void createAndPlacePlayerShip(Canvas background)
        {
            playerShip = new PlayerShip();
            background.Children.Add(playerShip.Sprite);

            placePlayerShipNearBottomOfBackgroundCentered();
        }

        private void placePlayerShipNearBottomOfBackgroundCentered()
        {
            playerShip.X = backgroundWidth / 2 - playerShip.Width / 2.0;
            playerShip.Y = backgroundHeight - playerShip.Height - PlayerShipBottomOffset;
        }

        /// <summary>
        ///     Moves the player ship to the left.
        ///     Precondition: Player ship is not at the left boundary of the background
        ///     Postcondition: The player ship has moved left.
        /// </summary>
        public void MovePlayerShipLeft()
        {
            if (playerShip.X > 0) playerShip.MoveLeft();
        }

        /// <summary>
        ///     Moves the player ship to the right.
        ///     Precondition: Player ship is not at the right boundary of the background
        ///     Postcondition: The player ship has moved right.
        /// </summary>
        public void MovePlayerShipRight()
        {
            if (playerShip.X < backgroundWidth - playerShip.Width) playerShip.MoveRight();
        }

        private void playerHit()
        {
            lives--;

            if(lives <= 0)
            {
                playerShip.Sprite.Visibility = Visibility.Collapsed;
                gameOver(false);
            }
        }
        private void activatePlayerBullet(Bullet bullet)
        {
            bullet.X = playerShip.X + .5 * playerShip.Width - .5 * bullet.Width;
            bullet.Y = playerShip.Y - bullet.Height;
            bullet.Sprite.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// Shoots the player bullet.
        /// </summary>
        public void ShootPlayerBullet()
        {
            int bulletCount = 0;

            bool bulletFired = false;

            if (timeBetweenPlayerBullets >= maxTimeBetweenPlayerBullets) bulletFired = true;

            do
            {
                if(playerBullets[bulletCount].Sprite.Visibility == Visibility.Collapsed)
                {
                    activatePlayerBullet(playerBullets[bulletCount]);
                    bulletFired = true;
                }
                bulletCount++;
            } while (bulletFired == false && bulletCount < playerBullets.Count);
        }

        #endregion
    }
}