using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flare.Framework.Graphics;

namespace Flare.Framework.Graphics.Fonts
{
    public class BitmapFont
    {
        private Dictionary<char, BitmapFontRegion> regions;

        public Texture Texture;
        public int LineHeight;
        public Vector2 TextureSize;

        public BitmapFont(string filename) : this(filename + ".fnt", filename + "_0.png") { }

        public BitmapFont(string xmlfilename, string bitmapfilename) : this(BitmapFontXMLSerializer.Load(xmlfilename), new Bitmap(bitmapfilename)) { }

        public BitmapFont(BitmapFontFile file, Bitmap bitmap)
        {
            var regions = GenerateRegions(file);
            this.regions = regions.ToDictionary(r => r.Character);
            Texture = new Texture(bitmap);
            LineHeight = file.Common.LineHeight;
            TextureSize = new Vector2(bitmap.Width, bitmap.Height);
        }

        private List<BitmapFontRegion> GenerateRegions(BitmapFontFile file)
        {
            var regions = new List<BitmapFontRegion>();
            foreach (var character in file.Chars)
            {
                BitmapFontRegion region = new BitmapFontRegion()
                {
                    Character = (char)character.ID,
                    Rect = new Rectangle(character.X, character.Y, character.Width, character.Height),
                    XOffset = character.XOffset,
                    YOffset = character.YOffset,
                    XAdvance = character.XAdvance
                };
                regions.Add(region);
            }
            return regions;
        }

        public BitmapFontRegion GetCharacterRegion(char character)
        {
            BitmapFontRegion region;
            return regions.TryGetValue(character, out region) ? region : null;
        }

        public Vector2 MeasureString(string text)
        {
            var width = 0;
            var height = 0;

            foreach (var c in text)
            {
                BitmapFontRegion fontRegion;

                if (regions.TryGetValue(c, out fontRegion))
                {
                    width += fontRegion.XAdvance;

                    if (fontRegion.Rect.Height + fontRegion.YOffset > height)
                        height = fontRegion.Rect.Height + fontRegion.YOffset;
                }
            }

            return new Vector2(width, height);
        }
    }
}
/*
namespace MonoGame.Extended.BitmapFonts
{
    public class BitmapFont 
    {
        internal BitmapFont(IEnumerable<BitmapFontRegion> regions, int lineHeight)
        {
            _characterMap = regions.ToDictionary(r => r.Character);// BuildCharacterMap(textures, _fontFile);
            LineHeight = lineHeight;
        }

        private readonly Dictionary<char, BitmapFontRegion> _characterMap;

        public int LineHeight { get; private set; }

        public BitmapFontRegion GetCharacterRegion(char character)
        {
            BitmapFontRegion region;
            return _characterMap.TryGetValue(character, out region) ? region : null;
        }

        public Rectangle GetStringRectangle(string text, Vector2 position)
        {
            var width = 0;
            var height = 0;

            foreach (var c in text)
            {
                BitmapFontRegion fontRegion;

                if (_characterMap.TryGetValue(c, out fontRegion))
                {
                    width += fontRegion.XAdvance;

                    if (fontRegion.Height + fontRegion.YOffset > height)
                        height = fontRegion.Height + fontRegion.YOffset;
                }
            }

            var p = position.ToPoint();
            return new Rectangle(p.X, p.Y, width, height);
        }
    }
}*/
