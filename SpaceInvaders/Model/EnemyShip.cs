using SpaceInvaders.View.Sprites;

namespace SpaceInvaders.Model
{
    public class EnemyShip : GameObject
    {
        #region Data members

        public const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;
        public int PointValue { get; protected set; }
        public bool CanShoot { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class.
        /// </summary>
        public EnemyShip()
        {
            Sprite = new EnemyShip1Sprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
            PointValue = 5;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class
        ///     of the given ship level.
        /// </summary>
        public EnemyShip(int shipLevel)
        {
            SetSpeed(SpeedXDirection, SpeedYDirection);

            switch (shipLevel)
            {
                case 2:
                    PointValue = 10;
                    Sprite = new EnemyShip2Sprite();
                    CanShoot = false;
                    break;
                case 3:
                    PointValue = 15;
                    Sprite = new EnemyShip3Sprite();
                    CanShoot = true;
                    break;
                case 4:
                    PointValue = 20;
                    Sprite = new EnemyShip4Sprite();
                    CanShoot = true;
                    break;
                default:
                    PointValue = 5;
                    Sprite = new EnemyShip1Sprite();
                    CanShoot = false;
                    break;
            }
        }

        #endregion
    }
}