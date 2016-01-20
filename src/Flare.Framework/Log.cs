#region License
/* Flare - A Framework by Developers, for Developers.
 * Copyright 2016 Benjamin Ward
 * 
 * Released under the Creative Commons Attribution 4.0 International Public License
 * See LICENSE.md for details.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


namespace Flare
{
    public static class Log
    {
        private static StreamWriter stream;

        private static string filename;
        public static string FileName
        {
            get { return filename; }
            set
            {
                filename = value;
                stream = File.AppendText(filename);
            }
        }

        static Log()
        {
            stream = File.AppendText(filename);
        }

        public static void Write(string str)
        {
            lock (stream)
            {
                stream.Write("[{0}, {1}]: ", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                stream.Write(str);
                stream.WriteLine();
            }
        }
    }
}
