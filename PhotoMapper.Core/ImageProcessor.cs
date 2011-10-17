using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Enum.Extensions;

namespace PhotoMapper.Core
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

        public string GenerateMIFFile(string outPath,string outFileName, IEnumerable<Picture> pics)
        {
            this.ReportProgress("Generating MIF file");
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
                        stream.WriteData(0, file.Name.Trim(), file.DateTimeOriginal, file.DirectionRef, file.Direction);
                    }
                    else
                    {
                        this.ReportProgress("   " + file.Name + " : No GPS information found");
                    }
                }
                string miffile = stream.MIFFile;
                this.ReportProgress("Generated MIF file at " + miffile);
                return miffile;
            }
        }

        /// <summary>
        /// Generates a MapInfo TAB file from a MIF file.
        /// </summary>
        /// <param name="mifpath">The path to the MID/MIF file to be used to generate the TAB file.</param>
        /// <returns>The path to the generated TAB file.</returns>
        public string GenerateTABFile(string mifpath)
        {
            string tabpath;

            this.ReportProgress("Loading Mapinfo...");
            this.ReportProgress("Generating TAB file");

            using (MapinfoWrapper wrapper = MapinfoWrapper.CreateInstance())
            {
                tabpath = wrapper.Import(mifpath);
            }
            this.ReportProgress("Generated TAB file at " + tabpath);
            return tabpath;
        }

        /// <summary>
        /// Report the process of the current <see cref="ImageProcessor"/>
        /// </summary>
        /// <param name="msg">The message to report.</param>
        private void ReportProgress(string msg)
        {
            if (this.ProgessUpdated != null)
                this.ProgessUpdated(msg);
        }

        /// <summary>
        /// Processes a list of photos generating files supplied using the <see cref="FormatFlags"/>
        /// </summary>
        /// <param name="path">The output path for the files to be generated.</param>
        /// <param name="name">The name of the output file, multiple files will be created with different extensions.</param>
        /// <param name="pictures">The list of pictures to be processed.</param>
        /// <param name="flag">The file types that will be generated.</param>
        public void ProcessPictures(string path, string name, List<Picture> pictures, FormatFlags flag)
        {
            if (flag.Has(FormatFlags.MIF) && flag.Has(FormatFlags.TAB))
            {
                string mifpath = this.GenerateMIFFile(path, name, pictures);              
                string tabpath = this.GenerateTABFile(mifpath);
            }
            else if (flag == FormatFlags.MIF)
            {
                string mifpath = this.GenerateMIFFile(path, name, pictures);
            }
            else if (flag == FormatFlags.TAB)
            {
                string mifpath = this.GenerateMIFFile(path, name, pictures);
                string tabpath = this.GenerateTABFile(mifpath);
                this.ReportProgress("Deleting MIF file");
                File.Delete(mifpath);
            }
        }

        public IEnumerable<Picture> CheckForNullGPSInfo(List<Picture> pictures)
        {
            foreach (var picture in pictures)
            {
                if (!picture.HasGPSInformation)
                    yield return picture;
            }
        }

        private const string FileHeader = "FileName\tGPSLongitude\tGPSLatitude"; 

        /// <summary>
        /// Generates a tab delimited file that contains each image with null GPS info and a place to store the new coordinates.
        /// 
        /// This file can be directly reimported by the program. 
        /// </summary>
        /// <param name="pictures">The list of pictures to check.</param>
        /// <param name="outdir">The output path for the resulting text file.</param>
        /// <returns>A string containing the path to the new file.</returns>
        public string GenerateTxtFileForNullGPS(List<Picture> pictures, string outdir)
        {
            List<Picture> nullGPSInfo = new List<Picture>(this.CheckForNullGPSInfo(pictures));

            if (nullGPSInfo.Count == 0)
            {
                this.ReportProgress("No pictures with missing GPS information");
                return null;
            }

            string outfile = Path.Combine(outdir,"NullGPSInfo.txt");
            StreamWriter sw = new StreamWriter(outfile, false);

            sw.WriteLine(FileHeader);

            foreach (Picture picture in nullGPSInfo)
            {
                sw.WriteLine("{0}\t{1}\t{2}",picture.FileName,string.Empty,string.Empty);
            }

            this.ReportProgress("Generated file for missing GPS info files to " + outfile);

            sw.Close();

            return outfile;
        }

        public void UpdatePhotos(string infofile)
        {
            throw new NotImplementedException("Updating isn't supported yet");
            //StreamReader reader = new StreamReader(infofile);
            //string line = reader.ReadLine();
            
            ////If we are a header line just read the next line.
            //if (line == FileHeader)
            //    line = reader.ReadLine();

            //while(!String.IsNullOrEmpty(line))
            //{
            //    string[] values = line.Split('\t');

            //    string filename = values[0];
            //    string gpsLongitude = values[1];
            //    string gpsLatitude = values[2];
                
            //    Picture image = new Picture(filename)
            //                        {
            //                            GPSLongitude = Convert.ToDouble(gpsLongitude),
            //                            GPSLatitude = Convert.ToDouble(gpsLatitude)
            //                        };
            //    image.Save();

            //    line = reader.ReadLine();
            }
        }
    }
