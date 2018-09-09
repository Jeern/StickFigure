using System;
using System.Collections.Generic;
using System.Linq;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Text
{
    public class TextUtil : DrawableGameComponent
    {
        private readonly SpriteFont _font;
        private readonly Color _color;
        private readonly Color _shadowColor;
        private Vector2 _offset;
        private readonly HorizontalAlignment _hAlign;
        private readonly VerticalAlignment _vAlign;
        private string[] _texts;

        private Rectangle _viewBox = Rectangle.Empty;
        private readonly Resetable<Vector2> _currentScrollPosition = new Resetable<Vector2>();
        private readonly Vector2 _scrollSpeed;
        private readonly bool _useShadow;
        private float _textHeight;
        private float _textWidth;
        private readonly float _gameWidth;
        private readonly float _gameHeight;

        public float TextHeight
        {
            get { return _textHeight; }
        }

        public float TextWidth
        {
            get { return _textWidth; }
        }

        public TextUtil(Vector2 scrollSpeed, SpriteFont font,
            Color color, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va) : base(BeebappsGame.Current)
        {
            _scrollSpeed = scrollSpeed;
            _font = font;
            _color = color;
            _offset = offset;
            _hAlign = ha;
            _vAlign = va;
            _currentScrollPosition.Value = Vector2.Zero;
            _currentScrollPosition.Set();
            _gameHeight = BeebappsGame.Current.GraphicsDevice.Viewport.Height;
            _gameWidth = BeebappsGame.Current.GraphicsDevice.Viewport.Width;
        }

        public TextUtil(Rectangle viewBox, Vector2 scrollSpeed, SpriteFont font, Color color, Vector2 offset, 
            HorizontalAlignment ha, VerticalAlignment va)
            : this(scrollSpeed, font, color, offset, ha, va)
        {
            _viewBox = viewBox;
            _offset.X -= viewBox.X;
            _offset.Y -= viewBox.Y;
        }

        public TextUtil(Vector2 scrollSpeed, SpriteFont font, Color color, Color shadowColor, Vector2 offset, 
            HorizontalAlignment ha, VerticalAlignment va)
            : this(scrollSpeed, font, color, offset, ha, va)
        {
            _shadowColor = shadowColor;
            _useShadow = true;
        }

        public TextUtil(Rectangle viewBox, Vector2 scrollSpeed, SpriteFont font,
            Color color, Color shadowColor, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va)
            : this(viewBox, scrollSpeed, font, color, offset, ha, va)
        {
            _shadowColor = shadowColor;
            _useShadow = true;
        }

        public void Reset()
        {
            _currentScrollPosition.Reset();
        }

        public bool Repeating { get; set; }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Write(_font, _color, _shadowColor, _offset, _hAlign, _vAlign, _texts);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _currentScrollPosition.Value += _scrollSpeed * gameTime.ElapsedGameTime.Milliseconds / 17F;
            CheckBounds();
        }

        public Vector2 OffSet
        {
            get { return _offset; }
            set { _offset = value; }
        }
        
        private void Write(SpriteFont font, Color color, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va, params string[] texts)
        {
            if (texts == null)
                return;

            int index = 0;
            foreach (Vector2 pos in GetPositions(font, ha, va, texts))
            {
                var savedViewport = new Viewport();
                if (_viewBox != Rectangle.Empty)
                {
                    BeebappsGame.Current.SpriteBatch.End();
                    BeebappsGame.Current.SpriteBatch.Begin();
                    savedViewport = Game.GraphicsDevice.Viewport;
                    Viewport currentViewPort = Game.GraphicsDevice.Viewport;
                    currentViewPort.Width = _viewBox.Width;
                    currentViewPort.Height = _viewBox.Height;
                    currentViewPort.X = _viewBox.X;
                    currentViewPort.Y = _viewBox.Y;
                    Game.GraphicsDevice.Viewport = currentViewPort;
                }

                _positionTopLeft = pos + offset + _currentScrollPosition; // +new Vector2(100F, 0F);
                _isAreaSet = true;

                if ((_viewBox == Rectangle.Empty) || (_positionTopLeft.X < _viewBox.Width && _positionTopLeft.Y < _viewBox.Height &&
                    _positionTopLeft.X + GetTextWidth(font, texts[index]) >= 0 &&
                    _positionTopLeft.Y + GetTextHeight(font, texts[index]) >= 0))
                {

                    BeebappsGame.Current.SpriteBatch.DrawString(font, texts[index], _positionTopLeft, color, 0F, Vector2.Zero,
                        Vector2.One, SpriteEffects.None, 1F);
                }
                if (_viewBox != Rectangle.Empty)
                {
                    BeebappsGame.Current.SpriteBatch.End();
                    BeebappsGame.Current.SpriteBatch.Begin();
                    Game.GraphicsDevice.Viewport = savedViewport;
                }

                index++;
            }
        }

        private Vector2 _positionTopLeft;
        protected Vector2 PositionTopLeft
        {
            get { return _positionTopLeft; }
        }

        private void Write(SpriteFont font, Color color, Color shadowColor, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va, params string[] texts)
        {
            if (_useShadow)
            {
                //First the Shadow
                Write(font, shadowColor, offset + new Vector2(2), ha, va, texts);
            }
            //Then the real color
            Write(font, color, offset, ha, va, texts);
        }

        private void CheckBounds()
        {
            if (!Repeating)
                return;

            if (_currentScrollPosition.Value.X > _viewBox.Width)
                _currentScrollPosition.Value.X = -_textWidth;

            if (_currentScrollPosition.Value.Y > _viewBox.Height)
                _currentScrollPosition.Value.Y = -_viewBox.Height;

            if (_currentScrollPosition.Value.X < -_textWidth)
                _currentScrollPosition.Value.X = _viewBox.Width;

            if (_currentScrollPosition.Value.Y < -_textHeight)
                _currentScrollPosition.Value.Y = _viewBox.Height;
        }

        private IEnumerable<Vector2> GetPositions(SpriteFont font, HorizontalAlignment ha, VerticalAlignment va, params string[] texts)
        {
            float verticalPos = GetVerticalPosition(font, va, texts);
            foreach(string text in texts)
            {
                yield return
                    new Vector2(
                        GetHorizontalPosition(font, ha, text),
                        verticalPos);
                verticalPos += GetTextHeight(font, text);
            }
        }

        public void SetText(params string[] texts)
        {
            _texts = texts;
            _textHeight = GetTextHeight(_font, texts);
            _textWidth = GetTextWidth(_font, texts);
        }

        private float GetVerticalPosition(SpriteFont font, VerticalAlignment va, string[] texts)
        {
            switch (va)
            {
                case VerticalAlignment.Center:
                    return (GameHeight- GetTextHeight(font, texts)) / 2F;
                case VerticalAlignment.Top:
                    return 0F;
                case VerticalAlignment.Bottom:
                    return GameHeight - GetTextHeight(font, texts);
                default:
                    return (GameHeight - GetTextHeight(font, texts)) / 2F;
            }
        }

        private float GetHorizontalPosition(SpriteFont font, HorizontalAlignment ha, string text)
        {
            switch (ha)
            {
                case HorizontalAlignment.Center:
                    return (GameWidth - GetTextWidth(font, text)) / 2F;
                case HorizontalAlignment.Left:
                    return 0F;
                case HorizontalAlignment.Right:
                    return GameWidth - GetTextWidth(font, text);
                default:
                    return (GameWidth - GetTextWidth(font, text)) / 2F;
            }
        }

        private float GameWidth
        {
            get
            {
                return _gameWidth;
            }
        }

        private float GameHeight
        {
            get
            {
                return _gameHeight;
            }
        }

        private float GetTextHeight(SpriteFont font, string[] texts)
        {
            return texts.Sum(text => GetTextHeight(font, text));
        }

        private float GetTextWidth(SpriteFont font, string[] texts)
        {
            float width = 0F;
            foreach (string text in texts)
            {
                width = Math.Max(width, GetTextWidth(font, text));
            }
            return width;
        }

        private float GetTextWidth(SpriteFont font, string text)
        {
            if (text == string.Empty)
                return font.MeasureString("X").X;

            return font.MeasureString(text).X;
        }

        private float GetTextHeight(SpriteFont font, string text)
        {
            if (text == string.Empty)
                return font.MeasureString("X").Y;

            return font.MeasureString(text).Y;
        }

        private Rectangle _area = Rectangle.Empty;

        public Rectangle Area
        {
            get 
            {
                if (_area == Rectangle.Empty)
                {
                    _area = new Rectangle((int)PositionTopLeft.X, (int)PositionTopLeft.Y,
                        (int)_textWidth, (int)_textHeight);
                }
                return _area;
            }
        }

        private bool _isAreaSet;
        public bool IsAreaReady
        {
            get { return _isAreaSet; }
        }
    }
}
