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

        public BaseSprite Sprite1;
        public BaseSprite Sprite2;

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
                    Sprite1 = new EnemyShip2Sprite();
                    Sprite2 = new EnemyShip2Sprite2();
                    CanShoot = false;
                    break;
                case 3:
                    PointValue = 15;
                    Sprite1 = new EnemyShip3Sprite();
                    Sprite2 = new EnemyShip3Sprite2();
                    CanShoot = true;
                    break;
                case 4:
                    PointValue = 20;
                    Sprite1 = new EnemyShip4Sprite();
                    Sprite2 = new EnemyShip4Sprite2();
                    CanShoot = true;
                    break;
                default:
                    PointValue = 5;
                    Sprite1 = new EnemyShip1Sprite();
                    Sprite2 = new EnemyShip1Sprite();
                    CanShoot = false;
                    break;
            }
            Sprite = Sprite1;
            Sprite2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public void Animate()
        {
            if(Sprite.Equals(Sprite1))
            {
                Sprite = Sprite2;
                Sprite1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Sprite2.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                Sprite = Sprite1;
                Sprite2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Sprite1.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }

        #endregion
    }
}