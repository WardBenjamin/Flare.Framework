using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Flare.Framework.Utility;
using Flare.Framework.Graphics;
using Flare.Framework.Graphics.Fonts;
using OpenTK;

namespace Flare.GUI
{
    public class ClockDisplay
    {
        private bool first = true;

        public FrameClock clock;
        public Vector2 Position;

        private BitmapFont font;
        private Dictionary<Data, Text> TextItems = new Dictionary<Data, Text>()
        {
            { Data.Time, null },
            { Data.Frame, null },
            { Data.FPS, null },
            { Data.MinFPS, null },
            { Data.AvgFPS, null },
            { Data.MaxFPS, null },
            { Data.Delta, null },
            { Data.MinDelta, null },
            { Data.AvgDelta, null },
            { Data.MaxDelta, null }
        };

        public ClockDisplay(FrameClock clock, BitmapFont font, Vector2 position)
        {
            this.clock = clock;
            this.font = font;
            this.Position = position;

            GenerateText();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            GenerateText();
            foreach (var item in TextItems.Values)
            {
                spriteBatch.AddString(item);
            }
            spriteBatch.DrawStrings();
        }

        private void GenerateText()
        {
            float dy = 0.0f;
            foreach (Data data in Enum.GetValues(typeof(Data)))
            {
                TextItems[data] = new Text(font, GetInfo(data), Position + new OpenTK.Vector2(0, dy));
                //Console.WriteLine("Text Pos: " + (Position + new OpenTK.Vector2(0, dy)));
                dy += font.MeasureString(data.ToString()).Y;
            }
        }

        private string GetInfo(Data key)
        {
            const string format = "{0,-10} {1, 5}";
            string ret = key.ToString() + ":";
            switch (key)
            {
                case Data.Time:
                    return String.Format(format, ret, clock.TotalFrameTime) + " (ms)";
                case Data.Frame:
                    return String.Format(format, ret, clock.TotalFrameCount);
                case Data.FPS:
                    return String.Format(format, ret, clock.FPS);
                case Data.MinFPS:
                    return String.Format(format, ret, clock.MinFPS);
                case Data.AvgFPS:
                    return String.Format(format, ret, clock.AverageFPS);
                case Data.MaxFPS:
                    return String.Format(format, ret, clock.MaxFPS);
                case Data.Delta:
                    return String.Format(format, ret, clock.LastFrameTime) + " (ms)";
                case Data.MinDelta:
                    return String.Format(format, ret, clock.MinFrameTime) + " (ms)";
                case Data.AvgDelta:
                    return String.Format(format, ret, clock.AverageFrameTime) + " (ms)";
                case Data.MaxDelta:
                    return String.Format(format, ret, clock.MaxFrameTime + " (ms)");
                default:
                    return "invalid key";
            }
        }

        private enum Data
        {
            Time, Frame,
            FPS, MinFPS, AvgFPS, MaxFPS,
            Delta, MinDelta, AvgDelta, MaxDelta
        }
    }
}
