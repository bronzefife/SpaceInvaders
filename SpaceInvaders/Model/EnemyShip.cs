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
        ///     Initializes a new instance of the <see cref="EnemyShip" /> class
        ///     of the given ship level.
        /// </summary>
        public EnemyShip(int shipLevel)
        {
            SetSpeed(SpeedXDirection, SpeedYDirection);

            switch (shipLevel)
            {
                case 2:
                    this.PointValue = 10;
                    this.Sprite1 = new EnemyShip2Sprite();
                    this.Sprite2 = new EnemyShip2Sprite2();
                    this.CanShoot = false;
                    break;
                case 3:
                    this.PointValue = 15;
                    this.Sprite1 = new EnemyShip3Sprite();
                    this.Sprite2 = new EnemyShip3Sprite2();
                    this.CanShoot = true;
                    break;
                case 4:
                    this.PointValue = 20;
                    this.Sprite1 = new EnemyShip4Sprite();
                    this.Sprite2 = new EnemyShip4Sprite2();
                    this.CanShoot = true;
                    break;
                default:
                    this.PointValue = 5;
                    this.Sprite1 = new EnemyShip1Sprite();
                    this.Sprite2 = new EnemyShip1Sprite();
                    this.CanShoot = false;
                    break;
            }
            Sprite = this.Sprite1;
            this.Sprite2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        
        /// <summary>
        /// Animates this ship's sprite.
        /// </summary>
        public void Animate()
        {
            if(Sprite.Equals(this.Sprite1))
            {
                Sprite = this.Sprite2;
                this.Sprite1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.Sprite2.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                Sprite = this.Sprite1;
                this.Sprite2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.Sprite1.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

        }

        #endregion
    }
}