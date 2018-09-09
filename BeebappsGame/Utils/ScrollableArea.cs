using System.Diagnostics;
using Beebapps.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Beebapps.Game.Utils
{
    /// <summary>
    /// Represents an area which can be scrolled by touch. Either up/down or right/left or both
    /// </summary>
    public class ScrollableArea : GameComponent
    {
        private Rectangle TouchArea { get; set; }
        private bool CanScrollVertical { get; set; }
        private bool CanScrollHorizontal { get; set; }

        /// <summary>
        /// Friction represents how much the scroll breaks at each update. 0 equals never. 1 At once.
        /// </summary>
        private float Friction { get; set; }

        private Vector2 ScrollSpeed 
        {
            get; set; 
        }

        private Vector2 _scrollPosition;
        public Vector2 ScrollPosition 
        {
            get { return _scrollPosition; }
            set
            {
                if (!CanScrollHorizontal)
                {
                    _scrollPosition = new Vector2(_scrollPosition.X, value.Y);
                }
                else if (!CanScrollVertical)
                {
                    _scrollPosition = new Vector2(value.X, _scrollPosition.Y);
                }
                else
                {
                    _scrollPosition = value;
                }
            }
        }

        private Vector2 Origin { get; set; }

        private Vector2 PotentialSpeed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="touchArea"></param>
        /// <param name="origin">The origin is just the Vector from which the ScrollPosition is calculated. It is just to have a starting point</param>
        /// <param name="canScrollVertical"></param>
        /// <param name="canScrollHorizontal"></param>
        /// <param name="friction"></param>
        public ScrollableArea(Rectangle touchArea, Vector2 origin, bool canScrollVertical, bool canScrollHorizontal, float friction) : base(BeebappsGame.Current)
        {
            TouchArea = touchArea;
            CanScrollHorizontal = canScrollHorizontal;
            CanScrollVertical = canScrollVertical;
            Friction = friction;
            Origin = origin;
            _scrollPosition = origin;
            _oldScrollPosition = origin;
        }

        private bool IsWithin
        {
            get
            {
                if (TouchArea.Equals(Rectangle.Empty))
                    return true;

                return TouchPosition.X >= TouchArea.X && TouchPosition.X <= TouchArea.X + TouchArea.Width &&
                       TouchPosition.Y >= TouchArea.Y && TouchPosition.Y <= TouchArea.Y + TouchArea.Height;
            }
        }

        public Vector2 TouchPosition
        {
            get { return SingleTouchExtended.Current.CurrentState.Position; }
        }



        public override void Update(GameTime gameTime)
        {
            _oldScrollPosition = ScrollPosition;
            if (SingleTouchExtended.Current.CurrentState.State == TouchLocationState.Pressed && IsWithin)
            {
                Origin = SingleTouchExtended.Current.CurrentState.Position;
                ScrollSpeed = Vector2.Zero;
            }
            else if (SingleTouchExtended.Current.CurrentState.State == TouchLocationState.Moved && IsWithin)
            {
                Vector2 newScrollPosition = ScrollPosition + (SingleTouchExtended.Current.CurrentState.Position - Origin);
                Origin = SingleTouchExtended.Current.CurrentState.Position;
                PotentialSpeed = ScrollPosition - newScrollPosition;
                ScrollPosition = newScrollPosition;
            }
            else if (PotentialSpeed != Vector2.Zero)
            {
                ScrollSpeed = PotentialSpeed;
                PotentialSpeed = Vector2.Zero;
            }
            else
            {
                ScrollSpeed = new Vector2(Friction * ScrollSpeed.X, Friction * ScrollSpeed.Y);
            }
            ScrollPosition -= ScrollSpeed;
            base.Update(gameTime);
        }

        public void StopScroll(Vector2 newScrollPosition)
        {
            ScrollSpeed = Vector2.Zero;
            ScrollPosition = newScrollPosition;
        }

        private Vector2 _oldScrollPosition;

        public bool IsMoving
        {
            get { return !_oldScrollPosition.Equals(ScrollPosition); }
        }
    }
}
