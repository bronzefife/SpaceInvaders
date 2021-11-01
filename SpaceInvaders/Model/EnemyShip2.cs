using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    internal class EnemyShip2 : EnemyShip
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip2" /> class.
        /// </summary>
        public EnemyShip2()
        {
            Sprite = new EnemyShip2Sprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
            PointValue = 10;
        }

        #endregion

        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion
    }
}