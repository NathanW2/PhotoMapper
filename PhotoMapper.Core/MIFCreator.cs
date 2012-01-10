using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        private void WritePoint(double x, double y, double direction)
        {
            mifwritter.WriteLine(string.Format("Point {0} {1}", x, y));
            mifwritter.WriteLine(String.Format("        Symbol (100,10157824,14,\"MapInfo Cartographic\",49,{0})", direction.ToString("0.0")));
        }

        private void WriteData(params string[] attibutes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in attibutes)
            {
                builder.AppendFormat("{0},", item);
            }
            string line = builder.ToString();
            line = line.TrimEnd(',');
            midwritter.WriteLine(line);
        }

        #region IDisposable Members

        public void Dispose()
        {
            midwritter.Close();
            mifwritter.Close();
        }

        #endregion

        public void WritePhoto(Picture file)
        {
            double x = file.GPSLongitude;
            double y = file.GPSLatitude;
            this.WritePoint(x, y, file.MapInfoDirection);
            this.WriteData(new string[] {
                "0", 
                file.Name.Trim(), 
                file.FilePath, 
                file.DateTimeOriginal.ToString("yyyyMMddHHmmssfff"), 
                file.DirectionRef, 
                file.MapInfoDirection.ToString(), 
                file.Direction.ToString()});
        }
    }
}
