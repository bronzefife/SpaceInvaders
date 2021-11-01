using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Manages the player ship.
    /// </summary>
    /// <seealso cref="SpaceInvaders.Model.GameObject" />
    public class PlayerShip : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerShip" /> class.
        /// </summary>
        public PlayerShip()
        {
            Sprite = new PlayerShipSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion
    }
}