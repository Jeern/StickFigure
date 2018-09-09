//using System;
//using Beebapps.Game.Input;
//using Microsoft.Xna.Framework;

//namespace Beebapps.Game.Utils
//{
//    //PinchableArea kigger på touches. Hvis der er 2 eller flere opfattes det som en potentiel Zoom ind, eller Zoom out.
//    //Dette afgøres ved at måle afstanden imellem de 2 positioner. Ved et enkelt touch opfattes det som at fokus flyttes i stedet
//    public class PinchableArea : GameComponent
//    {
//        private FloatRectangle PinchArea { get; set; }
//        public Camera InternalCamera  { get; private set; }
//        private float MinZoom { get; set; }
//        private float MaxZoom { get; set; }
//        public float CurrentZoom { get; set; }
//        private float _previousPinch;
//        private readonly Vector2 _areaSizeVector;
//        private Vector2 _previousTouchPosition;
//        private static readonly Vector2 NonInitialized = new Vector2(float.MinValue, float.MinValue);

//        public PinchableArea(FloatRectangle pinchArea, float minZoom, float maxZoom, float currentZoom) : base(BeebappsGame.Current)
//        {
//            if (currentZoom > maxZoom)
//                throw new ArgumentException("currentZoom must be <= maxZoom", "currentZoom");
//            if (currentZoom < minZoom)
//                throw new ArgumentException("currentZoom must be >= minZoom", "currentZoom");

//            PinchArea = pinchArea;
//            InternalCamera = Camera.Empty;
//            MinZoom = minZoom;
//            MaxZoom = maxZoom;
//            CurrentZoom = currentZoom;
//            _areaSizeVector = new Vector2(pinchArea.Width, pinchArea.Height);
//            _previousTouchPosition = NonInitialized;
//        }

//        public override void Update(GameTime gameTime)
//        {
//            float currentPinch = PinchTouch.Current.CurrentPinch;
//            if(currentPinch > 0 && _previousPinch > 0 && Math.Abs(currentPinch - _previousPinch) > 0f) //0 er usikkerhed på pinch
//            {
//                float previousZoom = CurrentZoom;
//                CurrentZoom = (currentPinch/_previousPinch)*CurrentZoom;
//                CurrentZoom = Math.Min(CurrentZoom, MaxZoom);
//                CurrentZoom = Math.Max(CurrentZoom, MinZoom);
//                if(CurrentZoom > 0 && previousZoom > 0)
//                {
//                    Vector2 cameraMove;
//                    if(CurrentZoom > previousZoom)
//                    {
//                        cameraMove = ((CurrentZoom - 1f)/2f)*_areaSizeVector;
//                        InternalCamera.Position = InternalCamera.OriginalPosition + cameraMove;
//                    }
//                    else if (CurrentZoom < previousZoom)
//                    {
//                        cameraMove = ((CurrentZoom - 1f) / 2f) * _areaSizeVector;
//                        Vector2 tmpCameraPos = InternalCamera.OriginalPosition + cameraMove;
//                        InternalCamera.Position = KeepWithinArea(tmpCameraPos);
//                        InternalCamera.OriginalPosition = InternalCamera.OriginalPosition + InternalCamera.Position - tmpCameraPos;
//                    }
//                }
//            }
//            else if(PinchTouch.Current.IsDoubleClick && SingleTouchIsWithinArea)
//            {
//                float previousZoom = CurrentZoom;
//                if(CurrentZoom == MaxZoom)
//                {
//                    CurrentZoom = MinZoom;
//                }
//                else
//                {
//                    CurrentZoom = MaxZoom;
//                }
//                if (CurrentZoom > 0 && previousZoom > 0)
//                {
//                    Vector2 cameraMove;
//                    if (CurrentZoom > previousZoom)
//                    {
//                        cameraMove = ((CurrentZoom - 1f) / 2f) * _areaSizeVector;
//                        InternalCamera.Position = InternalCamera.OriginalPosition + cameraMove;
//                    }
//                    else if (CurrentZoom < previousZoom)
//                    {
//                        cameraMove = ((CurrentZoom - 1f) / 2f) * _areaSizeVector;
//                        Vector2 tmpCameraPos = InternalCamera.OriginalPosition + cameraMove;
//                        InternalCamera.Position = KeepWithinArea(tmpCameraPos);
//                        InternalCamera.OriginalPosition = InternalCamera.OriginalPosition + InternalCamera.Position - tmpCameraPos;
//                    }
//                }
//                Vector2 previousCameraPos = InternalCamera.Position;
//                InternalCamera.Position = (PinchTouch.Current.ClickPosition - new Vector2(PinchArea.X, PinchArea.Y));
//                InternalCamera.Position = KeepWithinArea(InternalCamera.Position);
//                InternalCamera.OriginalPosition = InternalCamera.OriginalPosition + InternalCamera.Position - previousCameraPos;
//            }
//            else if (PinchTouch.Current.IsSingleTouchPressed && SingleTouchIsWithinArea)
//            {
//                _previousTouchPosition = PinchTouch.Current.SingleTouchPosition;
//            }
//            else if (PinchTouch.Current.IsSingleTouchMoving && SingleTouchIsWithinArea)
//            {
//                if(_previousTouchPosition.Equals(NonInitialized))
//                {
//                    _previousTouchPosition = PinchTouch.Current.SingleTouchPosition;
//                }
//                else
//                {
//                    Vector2 previousCameraPos = InternalCamera.Position;
//                    InternalCamera.Position -= (PinchTouch.Current.SingleTouchPosition - _previousTouchPosition);
//                    InternalCamera.Position = KeepWithinArea(InternalCamera.Position);
//                    InternalCamera.OriginalPosition = InternalCamera.OriginalPosition + InternalCamera.Position - previousCameraPos;
//                }

//                _previousTouchPosition = PinchTouch.Current.SingleTouchPosition;
//            }
//            else
//            {
//                _previousTouchPosition = NonInitialized;
//            }
//            _previousPinch = currentPinch;
//        }



//        private bool SingleTouchIsWithinArea
//        {
//            get
//            {
//                return 
//                    PinchTouch.Current.SingleTouchPosition.X >= PinchArea.X &&
//                       PinchTouch.Current.SingleTouchPosition.X <= PinchArea.X + PinchArea.Width &&
//                       PinchTouch.Current.SingleTouchPosition.Y >= PinchArea.Y &&
//                       PinchTouch.Current.SingleTouchPosition.Y <= PinchArea.Y + PinchArea.Height;
//            }
//        }

//        /// <summary> 
//        /// Based on the 2d line intersection method from "comp.graphics.algorithms Frequently Asked Questions" 
//        /// </summary> 
//        /// <param name="currentLocation">The current location.</param> 
//        /// <param name="requestedLocation">The requested location.</param> 
//        /// <param name="boundary">The boundary.</param> 
//        /// <param name="collisionVector">The collision vector.</param> 
//        /// <returns></returns> 
//        private bool Intersects(Vector2 currentLocation, Vector2 requestedLocation, Vector2 boundaryStart, Vector2 boundaryEnd, ref Vector2 collisionVector)
//        {
//            float q = (currentLocation.Y - boundaryStart.Y) * (boundaryEnd.X - boundaryStart.X) - (currentLocation.X - boundaryStart.X) * (boundaryEnd.Y - boundaryStart.Y);
//            float d = (requestedLocation.X - currentLocation.X) * (boundaryEnd.Y - boundaryStart.Y) - (requestedLocation.Y - currentLocation.Y) * (boundaryEnd.X - boundaryStart.X);

//            if (d == 0)
//            {
//                return false;
//            }

//            float r = q / d;

//            q = (currentLocation.Y - boundaryStart.Y) * (requestedLocation.X - currentLocation.X) - (currentLocation.X - boundaryStart.X) * (requestedLocation.Y - currentLocation.Y);
//            float s = q / d;

//            if (r < 0 || r > 1 || s < 0 || s > 1)
//            {
//                return false;
//            }

//            collisionVector.X = currentLocation.X + (int)(0.5f + r * (requestedLocation.X - currentLocation.X));
//            collisionVector.Y = currentLocation.Y + (int)(0.5f + r * (requestedLocation.Y - currentLocation.Y));
//            return true;
//        } 

//        /// <summary>
//        /// Hvis position falder uden for zoom området rykker vi den ind.
//        /// </summary>
//        /// <param name="position"></param>
//        /// <returns></returns>
//        private Vector2 KeepWithinArea(Vector2 position)
//        {
//            float xPos = Math.Min(Math.Max(position.X, 0), (CurrentZoom-1f)*PinchArea.Width);
//            float yPos = Math.Min(Math.Max(position.Y, 0), (CurrentZoom-1f) * PinchArea.Height);
//            return new Vector2(xPos, yPos);
//        }
//    }
//}
