//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Beebapps.Game.Input;
//using Microsoft.Xna.Framework.Input;

//namespace GameDev.Utils
//{
//    /// <summary>
//    /// Enables left mouse click in an area.
//    /// </summary>
//    public class ClickableArea : GameComponent
//    {
//        public ClickableArea() : base(GameDevGame.Current)
//        {
//        }

//        public ClickableArea(Rectangle rectangle) : base(GameDevGame.Current)
//        {
//            SetArea(rectangle);
//        }

//        private Rectangle _Rectangle;

//        public override void Update(GameTime gameTime)
//        {
//            base.Update(gameTime);
            
//            if(MouseClickWithin(MouseExtended.Current.CurrentState))
//            {
//                if (MouseExtended.Current.WasSingleClick(MouseButton.Left))
//                {
//                    OnAreaClicked();
//                }
//            }
//        }

//        private bool MouseClickWithin(MouseState mouseState)
//        {
//            if (_Rectangle.Left <= mouseState.X && mouseState.X <= _Rectangle.Right
//                && _Rectangle.Top <= mouseState.Y && mouseState.Y <= _Rectangle.Bottom)
//            {
//                return true;
//            }
//            return false;
//        }

//        private event EventHandler _AreaClicked = delegate { };
//        public event EventHandler AreaClicked
//        {
//            add { _AreaClicked += value; }
//            remove { _AreaClicked += value; }
//        }

//        protected virtual void OnAreaClicked()
//        {
//            _AreaClicked(this, new EventArgs());
//        }

//        public void SetArea(Rectangle rectangle)
//        {
//            _Rectangle = rectangle;
//        }

//    }
//}
