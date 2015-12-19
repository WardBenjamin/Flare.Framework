using System;
using System.IO;
using System.Xml.Serialization;

namespace Flare.Framework.Graphics.Fonts
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------

    public class BitmapFontXMLSerializer
    {
        public static BitmapFontFile Load(String filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(BitmapFontFile));
            TextReader textReader = new StreamReader(filename);
            BitmapFontFile file = (BitmapFontFile)deserializer.Deserialize(textReader);
            textReader.Close();
            return file;
        }
    }
}