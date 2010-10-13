using System;
using System.IO;
using log4net;

namespace PhotoMapper.Core
{
    public class MapinfoWrapper : IDisposable
    {
        /// <summary>
        /// Get the logger for this class.
        /// </summary>
        private static readonly ILog log = Logging.GetLog(typeof(MapinfoWrapper));

        DMapInfo mapinfo;
        private MapinfoWrapper(DMapInfo mapInfo)
        {
            this.mapinfo = mapInfo;
        }

        /// <summary>
        /// Creates and instance of MapInfo.
        /// </summary>
        /// <returns>A instance of MapInfo if it is installed.  Returns null if a error was raised.</returns>
        public static MapinfoWrapper CreateInstance()
        {
            try
            {
                Type mapinfoType = Type.GetTypeFromProgID("MapInfo.Application");
                DMapInfo mapinfo = (DMapInfo)Activator.CreateInstance(mapinfoType);
                return new MapinfoWrapper(mapinfo);
            }
            catch (Exception ex)
            {
                log.Error("MapInfo could not be opened",ex);
                return null;
            }
            
        }

        /// <summary>
        /// Imports a MapInfo MIF file and creates a MapInfo TAB file with the same name.
        /// </summary>
        /// <param name="mifpath">The path to the MIF file.</param>
        /// <returns></returns>
        public string Import(string mifpath)
        {
            string tabpath = Path.Combine(Path.GetDirectoryName(mifpath), Path.GetFileNameWithoutExtension(mifpath) + ".Tab");
            string importcommand = String.Format("Import \"{0}\" Into \"{1}\" Overwrite", mifpath,tabpath);
            mapinfo.Do(importcommand);
            return tabpath;
        }

        /// <summary>
        /// Saves a open table. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="outFullPath"></param>
        public void Save(string name, string outFullPath)
        {
            mapinfo.Do(string.Format("Commit Table {0} as \"{1}\"", name, outFullPath));
        }

        /// <summary>
        /// Closes the MapInfo session.
        /// </summary>
        public void CloseSession()
        {
            this.mapinfo.Do("End Mapinfo");
        }

        public void Dispose()
        {
            this.CloseSession();
        }
    }
}
