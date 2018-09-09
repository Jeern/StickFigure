using System;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.Utils
{

	public struct DoubleRectangle : IEquatable<DoubleRectangle>
	{
		#region Private Fields

		private static readonly DoubleRectangle EmptyRectangle;

		#endregion Private Fields


		#region Public Fields

		public double X;
		public double Y;
		public double Width;
		public double Height;

		#endregion Public Fields


		#region Public Properties

		public static DoubleRectangle Empty
		{
			get { return EmptyRectangle; }
		}

		public double Left
		{
			get { return X; }
		}

		public double Right
		{
			get { return (X + Width); }
		}

		public double Top
		{
			get { return Y; }
		}

		public double Bottom
		{
			get { return (Y + Height); }
		}

		public Vector2 Center
		{
			get { return new Vector2((float)(X + Width/2), (float)(Y + Height/2f)); }
		}

		#endregion Public Properties


		#region Constructors

		public DoubleRectangle(double x, double y, double width, double height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		#endregion Constructors


		#region Public Methods

		public static bool operator ==(DoubleRectangle a, DoubleRectangle b)
		{
			return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
		}

		public static bool operator !=(DoubleRectangle a, DoubleRectangle b)
		{
			return !(a == b);
		}

		public void Offset(Vector2 offset)
		{
			X += offset.X;
			Y += offset.Y;
		}

		public void Offset(double offsetX, double offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}

		public void Inflate(double horizontalValue, double verticalValue)
		{
			X -= horizontalValue;
			Y -= verticalValue;
			Width += horizontalValue * 2;
			Height += verticalValue * 2;
		}

		public bool Equals(DoubleRectangle other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			return (obj is DoubleRectangle) && this == ((DoubleRectangle)obj);
        }

		public override string ToString()
		{
			return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height);
		}

		public override int GetHashCode()
		{
			return Convert.ToInt32(X + Y + Width + Height);
		}

		public bool Intersects(DoubleRectangle r2)
		{
			return !(r2.Left > Right
					 || r2.Right < Left
					 || r2.Top > Bottom
					 || r2.Bottom < Top
					);

		}


		public void Intersects(ref DoubleRectangle value, out bool result)
		{
			result = !(value.Left > Right
					 || value.Right < Left
					 || value.Top > Bottom
					 || value.Bottom < Top
					);

		}

		public bool Contains(double x, double y)
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

		public bool Contains(DoubleRectangle value)
		{
			return (Left <= value.Left && Right >= value.Right &&
					Top <= value.Top && Bottom >= value.Bottom);
		}

		public void Contains(ref DoubleRectangle value, out bool result)
		{
			result = (Left <= value.Left && Right >= value.Right &&
					  Top <= value.Top && Bottom >= value.Bottom);
		}

		#endregion Public Methods
		
		public static DoubleRectangle Intersect(DoubleRectangle rect1, DoubleRectangle rect2)
		{
			if(!rect1.Intersects(rect2))
				return Empty;
			
			double x = Math.Max(rect1.X, rect2.X);
			double y = Math.Max(rect1.Y, rect2.Y);
			double width = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width) - x;
			double height = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height) - y;
			
			return 
				new DoubleRectangle(x,y,width,height);
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
