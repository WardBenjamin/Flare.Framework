using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using Flare.Framework.Utility;

namespace Flare.Framework.GUI
{
    class ClockHUD : Flare.Framework.Graphics.Sprite
    {
        public FrameClock clock;
        private ClockTexture cTex;

        public ClockHUD(int width, int height, Vector2 pos, int depth = 100)
        {
            clock = new FrameClock(depth);
            cTex = new ClockTexture(width, height);
            Texture = cTex;
        }

        public void BeginFrame()
        {
            clock.BeginFrame();
        }

        public void EndFrame()
        {
            clock.EndFrame();
            UpdateData();
        }

        public void Clear()
        {
            clock.Clear();
        }

        public void ClearBackground()
        {
            cTex.renderer.Clear(Color.Transparent);
        }

        public void UpdateData()
        {
            ClockData cd = new ClockData();
            cd.Data[ClockData.DataType.Time] = ClockData.FormatData(ClockData.DataType.Time, clock.TotalFrameTime.ToString());
            cd.Data[ClockData.DataType.Frame] = ClockData.FormatData(ClockData.DataType.Frame, clock.TotalFrameCount.ToString());
            cd.Data[ClockData.DataType.FPS] = ClockData.FormatData(ClockData.DataType.FPS, clock.FPS.ToString());
            cd.Data[ClockData.DataType.MinFPS] = ClockData.FormatData(ClockData.DataType.MinFPS, clock.MinFPS.ToString());
            cd.Data[ClockData.DataType.AvgFPS] = ClockData.FormatData(ClockData.DataType.AvgFPS, clock.AverageFPS.ToString());
            cd.Data[ClockData.DataType.MaxFPS] = ClockData.FormatData(ClockData.DataType.MaxFPS, clock.MaxFPS.ToString());
            cd.Data[ClockData.DataType.Delta] = ClockData.FormatData(ClockData.DataType.Delta, clock.LastFrameTime.ToString());
            cd.Data[ClockData.DataType.MinDelta] = ClockData.FormatData(ClockData.DataType.MinDelta, clock.MinFrameTime.ToString());
            cd.Data[ClockData.DataType.AvgDelta] = ClockData.FormatData(ClockData.DataType.AvgDelta, clock.AverageFrameTime.ToString());
            cd.Data[ClockData.DataType.MaxDelta] = ClockData.FormatData(ClockData.DataType.MaxDelta, clock.MaxFrameTime.ToString());
            cTex.UpdateData(cd);
        }

        internal class ClockTexture : Flare.Framework.Graphics.Texture
        {
            internal TextRenderer renderer;
            private Font font;
            private int textSize = 12;
            private PrivateFontCollection collection = new PrivateFontCollection();

            private ClockData lastData = new ClockData();

            public override int Width { get; protected set; }
            public override int Height { get; protected set; }
            internal override int TexID { get { return renderer.Texture; } set { /* Do nothing. */ } }

            internal ClockTexture(int width, int height) : base()
            {
                Width = width;
                Height = height;
                if (!File.Exists(@"Fonts/ClockHUD.ttf"))
                    throw new FileNotFoundException("Required font could not be loaded.", @"Fonts/ClockHUD.ttf");
                collection.AddFontFile(@"Fonts/ClockHUD.ttf");
                //font = new Font((FontFamily)collection.Families[0], textSize);
                font = new Font(FontFamily.GenericMonospace, textSize);
                renderer = new TextRenderer(width, height);

                DrawPositions = new Dictionary<ClockData.DataType, PointF>()
                {
                    { ClockData.DataType.Time,     new PointF(0, 0)               },
                    { ClockData.DataType.Frame,    new PointF(0, font.Height)     },
                    { ClockData.DataType.FPS,      new PointF(0, font.Height * 2) },
                    { ClockData.DataType.MinFPS,   new PointF(0, font.Height * 3) },
                    { ClockData.DataType.AvgFPS,   new PointF(0, font.Height * 4) },
                    { ClockData.DataType.MaxFPS,   new PointF(0, font.Height * 5) },
                    { ClockData.DataType.Delta,    new PointF(0, font.Height * 6) },
                    { ClockData.DataType.MinDelta, new PointF(0, font.Height * 7) },
                    { ClockData.DataType.AvgDelta, new PointF(0, font.Height * 8) },
                    { ClockData.DataType.MaxDelta, new PointF(0, font.Height * 9) }
                };
            }

            internal void UpdateData(ClockData data)
            {
                /*
                if (data.Data[ClockData.DataType.Time] != lastData.Data[ClockData.DataType.Time])
                    renderer.DrawString(data.Data[ClockData.DataType.Time], font, 
                        ClockData.DataColors[ClockData.DataType.Time], DrawPositions[ClockData.DataType.Time]);
                if (data.Data[ClockData.DataType.Frame] != lastData.Data[ClockData.DataType.Frame])
                    renderer.DrawString(data.Data[ClockData.DataType.Frame], font,
                        ClockData.DataColors[ClockData.DataType.Frame], DrawPositions[ClockData.DataType.Frame]);
                if (data.Data[ClockData.DataType.FPS] != lastData.Data[ClockData.DataType.FPS])
                    renderer.DrawString(data.Data[ClockData.DataType.FPS], font,
                        ClockData.DataColors[ClockData.DataType.FPS], DrawPositions[ClockData.DataType.FPS]);
                if (data.Data[ClockData.DataType.MinFPS] != lastData.Data[ClockData.DataType.MinFPS])
                    renderer.DrawString(data.Data[ClockData.DataType.MinFPS], font,
                        ClockData.DataColors[ClockData.DataType.MinFPS], DrawPositions[ClockData.DataType.MinFPS]);
                if (data.Data[ClockData.DataType.AvgFPS] != lastData.Data[ClockData.DataType.AvgFPS])
                    renderer.DrawString(data.Data[ClockData.DataType.AvgFPS], font,
                        ClockData.DataColors[ClockData.DataType.AvgFPS], DrawPositions[ClockData.DataType.AvgFPS]);
                if (data.Data[ClockData.DataType.MaxFPS] != lastData.Data[ClockData.DataType.MaxFPS])
                    renderer.DrawString(data.Data[ClockData.DataType.MaxFPS], font,
                        ClockData.DataColors[ClockData.DataType.MaxFPS], DrawPositions[ClockData.DataType.MaxFPS]);
                if (data.Data[ClockData.DataType.Delta] != lastData.Data[ClockData.DataType.Delta])
                    renderer.DrawString(data.Data[ClockData.DataType.Delta], font,
                        ClockData.DataColors[ClockData.DataType.Delta], DrawPositions[ClockData.DataType.Delta]);
                if (data.Data[ClockData.DataType.MinDelta] != lastData.Data[ClockData.DataType.MinDelta])
                    renderer.DrawString(data.Data[ClockData.DataType.MinDelta], font,
                        ClockData.DataColors[ClockData.DataType.MinDelta], DrawPositions[ClockData.DataType.MinDelta]);
                if (data.Data[ClockData.DataType.AvgDelta] != lastData.Data[ClockData.DataType.AvgDelta])
                    renderer.DrawString(data.Data[ClockData.DataType.AvgDelta], font,
                        ClockData.DataColors[ClockData.DataType.AvgDelta], DrawPositions[ClockData.DataType.AvgDelta]);
                if (data.Data[ClockData.DataType.MaxDelta] != lastData.Data[ClockData.DataType.MaxDelta])
                    renderer.DrawString(data.Data[ClockData.DataType.MaxDelta], font,
                        ClockData.DataColors[ClockData.DataType.MaxDelta], DrawPositions[ClockData.DataType.MaxDelta]);
                lastData = data;*/
                renderer.DrawString("Testing", font, ClockData.DataColors[ClockData.DataType.FPS], PointF.Empty);
            }

            private Dictionary<ClockData.DataType, PointF> DrawPositions;
        }

        internal class ClockData
        {
            internal enum DataType
            {
                Time, Frame,
                FPS, MinFPS, AvgFPS, MaxFPS,
                Delta, MinDelta, AvgDelta, MaxDelta
            }

            internal Dictionary<DataType, string> Data;

            internal ClockData()
            {
                Data = new Dictionary<DataType, string>()
                {
                    { DataType.Time, FormatData(DataType.Time, "") },
                    { DataType.Frame, FormatData(DataType.Frame, "") },
                    { DataType.FPS, FormatData(DataType.FPS, "") },
                    { DataType.MinFPS, FormatData(DataType.MinFPS, "")},
                    { DataType.AvgFPS, FormatData(DataType.AvgFPS, "")},
                    { DataType.MaxFPS,FormatData(DataType.MaxFPS, "") },
                    { DataType.Delta, FormatData(DataType.Delta, "") },
                    { DataType.MinDelta, FormatData(DataType.MinDelta, "") },
                    { DataType.AvgDelta, FormatData(DataType.AvgDelta, "") },
                    { DataType.MaxDelta, FormatData(DataType.MaxDelta, "") }
                };
            }

            internal static string FormatData(DataType type, string data)
            {

                const int titlePadding = 9, dataPadding = 6, unitPadding = 5;
                return type.ToString().PadRight(titlePadding) + ": "
                    + (data.Length > dataPadding ? data.Substring(0, dataPadding) : data.PadRight(dataPadding))
                    + DataUnits[type].PadRight(unitPadding);
            }

            internal static Dictionary<DataType, Brush> DataColors = new Dictionary<DataType, Brush>()
            {
                    { DataType.Time,     Brushes.Yellow},
                    { DataType.Frame,    Brushes.White     },
                    { DataType.FPS,      Brushes.LimeGreen },
                    { DataType.MinFPS,   Brushes.LimeGreen },
                    { DataType.AvgFPS,   Brushes.LimeGreen },
                    { DataType.MaxFPS,   Brushes.LimeGreen },
                    { DataType.Delta,    Brushes.DarkCyan  },
                    { DataType.MinDelta, Brushes.DarkCyan  },
                    { DataType.AvgDelta, Brushes.DarkCyan  },
                    { DataType.MaxDelta, Brushes.DarkCyan  }
            };

            private static Dictionary<DataType, string> DataUnits = new Dictionary<DataType, string>()
            {
                    { DataType.Time,     "(sec)" },
                    { DataType.Frame,    ""      },
                    { DataType.FPS,      ""      },
                    { DataType.MinFPS,   ""      },
                    { DataType.AvgFPS,   ""      },
                    { DataType.MaxFPS,   ""      },
                    { DataType.Delta,    "(ms)"  },
                    { DataType.MinDelta, "(ms)"  },
                    { DataType.AvgDelta, "(ms)"  },
                    { DataType.MaxDelta, "(ms)"  }
            };

            /*{ sf::Color::Yellow, format("Time",  "(sec)", m_clock->getTotalFrameTime().asSeconds())        },
            { sf::Color::White,  format("Frame", "",      m_clock->getTotalFrameCount())                   },
            { sf::Color::Green,  format("FPS",   "",      m_clock->getFramesPerSecond())                   },
            { sf::Color::Green,  format("min.",  "",      m_clock->getMinFramesPerSecond())                },
            { sf::Color::Green,  format("avg.",  "",      m_clock->getAverageFramesPerSecond())            },
            { sf::Color::Green,  format("max.",  "",      m_clock->getMaxFramesPerSecond())                },
            { sf::Color::Cyan,   format("Delta", "(ms)",  m_clock->getLastFrameTime().asMilliseconds())    },
            { sf::Color::Cyan,   format("min.",  "(ms)",  m_clock->getMinFrameTime().asMilliseconds())     },
            { sf::Color::Cyan,   format("avg.",  "(ms)",  m_clock->getAverageFrameTime().asMilliseconds()) },
            { sf::Color::Cyan,   format("max.",  "(ms)",  m_clock->getMaxtFrameTime().asMilliseconds())    }*/
        }
    }
}
/*
#include <vector>
#include <string>
#include <sstream>
#include <iomanip>

#include <SFML/Graphics.hpp>
#include "FrameClock.h"

class ClockHUD : public sf::Drawable
{
    struct Stat
    {
        sf::Color color;
        std::string str;
    };

    typedef std::vector<Stat> Stats_t;

public:

    ClockHUD(const sfx::FrameClock& clock, const sf::Font& font)
        : m_clock (&clock)
        , m_font  (&font)
    {}

private:

    void draw(sf::RenderTarget& rt, sf::RenderStates states) const
    {
        // Gather the available frame time statistics.
        const Stats_t stats = build();

        sf::Text elem;
        elem.setFont(*m_font);
        elem.setCharacterSize(16);
        elem.setPosition(5.0f, 5.0f);

        // Draw the available frame time statistics.
        for (std::size_t i = 0; i < stats.size(); ++i)
        {
            elem.setString(stats[i].str);
            elem.setColor(stats[i].color);

            rt.draw(elem, states);

            // Next line.
            elem.move(0.0f, 16.0f);
        }
    }

private:

    template<typename T>
    static std::string format(std::string name, std::string resolution, T value)
    {
        std::ostringstream os;
        os.precision(4);
        os << std::left << std::setw(5);
        os << name << " : ";
        os << std::setw(5);
        os << value << " " << resolution;
        return os.str();
    }

    Stats_t build() const
    {
        const int count = 10;
        const Stat stats[count] = {
            { sf::Color::Yellow, format("Time",  "(sec)", m_clock->getTotalFrameTime().asSeconds())        },
            { sf::Color::White,  format("Frame", "",      m_clock->getTotalFrameCount())                   },
            { sf::Color::Green,  format("FPS",   "",      m_clock->getFramesPerSecond())                   },
            { sf::Color::Green,  format("min.",  "",      m_clock->getMinFramesPerSecond())                },
            { sf::Color::Green,  format("avg.",  "",      m_clock->getAverageFramesPerSecond())            },
            { sf::Color::Green,  format("max.",  "",      m_clock->getMaxFramesPerSecond())                },
            { sf::Color::Cyan,   format("Delta", "(ms)",  m_clock->getLastFrameTime().asMilliseconds())    },
            { sf::Color::Cyan,   format("min.",  "(ms)",  m_clock->getMinFrameTime().asMilliseconds())     },
            { sf::Color::Cyan,   format("avg.",  "(ms)",  m_clock->getAverageFrameTime().asMilliseconds()) },
            { sf::Color::Cyan,   format("max.",  "(ms)",  m_clock->getMaxtFrameTime().asMilliseconds())    }
        };
        return Stats_t(&stats[0], &stats[0] + count);
    }

private:

    const sf::Font* m_font;
    const sfx::FrameClock* m_clock;
};
*/
