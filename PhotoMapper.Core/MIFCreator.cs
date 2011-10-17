using System;
using System.Collections.Generic;
using System.IO;

namespace PhotoMapper.Core
{
    public class MIFCreator : IDisposable
    {
        StreamWriter mifwritter;
        StreamWriter midwritter;

        public MIFCreator(string path, string fileName)
        {
            // Create the directory if it doesn't exsits.
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            mifwritter = new StreamWriter(Path.Combine(path,fileName + ".mif"),false);
            midwritter = new StreamWriter(Path.Combine(path, fileName + ".mid"),false);

            this.MIFFile = Path.Combine(path,fileName + ".mif");
        }

        public string MIFFile { get; private set; }

        public void WriteHeader()
        {
            mifwritter.WriteLine("Version 900");
            mifwritter.WriteLine("Charset \"WindowsLatin1\"");
            mifwritter.WriteLine("Delimiter \",\"");
            mifwritter.WriteLine("CoordSys Earth Projection 1, 104");
        }

        public void WriteColumns(Dictionary<string,string> columns)
        {
            mifwritter.WriteLine("Columns " + columns.Count);
            foreach (var item in columns)
            {
                mifwritter.WriteLine("    " + item.Key + " " + item.Value);
            }
            mifwritter.WriteLine("Data");
        }

        public void WritePoint(double x, double y,double direction)
        {
            mifwritter.WriteLine(string.Format("Point {0} {1}", x, y));
            mifwritter.WriteLine(String.Format("        Symbol (100,10157824,14,\"MapInfo Cartographic\",49,{0})", direction.ToString("0.0")));
        }

        public void WriteData(int p, string file, DateTime date,string directionref, double mapinfoDirection, double direction)
        {
            midwritter.WriteLine(p + "," + file + "," + date.ToString("yyyyMMddHHmmssfff") + "," + directionref + "," + mapinfoDirection + "," + direction);
        }

        #region IDisposable Members

        public void Dispose()
        {
            midwritter.Close();
            mifwritter.Close();
        }

        #endregion


    }
}
