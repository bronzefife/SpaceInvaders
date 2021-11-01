using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the player ship.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class PlayerBullet : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerBullet()
        {
            Sprite = new PlayerBulletSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Data members

        private const int SpeedXDirection = 0;
        private const int SpeedYDirection = 12;

        #endregion
    }
}