using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Reflection;
using System.Diagnostics;

namespace PhotoMapper.Core
{
    public static class About
    {
        public static string AboutString
        {
            get
            {
                string about = 
                    @"PhotoMapper Utility by Nathan Woodrow
http://code.google.com/p/nathansmapinfoprojects/
http://woostuff.wordpress.com/

Allows processing of images that contain coordinates. 
Can output to MapInfo TAB and/or MID/MIF. 
At the moment it needs MapInfo Pro installed to create a TAB file,
however MapInfo is not needed if you are just creating a MID/MIF. 

PhotoMapper uses Microsoft VirtualEarth to allow for quick visualization of
photo locations at process time.

Use PhotoMapper.Cmd for batch/command line processing.

Change Set
Version 1.5.1.0
- Folder and File picker for input now remember last location.
- Can sync output path with input path.
- Added background logging.
- Supports updating image GPS info via command line.
    - See help file section 'Updating GPS Info' for details
- Output list of files with no GPS info via command line

Version info:

";
                StringBuilder builder = new StringBuilder();
                var names = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assm in names)
                {
                    //string name = assm.GetName().Name;
                    //assm.ImageRuntimeVersion
                    string version = FileVersionInfo.GetVersionInfo(assm.Location).FileVersion;
                    builder.Append(assm.GetName().Name + " " + version + "\n");
                }
                about += builder.ToString();
                return about;
            }
        }
    }
}
