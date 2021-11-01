using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    internal class EnemyShip3 : EnemyShip
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip3" /> class.
        /// </summary>
        public EnemyShip3()
        {
            Sprite = new EnemyShip3Sprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
            PointValue = 15;
        }

        #endregion

        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion
    }
}