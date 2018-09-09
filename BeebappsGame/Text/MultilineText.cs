using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beebapps.Game.Sprites;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Beebapps.Game.Text
{
    public class MultilineText : DrawableGameComponent, ISpatial
    {
        public Vector2 Position { get; set; }
        public Color Color { get; private set; }
        public SpriteFont Font { get; private set; }
        public float LineSpacing { get; private set; }
        public HorizontalAlignment HorizontalAlignment { get; private set; }
        public float Height { get; private set; }
        public float Width { get; private set; }
        private VisualString[] _textLines;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="position"></param>
        /// <param name="lineSpacing"></param>
        /// <param name="ha"></param>
        /// <param name="maxWordWrapWidth">use 0 or less if no wordwrap</param>
        /// <param name="lines"></param>
        public MultilineText(Color color, Vector2 position, float lineSpacing, HorizontalAlignment ha, float maxWordWrapWidth, params VisualString[] lines)
            : base(BeebappsGame.Current)
        {
            Color = color;
            Position = position;
            LineSpacing = lineSpacing;
            HorizontalAlignment = ha;
            TextLines = maxWordWrapWidth > 0 ? WordWrap(lines, maxWordWrapWidth) : lines;
        }

        /// <summary>
        /// If this is set linespacing will be ignored instead the Line Height will be fixed to this
        /// </summary>
        public float RequiredLineHeight { get; set; }

        public VisualString[] TextLines 
        {
            get { return _textLines; }
            set 
            { 
                _textLines = value;
                if (value == null)
                    return;
                Vector2 position = Vector2.Zero;
                Width = GetTextWidth(_textLines);
                Height = GetTextHeight(_textLines);
                foreach (VisualString text in _textLines)
                {
                    CalculatePosition(text, position, Width, HorizontalAlignment);
                    if (RequiredLineHeight > 0)
                    {
                        position += new Vector2(0, RequiredLineHeight);
                    }
                    else
                    {
                        position += new Vector2(0, LineSpacing + GetTextHeight(text));
                    }
                }
            }
        }

        private void CalculatePosition(VisualString text, Vector2 position, float textWidth, HorizontalAlignment ha)
        {
            switch (ha)
            {
                case HorizontalAlignment.Left:
                    text.Position = position;
                    break;
                case HorizontalAlignment.Center:
                    text.Position = position + new Vector2((textWidth - GetTextWidth(text)) / 2, 0);
                    break;
                case HorizontalAlignment.Right:
                    text.Position = position + new Vector2(textWidth - GetTextWidth(text), 0);
                    break;
                default:
                    throw new ArgumentException("Invalid value for HorizontalAlignment in CalculatePosition", "ha");
            }
        }

        private float GetTextHeight(VisualString[] texts)
        {
            if (texts == null)
                return 0f;

            if (RequiredLineHeight > 0)
                return RequiredLineHeight * texts.Length;

            float height = texts.Sum(text => GetTextHeight(text) + LineSpacing);
            return height - LineSpacing;
        }

        private float GetTextWidth(VisualString[] texts)
        {
            if (texts == null)
                return 0f;

            float width = 0F;
            foreach (VisualString text in texts)
            {
                width = Math.Max(width, GetTextWidth(text));
            }
            return width;
        }

        private float GetTextWidth(VisualString text)
        {
            if (text.Caption == string.Empty)
                return text.Font.MeasureString("H").X;

            return text.Font.MeasureString(text.Caption).X;
        }

        private float MaxFontSize { get; set; }
        private float GetTextHeight(VisualString text)
        {
            if (text.Caption == string.Empty)
                return Math.Max(text.Font.MeasureString("X").Y, MaxFontSize);

            float lineHeight = text.Font.MeasureString(text.Caption).Y;
            MaxFontSize = Math.Max(lineHeight, MaxFontSize);
            return lineHeight;
        }

        private VisualString[] WordWrap(VisualString[] textLines, float maxWidth)
        {
            if (textLines == null)
                return null;

            var texts = new List<VisualString>();
            foreach (VisualString text in textLines)
            {
                texts.AddRange(WordWrap(text, maxWidth));
            }
            return texts.ToArray();

        }

        /// <summary>
        /// WordWrap takes the texts that are larger than maxWidth and splits them up until they are smaller.
        /// If only one word is left though it is used no matter what.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        private List<VisualString> WordWrap(VisualString text, float maxWidth)
        {
            var texts = new List<VisualString>();
            float length = text.Font.MeasureString(text.Caption).X;
            if (length <= maxWidth || !text.Caption.Contains(' '))
            {
                texts.Add(text);
            }
            else
            {
                //First we find the first part of the text which is smaller than maxWidth
                string[] words = text.Caption.Trim().Split(' ');
                var sb = new StringBuilder();
                string firstLine = null;
                foreach(string word in words)
                {
                    sb.Append(word);
                    if(text.Font.MeasureString(sb.ToString()).X < maxWidth)
                    {
                        firstLine = sb.ToString();
                    }
                    else
                    {
                        break;
                    }
                    sb.Append(' ');
                }

                if(firstLine == null)
                {
                    firstLine = words[0];
                }

                string restOfTheLines = text.Caption.Replace(firstLine, string.Empty).Trim();
                texts.Add(new VisualString(firstLine, text.Font));
                if (firstLine.StartsWith("\b"))
                {
                    restOfTheLines = "\b" + restOfTheLines;
                }
                texts.AddRange(WordWrap(new VisualString(restOfTheLines, text.Font), maxWidth));
            }
            return texts;
        }

        public override void Draw(GameTime gameTime)
        {
            if (TextLines == null)
                return;

            foreach (VisualString text in TextLines)
            {
                BeebappsGame.Current.SpriteBatch.DrawString(text.Font, text.Caption, Position + text.Position, Color, 0F, Vector2.Zero,
                    1f, SpriteEffects.None, 1F);
            }
        }


    }
}
