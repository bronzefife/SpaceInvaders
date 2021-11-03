using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.Model
{
    internal class EnemyManager
    {
        private const int EnemiesInRow1 = 2;
        private const int EnemiesInRow2 = 4;
        private const int EnemiesInRow3 = 6;
        private const int EnemiesInRow4 = 8;
        private const int TotalMovementsAllowed = 20;
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        private readonly IList<EnemyShip> enemyShips;
        private readonly Random random;
        private int movements;

        private bool movingRight;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0 AND canvas != null
        /// </summary>
        /// <param name="canvas">The background canvas.</param>
        /// <param name="height">The height of the game play window.</param>
        /// <param name="width">The width of the game play window.</param>
        public EnemyManager(Canvas canvas, double height, double width)
        {
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(this.backgroundHeight));
            }

            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(this.backgroundWidth));
            }

            if (canvas == null)
            {
                throw new ArgumentOutOfRangeException(nameof(canvas));
            }

            this.backgroundHeight = height;
            this.backgroundWidth = width;

            this.enemyShips = new List<EnemyShip>();
            this.random = new Random();
            this.movingRight = true;
            this.movements = 10;
            this.EnemyBullet = new Bullet(false);
            canvas.Children.Add(this.EnemyBullet.Sprite);
            this.EnemyBullet.Sprite.Visibility = Visibility.Collapsed;
            this.createAllEnemies();
            this.addEnemiesToCanvas(canvas);
            this.positionEnemies();
        }

        public Bullet EnemyBullet { get; protected set; }

        private void addEnemiesToCanvas(Canvas canvas)
        {
            foreach (var ship in this.enemyShips)
            {
                canvas.Children.Add(ship.Sprite1);
                canvas.Children.Add(ship.Sprite2);
            }
        }

        private void positionEnemies()
        {
            var shipWidth = this.enemyShips[0].Width;
            var shipGap = shipWidth / 2;

            for (var i = 0; i < this.enemyShips.Count; i++)
            {
                int rowNumberFromTop;
                int shipsPerRow;
                int shipNumberInRow = i;

                if (i < EnemiesInRow1)
                {
                    rowNumberFromTop = 1;
                    shipsPerRow = EnemiesInRow1;
                }
                else if (i < EnemiesInRow1 + EnemiesInRow2)
                {
                    rowNumberFromTop = 2;
                    shipsPerRow = EnemiesInRow2;
                    shipNumberInRow -= EnemiesInRow1;
                }
                else if (i < EnemiesInRow1 + EnemiesInRow2 + EnemiesInRow3)
                {
                    rowNumberFromTop = 3;
                    shipsPerRow = EnemiesInRow3;
                    shipNumberInRow -= EnemiesInRow1 + EnemiesInRow2;
                }
                else
                {
                    rowNumberFromTop = 4;
                    shipsPerRow = EnemiesInRow4;
                    shipNumberInRow -= EnemiesInRow1 + EnemiesInRow2 + EnemiesInRow3;
                }

                double distanceFromLeft = (this.backgroundWidth - shipsPerRow * shipWidth - (shipsPerRow - 1) * shipGap) / 2;

                this.enemyShips[i].Y = (this.enemyShips[i].Height + 20) * rowNumberFromTop;

                this.enemyShips[i].X = distanceFromLeft + (shipGap + shipWidth) * shipNumberInRow;
            }
        }

        private void createAllEnemies()
        {
            for (var i = 0; i < EnemiesInRow1; i++)
            {
                this.createEnemy(4);
            }

            for (var i = 0; i < EnemiesInRow2; i++)
            {
                this.createEnemy(3);
            }

            for (var i = 0; i < EnemiesInRow3; i++)
            {
                this.createEnemy(2);
            }

            for (var i = 0; i < EnemiesInRow4; i++)
            {
                this.createEnemy(1);
            }
        }

        private void createEnemy(int enemyLevel)
        {
            this.enemyShips.Add(new EnemyShip(enemyLevel));
        }

        /// <summary>
        ///     Performs enemy tasks that should be undertaken on the tick of a clock
        /// </summary>
        public void OnTick()
        {
            this.moveEnemies();
            this.animateEnemies();
            this.randomShot();
            this.moveBullet();
        }

        private void animateEnemies()
        {
            foreach(var ship in this.enemyShips)
            {
                ship.Animate();
            }
        }

        private void moveEnemies()
        {
            if (this.movements >= TotalMovementsAllowed)
            {
                this.movements = 0;
                this.movingRight = !this.movingRight;
            }

            if (this.movingRight)
            {
                this.moveEnemiesRight();
            }
            else
            {
                this.moveEnemiesLeft();
            }

            this.movements++;
        }

        private void moveEnemiesRight()
        {
            foreach (var ship in this.enemyShips)
            {
                ship.MoveRight();
            }
        }

        private void moveEnemiesLeft()
        {
            foreach (var ship in this.enemyShips)
            {
                ship.MoveLeft();
            }
        }

        private void randomShot()
        {
            var shootingShips = 0;

            foreach (var ship in this.enemyShips)
            {
                if (ship.CanShoot)
                {
                    shootingShips++;
                }
            }

            if (this.random.Next(101) == 1 && shootingShips > 0 && this.EnemyBullet.Sprite.Visibility == Visibility.Collapsed)
            {
                this.spawnEnemyBullet(this.enemyShips[this.random.Next(shootingShips + 1)]);
            }
        }

        private void spawnEnemyBullet(EnemyShip enemy)
        {
            this.EnemyBullet.Y = enemy.Y + enemy.Height + this.EnemyBullet.Height;
            this.EnemyBullet.X = enemy.X + .5 * enemy.Width - .5 * this.EnemyBullet.Width;
            this.EnemyBullet.Sprite.Visibility = Visibility.Visible;
        }

        private void moveBullet()
        {
            if (this.EnemyBullet.Sprite.Visibility == Visibility.Visible)
            {
                this.EnemyBullet.MoveDown();
            }

            if (this.EnemyBullet.Y >= this.backgroundHeight - this.EnemyBullet.Height)
            {
                this.EnemyBullet.Sprite.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0 AND canvas != null
        /// </summary>
        /// <returns>True if there are enemies remaining, and false if not</returns>
        public bool EnemiesRemain()
        {
            return this.enemyShips.Count != 0;
        }

        /// <summary>
        ///     Evaluates whether a player bullet has hit an enemy, returning 0 if
        ///     there is no collision and returning the enemy's score value otherwise
        /// </summary>
        /// <returns>The score of the enemy hit, if any</returns>
        public int DidPlayerBulletHitEnemy(Bullet playerBullet)
        {
            var returnValue = 0;

            for (var i = 0; i < this.enemyShips.Count; i++)
            {
                if (playerBullet.CheckForCollision(this.enemyShips[i]) != null)
                {
                    returnValue = this.enemyShips[i].PointValue;
                    this.enemyShips[i].Sprite.Visibility = Visibility.Collapsed;
                    this.enemyShips.RemoveAt(i);
                    return returnValue;
                }
            }

            return returnValue;
        }
    }
}