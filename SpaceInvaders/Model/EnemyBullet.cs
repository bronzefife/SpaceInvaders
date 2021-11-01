using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the player ship.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class EnemyBullet : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyBullet" /> class.
        /// </summary>
        public EnemyBullet()
        {
            Sprite = new EnemyBulletSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Data members

        private const int SpeedXDirection = 0;
        private const int SpeedYDirection = 12;

        #endregion
    }
}