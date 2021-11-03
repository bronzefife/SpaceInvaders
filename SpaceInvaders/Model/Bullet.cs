using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages bullet objects.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class Bullet : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 0;
        private const int SpeedYDirection = 12;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyBullet" /> class.
        /// </summary>
        public Bullet(bool isPlayerBullet)
        {
            if (isPlayerBullet)
                Sprite = new PlayerBulletSprite();
            else
                Sprite = new EnemyBulletSprite();

            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        /// <summary>Checks for collision between the bullet and a given ship.</summary>
        /// <param name="ship">The ship.</param>
        /// <returns>
        ///     The ship GameObject if there is a collision, or null if the objects do not collide.
        /// </returns>
        /// <exception cref="ArgumentNullException">ship</exception>
        public GameObject CheckForCollision(GameObject ship)
        {
            if (ship == null) throw new ArgumentNullException(nameof(ship));

            if (Sprite.Visibility == Visibility.Collapsed) return null;

            var bulletBoundary = new Rect(X, Y, Width, Height);
            var shipBoundary = new Rect(ship.X, ship.Y, ship.Width, ship.Height);

            var intersect = bulletBoundary;
            intersect.Intersect(shipBoundary);

            if (intersect.IsEmpty) return null;

            Sprite.Visibility = Visibility.Collapsed;

            return ship;
        }

        #endregion
    }
}