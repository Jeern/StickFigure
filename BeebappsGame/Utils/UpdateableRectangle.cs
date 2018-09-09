using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{

    public struct UpdateableRectangle : IEquatable<UpdateableRectangle>
	{

        #region Private Fields

        private static readonly UpdateableRectangle EmptyRectangle;
        private readonly Func<float> _xCalculator;
        private readonly Func<float> _yCalculator;
        private readonly Func<float> _widthCalculator;
        private readonly Func<float> _heightCalculator;

		#endregion Private Fields


		#region Public Fields

		public float X;
		public float Y;
		public float Width;
		public float Height;

		#endregion Public Fields


		#region Public Properties

        public static UpdateableRectangle Empty
		{
			get { return EmptyRectangle; }
		}

		public float Left
		{
			get { return X; }
		}

		public float Right
		{
			get { return (X + Width); }
		}

		public float Top
		{
			get { return Y; }
		}

		public float Bottom
		{
			get { return (Y + Height); }
		}

		public Vector2 Center
		{
			get { return new Vector2(X + Width/2f, Y + Height/2f); }
		}

		#endregion Public Properties


		#region Constructors

        public UpdateableRectangle(Func<float> xCalculator, Func<float> yCalculator, Func<float> widthCalculator, Func<float> heightCalculator)
        {
            _xCalculator = xCalculator;
            _yCalculator = yCalculator;
            _widthCalculator = widthCalculator;
            _heightCalculator = heightCalculator;
            //Her ville det være smart bare at kalde Update - men det kan man ikke i en Struct
            X = _xCalculator();
            Y = _yCalculator();
            Width = _widthCalculator();
            Height = _heightCalculator();
        }

        #endregion Constructors


		#region Public Methods

        public void Update()
        {
            X = _xCalculator();
            Y = _yCalculator();
            Width = _widthCalculator();
            Height = _heightCalculator();
        }

        public static bool operator ==(UpdateableRectangle a, UpdateableRectangle b)
		{
			return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
		}

        public static bool operator !=(UpdateableRectangle a, UpdateableRectangle b)
		{
			return !(a == b);
		}

		public void Offset(Vector2 offset)
		{
			X += offset.X;
			Y += offset.Y;
		}

		public void Offset(float offsetX, float offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}

		public void Inflate(float horizontalValue, float verticalValue)
		{
			X -= horizontalValue;
			Y -= verticalValue;
			Width += horizontalValue * 2;
			Height += verticalValue * 2;
		}

        public bool Equals(UpdateableRectangle other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
            return (obj is UpdateableRectangle) && this == ((UpdateableRectangle)obj);
        }

		public override string ToString()
		{
			return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height);
		}

		public override int GetHashCode()
		{
			return Convert.ToInt32(X + Y + Width + Height);
		}

		public bool Intersects(UpdateableRectangle r2)
		{
			return !(r2.Left > Right
					 || r2.Right < Left
					 || r2.Top > Bottom
					 || r2.Bottom < Top
					);

		}


        public void Intersects(ref UpdateableRectangle value, out bool result)
		{
			result = !(value.Left > Right
					 || value.Right < Left
					 || value.Top > Bottom
					 || value.Bottom < Top
					);

		}

		public bool Contains(float x, float y)
		{
			return (Left <= x && Right >= x &&
					Top <= y && Bottom >= y);
		}

		public bool Contains(Vector2 value)
		{
			return (Left <= value.X && Right >= value.X &&
					Top <= value.Y && Bottom >= value.Y);
		}

		public void Contains(ref Vector2 value, out bool result)
		{
			result = (Left <= value.X && Right >= value.X &&
					  Top <= value.Y && Bottom >= value.Y);
		}

        public bool Contains(UpdateableRectangle value)
		{
			return (Left <= value.Left && Right >= value.Right &&
					Top <= value.Top && Bottom >= value.Bottom);
		}

        public void Contains(ref UpdateableRectangle value, out bool result)
		{
			result = (Left <= value.Left && Right >= value.Right &&
					  Top <= value.Top && Bottom >= value.Bottom);
		}

		#endregion Public Methods

        public static UpdateableRectangle Intersect(UpdateableRectangle rect1, UpdateableRectangle rect2)
		{
			if(!rect1.Intersects(rect2))
				return Empty;
			
			float x = Math.Max(rect1.X, rect2.X);
			float y = Math.Max(rect1.Y, rect2.Y);
			float width = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width) - x;
			float height = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height) - y;
			
			return
                new UpdateableRectangle(() => x, () => y, () => width, () => height);
		}
		
		public bool IsEmpty
		{
			get
			{
				return Equals(Empty);
			}
		}
		
		public Rectangle GetRectangle()
		{
			return new Rectangle(
				Convert.ToInt32(X),
				Convert.ToInt32(Y),
				Convert.ToInt32(Width),
				Convert.ToInt32(Height));
				
		}		
	}
}
