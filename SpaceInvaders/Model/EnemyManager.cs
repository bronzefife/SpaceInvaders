using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SpaceInvaders.Model
{
    internal class EnemyManager
    {
        private const int enemiesInRow1 = 2;
        private const int enemiesInRow2 = 4;
        private const int enemiesInRow3 = 6;
        private const int enemiesInRow4 = 8;
        private const int totalMovementsAllowed = 20;
        private readonly double backgroundHeight;
        private readonly double backgroundWidth;
        private readonly IList<EnemyShip> enemyShips;
        private readonly Random random;
        private int movements;

        private bool movingRight;

        public EnemyManager(Canvas canvas, double height, double width)
        {
            //TODO: error checking

            backgroundHeight = height;
            backgroundWidth = width;

            enemyShips = new List<EnemyShip>();
            random = new Random();
            movingRight = true;
            movements = 10;
            EnemyBullet = new Bullet(false);
            canvas.Children.Add(EnemyBullet.Sprite);
            EnemyBullet.Sprite.Visibility = Visibility.Collapsed;
            createAllEnemies();
            addEnemiesToCanvas(canvas);
            positionEnemies();
        }

        public Bullet EnemyBullet { get; protected set; }

        private void addEnemiesToCanvas(Canvas canvas)
        {
            foreach (var ship in enemyShips)
            {
                canvas.Children.Add(ship.Sprite1);
                canvas.Children.Add(ship.Sprite2);
            }
        }

        private void positionEnemies()
        {
            var shipWidth = enemyShips[0].Width;
            var shipGap = shipWidth / 2;
            double distanceFromLeft;
            int rowNumberFromTop;
            int shipsPerRow;
            int shipNumberinRow;
            var gapFromSides = totalMovementsAllowed * enemyShips[0].SpeedX;

            for (var i = 0; i < enemyShips.Count; i++)
            {
                shipNumberinRow = i;

                if (i < enemiesInRow1)
                {
                    rowNumberFromTop = 1;
                    shipsPerRow = enemiesInRow1;
                }
                else if (i < enemiesInRow1 + enemiesInRow2)
                {
                    rowNumberFromTop = 2;
                    shipsPerRow = enemiesInRow2;
                    shipNumberinRow -= enemiesInRow1;
                }
                else if (i < enemiesInRow1 + enemiesInRow2 + enemiesInRow3)
                {
                    rowNumberFromTop = 3;
                    shipsPerRow = enemiesInRow3;
                    shipNumberinRow -= enemiesInRow1 + enemiesInRow2;
                }
                else
                {
                    rowNumberFromTop = 4;
                    shipsPerRow = enemiesInRow4;
                    shipNumberinRow -= enemiesInRow1 + enemiesInRow2 + enemiesInRow3;
                }

                distanceFromLeft = (backgroundWidth - shipsPerRow * shipWidth - (shipsPerRow - 1) * shipGap) / 2;

                enemyShips[i].Y = (enemyShips[i].Height + 20) * rowNumberFromTop;

                enemyShips[i].X = distanceFromLeft + (shipGap + shipWidth) * shipNumberinRow;
            }
        }

        private void createAllEnemies()
        {
            for (var i = 0; i < enemiesInRow1; i++) createEnemy(4);
            for (var i = 0; i < enemiesInRow2; i++) createEnemy(3);
            for (var i = 0; i < enemiesInRow3; i++) createEnemy(2);
            for (var i = 0; i < enemiesInRow4; i++) createEnemy(1);
        }

        private void createEnemy(int enemyLevel)
        {
            enemyShips.Add(new EnemyShip(enemyLevel));
        }

        public void OnTick()
        {
            moveEnemies();
            animateEnemies();
            randomShot();
            moveBullet();
        }

        private void animateEnemies()
        {
            foreach(var ship in enemyShips)
            {
                ship.Animate();
            }
        }

        private void moveEnemies()
        {
            if (movements >= totalMovementsAllowed)
            {
                movements = 0;
                movingRight = !movingRight;
            }

            if (movingRight)
                moveEnemiesRight();
            else
                moveEnemiesLeft();

            movements++;
        }

        private void moveEnemiesRight()
        {
            foreach (var ship in enemyShips) ship.MoveRight();
        }

        private void moveEnemiesLeft()
        {
            foreach (var ship in enemyShips) ship.MoveLeft();
        }

        private void randomShot()
        {
            var shootingShips = -1;

            foreach (var ship in enemyShips)
                if (ship.CanShoot)
                    shootingShips++;

            if (random.Next(101) == 1 && shootingShips > 0 && EnemyBullet.Sprite.Visibility == Visibility.Collapsed)
                spawnEnemyBullet(enemyShips[random.Next(shootingShips + 1)]);
        }

        private void spawnEnemyBullet(EnemyShip enemy)
        {
            EnemyBullet.Y = enemy.Y + enemy.Height + EnemyBullet.Height;
            EnemyBullet.X = enemy.X + .5 * enemy.Width - .5 * EnemyBullet.Width;
            EnemyBullet.Sprite.Visibility = Visibility.Visible;
        }

        private void moveBullet()
        {
            if (EnemyBullet.Sprite.Visibility == Visibility.Visible) EnemyBullet.MoveDown();

            if (EnemyBullet.Y >= backgroundHeight - EnemyBullet.Height)
                EnemyBullet.Sprite.Visibility = Visibility.Collapsed;
        }

        //TODO comment
        public bool EnemiesRemain()
        {
            if (enemyShips.Count == 0) return false;
            return true;
        }

        //TODO comment
        public int DidPlayerBulletHitEnemy(Bullet playerBullet)
        {
            var returnValue = 0;

            if (playerBullet.Sprite.Visibility == Visibility.Collapsed) return returnValue;

            for (var i = 0; i < enemyShips.Count; i++)
                if (playerBullet.CheckForCollision(enemyShips[i]) != null)
                {
                    returnValue = enemyShips[i].PointValue;
                    enemyShips.RemoveAt(i);
                    return returnValue;
                }

            return returnValue;
        }
    }
}