using System;
using System.IO;
using ExifLibrary;

namespace PhotoMapper.Core
{
    public class Picture
    {
        FileInfo fileinfo;
        ImageFile image;

        public Picture(string file)
        {
            this.fileinfo = new FileInfo(file);
            this.image = ImageFile.FromFile(file);
        }

        public string Name
        {
            get
            {
                return this.fileinfo.Name;
            }
        }

        public float GPSLatitude
        {
            get 
            {
                GPSLatitudeLongitude location = this.image[ExifTag.GPSLatitude] as GPSLatitudeLongitude;
                if ((GPSLatitudeRef)this.image[ExifTag.GPSLatitudeRef].Value == GPSLatitudeRef.South)
                    return location.ToFloat() * -1;
                else
                    return location.ToFloat();
                
            }
            set {
                throw new NotImplementedException("Setting GPS not supported yet");
            }
        }

        public float GPSLongitude
        {
            get {
                GPSLatitudeLongitude location = this.image[ExifTag.GPSLongitude] as GPSLatitudeLongitude;
                if ((GPSLongitudeRef)this.image[ExifTag.GPSLongitudeRef].Value == GPSLongitudeRef.West)
                    return location.ToFloat() * -1;
                else
                    return location.ToFloat();
            }
            set { throw new NotImplementedException("Setting GPS not supported yet"); }
        }

        public DateTime DateTimeOriginal
        {
            get 
            { 
                ExifDateTime datetime = this.image[ExifTag.DateTimeOriginal] as ExifDateTime;
                return datetime.Value;
            }    
        }

        public DateTime? GPSDateTime
        {
            get
            {
                try
                {
                    ExifDateTime datetime = this.image[ExifTag.GPSDateStamp] as ExifDateTime;
                    return datetime.Value;
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
            get { return this.fileinfo.FullName; }       
        }

        public string FilePath
        {
            get { return this.fileinfo.Directory.FullName; }
        }
        

        public string DirectionRef
        {
            get
            {
                if (this.HasCompassInfo)
                {
                    GPSDirectionRef directRef = (GPSDirectionRef)this.image[ExifTag.GPSImgDirectionRef].Value;
                    return directRef == GPSDirectionRef.TrueDirection ? "True North" : "Magnetic North";
                }
                return "No Compass Info";
            }
        }

        /// <summary>
        /// Returns the direction that works with MapInfo.  MapInfo uses back to front directions.
        /// </summary>
        public float MapInfoDirection
        {
            get
            {
                if (this.HasCompassInfo)
                {
                    return 360 - (this.image[ExifTag.GPSImgDirection] as ExifURational).ToFloat();
                }
                return 0;
            }
        }

        public float Direction
        {
            get
            {
                if (this.HasCompassInfo)
                {
                    return (this.image[ExifTag.GPSImgDirection] as ExifURational).ToFloat();
                }
                return 0;
            }
        }


        public bool HasCompassInfo
        {
            get
            {
                return this.image.Properties.ContainsKey(ExifTag.GPSImgDirection);
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
                return ((this.image.Properties.ContainsKey(ExifTag.GPSLatitude) && !ThrowsException(() => test = this.GPSLatitude)) &&
                        (this.image.Properties.ContainsKey(ExifTag.GPSLatitudeRef) && !ThrowsException(() => test = this.image[ExifTag.GPSLatitudeRef])) &&
                        (this.image.Properties.ContainsKey(ExifTag.GPSLongitude) && !ThrowsException(() => test = this.GPSLongitude)) &&
                        (this.image.Properties.ContainsKey(ExifTag.GPSLongitudeRef)) && !ThrowsException(() => test = this.image[ExifTag.GPSLongitudeRef]));
            }
        }

        public void Save()
        {
            this.image.Save(this.FileName);
        }

        public delegate void Action();

        public static bool ThrowsException(Action method)
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
