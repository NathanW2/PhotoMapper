using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PhotoMapper
{
    public class MapinfoWrapper : IDisposable
    {
        DMapInfo mapinfo;
        private MapinfoWrapper(DMapInfo mapInfo)
        {
            this.mapinfo = mapInfo;
        }

        public static MapinfoWrapper CreateInstance()
        {
            Type mapinfoType = Type.GetTypeFromProgID("MapInfo.Application");
            DMapInfo mapinfo = (DMapInfo)Activator.CreateInstance(mapinfoType);
            return new MapinfoWrapper(mapinfo);
        }

        public string Import(string mifpath)
        {
            string tabpath = Path.Combine(Path.GetDirectoryName(mifpath), Path.GetFileNameWithoutExtension(mifpath) + ".Tab");
            string importcommand = String.Format("Import \"{0}\" Into \"{1}\" Overwrite", mifpath,tabpath);
            mapinfo.Do(importcommand);
            return tabpath;
        }

        public void Save(string name, string outFullPath)
        {
            mapinfo.Do(string.Format("Commit Table {0} as \"{1}\"", name, outFullPath));
        }

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
