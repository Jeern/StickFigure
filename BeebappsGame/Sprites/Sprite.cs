using System.Diagnostics;
using System.Linq;
using Beebapps.Game.GraphicUtils;
using Beebapps.Game.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Beebapps.Game.Sprites
{
    public abstract class Sprite : DrawableGameComponent
    {
        private readonly Texture2D _texture;

        //Local
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }

        //Global
        public Vector2 GlobalPosition { get; private set; }
        public Vector2 GlobalScale { get; private set; }
        public float GlobalRotation { get; private set; }


        //Benyttes udelukkende til situationer hvor der er Subsprites attached til parent. Lavt nummer tegnes først
        public int LocalDrawOrder { get; private set; }
        public Sprite Parent { get; private set; }

        public readonly List<Sprite> ChildSprites = new List<Sprite>();
        public readonly SortedList<int, Sprite> SpritesToDraw = new SortedList<int, Sprite>();

        protected Sprite(string texture, Vector2 localPosition, Vector2 localScale, float localRotation, int localDrawOrder)
            : base(BeebappsGame.Current)
        {
            _texture = BeebappsGame.Current.Content.Load<Texture2D>(texture);
            Position = localPosition;
            Scale = localScale;
            Rotation = localRotation;
            LocalDrawOrder = localDrawOrder;
        }

        public Color Color { get; set; }

        public override void Update(GameTime gameTime)
        {
            CalculateChildSpritesToDraw();
            foreach (var sprite in SpritesToDraw)
            {
                sprite.Value.ClearGlobalMatrix();
            }
            CalculateGlobalMatrix();
            CalculateGlobalPositionsFromMatrix();
        }

        private void CalculateGlobalPositionsFromMatrix()
        {
            Vector2 globalPosition;
            float globalRotation;
            Vector2 globalScale;
            DecomposeMatrix3(GlobalTransform, out globalPosition, out globalRotation, out globalScale);
            GlobalPosition = globalPosition;
            GlobalRotation = globalRotation;
            GlobalScale = globalScale;
        }

        private void DecomposeMatrix(Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            if (!matrix.Decompose(out scale3, out rotationQ, out position3))
            {
                Vector3 donotUse;
                Matrix.Invert(matrix).Decompose(out scale3, out rotationQ, out donotUse);
                scale3 = new Vector3(1 / scale3.X, 1 / scale3.Y, 1 / scale3.Z);
                rotationQ = Quaternion.Inverse(rotationQ);
                Debug.WriteLine(LocalDrawOrder);
            }
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2(direction.Y, direction.X);
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }

        private void DecomposeMatrix2(Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            if (!matrix.Decompose(out scale3, out rotationQ, out position3))
            {
                if (Parent == null)
                {
                    rotation = Rotation;
                    scale = Scale;
                }
                else
                {
                    rotation = Rotation + Parent.GlobalRotation;
                    scale = Scale * Parent.GlobalScale;
                }
            }
            else
            {
                Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
                rotation = (float)Math.Atan2(direction.Y, direction.X);
                scale = new Vector2(scale3.X, scale3.Y);
            }
            position = new Vector2(position3.X, position3.Y);
        }

        private void DecomposeMatrix3(Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            if (Parent == null)
            {
                rotation = Rotation;
                scale = Scale;
            }
            else
            {
                rotation = Rotation + Parent.GlobalRotation;
                scale = Scale * Parent.GlobalScale;
            }
            position = new Vector2(position3.X, position3.Y);
        }

        private void CalculateGlobalMatrix()
        {
            if (GlobalTransformCalculated)
                return;

            if (Parent == null)
            {
                GlobalTransform = LocalTransform;
            }
            else
            {
                Parent.CalculateGlobalMatrix();
                GlobalTransform = LocalTransform * Parent.GlobalTransform;
            }
            GlobalTransformCalculated = true;
        }

        private void CalculateChildSpritesToDraw()
        {
            if (Parent != null || SpritesToDraw.Count > 0)
                return;

            var allSprites = new List<Sprite>();
            allSprites.Add(this);
            if (ChildSprites.Count > 0)
            {
                foreach (var childSprite in GetAllChildren())
                {
                    allSprites.Add(childSprite);
                }
            }
            var allSpritesOrdered = allSprites.OrderBy(sprite => sprite.LocalDrawOrder);
            int drawOrder = 0;
            foreach (var sprite in allSpritesOrdered)
            {
                drawOrder++;
                SpritesToDraw.Add(drawOrder, sprite);
            }
        }

        public void ClearGlobalMatrix()
        {
            GlobalTransformCalculated = false;
        }

        public IEnumerable<Sprite> GetAllChildren()
        {
            foreach (var childSprite in ChildSprites)
            {
                foreach (var subChildSprite in childSprite.GetAllChildren())
                {
                    yield return subChildSprite;
                }
                yield return childSprite;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //En sprite med en parent tegner ikke sig selv. Da den vil blive tegnet af sin parent.
            //Parent tegner alle children inkl. sig selv i Draworder
            if (Parent != null || SpritesToDraw.Count == 0)
                return;

            foreach (var sprite in SpritesToDraw)
            {
                sprite.Value.DrawTexture();
            }
        }

        public void DrawTexture()
        {
            BeebappsGame.Current.SpriteBatch.Draw(_texture,
               GlobalPosition,
               null,
               Color,
               GlobalRotation,
               Vector2.Zero,
               GlobalScale,
               SpriteEffects.None,
               1f);
        }

        public void AddChildSprite(Sprite sprite)
        {
            sprite.Parent = this;
            ChildSprites.Add(sprite);
        }

        public virtual Vector2 Middle
        {
            get { return new Vector2(Width / 2F, Height / 2F); }
        }

        public float Height
        {
            get
            {
                return _texture.Height;
            }
        }

        public float Width
        {
            get { return _texture.Width; }
        }

        public bool GlobalTransformCalculated { get; private set; }
        public Matrix GlobalTransform { get; private set; }

        public Matrix LocalTransform
        {
            get
            {
                // Transform = -Origin * Scale * Rotation * Translation
                return Matrix.CreateTranslation(-Width / 2f, -Height / 2f, 0f) *
                       Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                       Matrix.CreateRotationZ(Rotation) *
                       Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            }
        }

        public void InitialIgnoreParent()
        {
            InitialOffsetPositionFromMiddleOfParent();
            InitialIgnoreParentScale();
        }

        public void InitialOffsetPositionFromMiddleOfParent()
        {
            if (Parent == null)
                throw new ArgumentException("Must have parent");

            Position = new Vector2(Position.X + Parent.Width / 2f, Position.Y + Parent.Height / 2f);
        }

        public void InitialIgnoreParentScale()
        {
            if (Parent == null)
                throw new ArgumentException("Must have parent");

            Scale = Scale / Parent.Scale;
        }
    }
}
