using System;
using System.Collections.Generic;
using System.Linq;
using Beebapps.Game.Sequencers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Beebapps.Game.GraphicUtils
{
    public class ImageState : List<GameImage>
    {
        public ImageState(GameImage image)  : this(StateChangeType.None, new List<GameImage> { image })
        {
        }

        public ImageState(StateChangeType changeType, IEnumerable<GameImage> images)
            : this(changeType, images.ToArray())
        {
        }

        public ImageState(StateChangeType changeType, params GameImage[] images)
        {
            AddRange(images);
            SetImageChanger(changeType);
            Reset();
        }


        public GameImage Current
        {
            get { return this[CurrentImageIndex]; }
        }

        public void SetImageChanger(StateChangeType changeType)
        {
            switch (changeType)
            {
                case StateChangeType.None:
                    ImageChanger = new StaticSequencer();
                    break;
                case StateChangeType.Alternating:
                    ImageChanger = new AlternatingSequencer(Count-1);
                    break;
                case StateChangeType.Forwarding:
                    ImageChanger = new ForwardingSequencer(Count-1);
                    break;
                case StateChangeType.Random:
                    ImageChanger = new RandomSequencer(Count-1);
                    break;
                case StateChangeType.Repeating:
                    ImageChanger = new RepeatingSequencer(Count-1);
                    break;
                default:
                    ImageChanger = new StaticSequencer();
                    break;
            }
        }

        public Sequencer ImageChanger
        {
            get;
            private set;
        }

        public static implicit operator Texture2D(ImageState state)
        {
            return state.CurrentTexture;
        }

        private TimeSpan _lastChanged = TimeSpan.MinValue;

        public void Update(GameTime time)
        {
            TimeSpan newTime = time.TotalGameTime;
            if (_lastChanged == TimeSpan.MinValue || _lastChanged.Add(new TimeSpan(0, 0, 0, 0, Current.Delay)) <= newTime)
            {
                _lastChanged = newTime;
                ImageChanger.MoveNext();
            }
        }

        public Texture2D CurrentTexture
        {
            get { return Current.Texture; }
        }

        public int CurrentImageIndex
        {
            get {
                return ImageChanger.Current; }
        }

        public void Reset()
        {
            _lastChanged = TimeSpan.MinValue;
            ImageChanger.Reset();
        }


    }
}
