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

        private const int enemyShipSteps = 20;
        private const double PlayerShipBottomOffset = 30;
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;

        private const int shipsPerRow = 4;

        private bool movingRight;
        private int movements;

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

        private List<EnemyShip> EnemyShips1;
        private List<EnemyShip> EnemyShips2;
        private List<EnemyShip> EnemyShips3;

        private Bullet playerBullet;
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

            movements = 0;
            movingRight = true;
            score = 0;

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

            playerBullet = new Bullet(true);
            background.Children.Add(playerBullet.Sprite);
            playerBullet.Sprite.Visibility = Visibility.Collapsed;

            //this.enemyBullet = new EnemyBullet();
            enemyBullet = new Bullet(false);
            background.Children.Add(enemyBullet.Sprite);
            enemyBullet.Sprite.Visibility = Visibility.Collapsed;
        }

        private void timerTick(object sender, object e)
        {
            if (playerBullet.Sprite.Visibility == Visibility.Visible) movePlayerBullet();

            enemyManager.OnTick();
            scoreChanged(enemyManager.DidPlayerBulletHitEnemy(playerBullet));

            if (!enemyManager.EnemiesRemain()) gameOver(true);

            if (enemyManager.EnemyBullet.CheckForCollision(playerShip) != null) gameOver(false);

            movements++;
        }

        private void movePlayerBullet()
        {
            playerBullet.MoveUp();

            if (playerBullet.Y <= 0) playerBullet.Sprite.Visibility = Visibility.Collapsed;
        }

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

        public void ShootPlayerBullet()
        {
            if (playerBullet.Sprite.Visibility == Visibility.Collapsed &&
                playerShip.Sprite.Visibility == Visibility.Visible)
            {
                playerBullet.X = playerShip.X + .5 * playerShip.Width - .5 * playerBullet.Width;
                playerBullet.Y = playerShip.Y - playerBullet.Height;
                playerBullet.Sprite.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}