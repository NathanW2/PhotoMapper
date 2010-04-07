using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Exiv2Net;

namespace PhotoMapper
{
    public class Picture
    {
        FileInfo fileinfo;
        Image image;

        public Picture(string file)
        {
            this.fileinfo = new FileInfo(file);
            this.image = new Image(file);
        }

        public string Name
        {
            get
            {
                return this.fileinfo.Name;
            }
        }

        public double GPSLatitude
        {
            get {return this.image.GPSLatitude; }
            set { this.image.GPSLatitude = value; }
        }

        public double GPSLongitude
        {
            get { return this.image.GPSLongitude; }
            set { this.image.GPSLongitude = value; }
        }

        public DateTime DateTimeOriginal
        {
            get { return this.image.DateTimeOriginal; }
                
        }

        public DateTime? GPSDateTime
        {
            get
            {
                try
                {
                    return this.image.GPSDateTime;
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
            }
            set
            {
                this.GPSDateTime = value;
            }
        }

        public string FileName
        {
            get { return this.image.FileName; }       
        }
        

        public string DirectionRef
        {
            get
            {
                if (this.HasCompassInfo)
                {
                    string value = (this.image["Exif.GPSInfo.GPSImgDirectionRef"] as Exiv2Net.AsciiString).Value;
                    return value == "T" ? "True North" : "Magnetic North";
                }
                else
                {
                    return "No Compass Info";
                }
            }
        }

        public double Direction
        {
            get
            {
                if (this.HasCompassInfo)
                {
                    return 360 - (this.image["Exif.GPSInfo.GPSImgDirection"] as Exiv2Net.UnsignedRational).Value[0].ToDouble();
                }
                else
                {
                    return 0.0;
                }
            }
        }



        public bool HasCompassInfo
        {
            get
            {
                return this.image.ContainsKey("Exif.GPSInfo.GPSImgDirection");
            }
        }

        /// <summary>
        /// Returns true if the this.image has vaild GPS info.
        /// </summary>
        /// <remarks>Overrides the HasGPSInformation in the base class that checked for DateTime.</remarks>
        public bool HasGPSInformation
        {
            get
            {
                object test;
                return ((this.image.ContainsKey(Tags.GPSLatitude) && !ThrowException(() => test = this.GPSLatitude)) &&
                        (this.image.ContainsKey(Tags.GPSLatitudeRef) && !ThrowException(() => test = this.image[Tags.GPSLatitudeRef])) &&
                        (this.image.ContainsKey(Tags.GPSLongitude) && !ThrowException(() => test = this.GPSLongitude)) &&
                        (this.image.ContainsKey(Tags.GPSLongitudeRef)) && !ThrowException(() => test = this.image[Tags.GPSLongitudeRef]));
            }
        }

        public delegate void Action();

        public static bool ThrowException(Action method)
        {
            try
            {
                method.Invoke();
                return false;
            }
            catch (Exception)
            {
                return true;            
            }
        }
    }
}
