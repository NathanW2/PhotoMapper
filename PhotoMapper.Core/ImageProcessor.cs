using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Enum.Extensions;

namespace PhotoMapper
{
    public class ImageProcessor
    {
        [Flags]
        public enum FormatFlags
        {
            None = 0,
            MIF = 1,
            TAB = 2
        }

        IEnumerable<Picture> pics;
        public event Action<string> ProgessUpdated;

        public ImageProcessor()
        {

        }

        public string GenerateMIFFile(string outPath,string outFileName, IEnumerable<Picture> pics)
        {
            using (MIFCreator stream = new MIFCreator(outPath, outFileName))
            {
                stream.WriteHeader();
                Dictionary<string, string> columns = new Dictionary<string, string>();
                columns.Add("ID", "Integer");
                columns.Add("FilePath", "Char(50)");
                columns.Add("Date", "DateTime");
                columns.Add("Direction_Ref", "Char(20)");
                columns.Add("Direction_AntiClock", "Float");

                stream.WriteColumns(columns);

                foreach (Picture file in pics)
                {
                    if (file.HasGPSInformation)
                    {
                        this.ReportProgress("   Working on ->" + file.Name);
                        double x = file.GPSLongitude;
                        double y = file.GPSLatitude;
                        stream.WritePoint(x, y, file.Direction);
                        stream.WriteData(0, file.Name, file.DateTimeOriginal, file.DirectionRef, file.Direction);
                    }
                    else
                    {
                        this.ReportProgress("   " + file.Name + " : No GPS information found");
                    }
                }

                return stream.MIFFile;
            }
        }

        public string GenerateTABFile(string mifpath)
        {
            string tabpath;

            this.ReportProgress("Loading Mapinfo...");
            using (MapinfoWrapper wrapper = MapinfoWrapper.CreateInstance())
            {
                tabpath = wrapper.Import(mifpath);
            }
            return tabpath;
        }

        private void ReportProgress(string msg)
        {
            if (this.ProgessUpdated != null)
                this.ProgessUpdated(msg);
        }

        public void ProcessPictures(string path, string name, List<Picture> pictures, FormatFlags flag)
        {
            if (flag.Has(FormatFlags.MIF) && flag.Has(FormatFlags.TAB))
            {
                this.ReportProgress("Generating MIF file");
                string mifpath = this.GenerateMIFFile(path, name, pictures);
                this.ReportProgress("Generated MIF file at " + mifpath);
                this.ReportProgress("Generating TAB file");
                string tabpath = this.GenerateTABFile(mifpath);
                this.ReportProgress("Generated TAB file at " + tabpath);
            }
            else if (flag == FormatFlags.MIF)
            {
                this.ReportProgress("Generating MIF file");
                string mifpath = this.GenerateMIFFile(path, name, pictures);
                this.ReportProgress("Generated MIF file at " + mifpath);
            }
            else if (flag == FormatFlags.TAB)
            {
                string mifpath = this.GenerateMIFFile(path, name, pictures);
                this.ReportProgress("Generating TAB file");
                string tabpath = this.GenerateTABFile(mifpath);
                this.ReportProgress("Generated TAB file at " + tabpath);
                File.Delete(mifpath);
            }
        }
    }
}
