using System;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Menus
{
    public delegate void MenuItemHandler(MenuItem sender, EventArgs e);

    public abstract class MenuItem : DrawableGameComponent
    {

        #region Properties

        public int MillisecondsBetweenKeyReceives { get; set; }
        private DateTime _lastKeyboardReceive = DateTime.MinValue;
        protected bool NeedsPositionRecalculation { get; set; }
        public string Name { get; set; }
        private bool _isSelected;
        public int Top { get; set; }
        public Vector2 Position { get; set; }
        private bool _centered = true;
        public bool Selectable { get; set; }
        public bool Centered
        {
            get
            {
                return _centered;
            }
            set
            {
                _centered = value;
                NeedsPositionRecalculation = true;
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (Selectable)
                {

                
                bool oldValue = _isSelected;
                _isSelected = value;
                if (oldValue != IsSelected)
                {
                    if (IsSelected)
                    {
                        OnSelected();
                    }
                    else
                    {
                        OnDeSelected();
                    }
                }
                }
            }
        }

        #endregion

        public event MenuItemHandler Selected;
        public event MenuItemHandler Deselected;
        public event MenuItemHandler Activated;


        #region Constructors

        protected MenuItem(string name, Vector2 position, bool centered)
            : base(BeebappsGame.Current)
        {
            Name = name;
            Position = position;
            Centered = centered;
            MillisecondsBetweenKeyReceives = 200;
            Selectable = true;
        }

        #endregion
        

        #region Events

        protected void OnSelected()
        {
            if (Selected != null)
            {
                Selected(this, EventArgs.Empty);
            }
        }

        protected void OnDeSelected()
        {
            if (Deselected != null)
            {
                Deselected(this, EventArgs.Empty);
            }
        } 
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (NeedsPositionRecalculation)
            {
                RecalculatePosition();
                NeedsPositionRecalculation = false;
            }
        }

        public void Activate()
        {
            OnActivate();
        }

        protected void OnActivate()
        {
            if (Activated != null)
            {
                Activated(this, EventArgs.Empty);
            }
        }
        public bool IsReadyForKeyboardInteraction
        {
            get { return (DateTime.Now - _lastKeyboardReceive).TotalMilliseconds > MillisecondsBetweenKeyReceives; }
        }

        public void ResetKeyboardIntervalTimer()
        {
            _lastKeyboardReceive = DateTime.Now;
        }
        protected abstract void RecalculatePosition();
    }
}
